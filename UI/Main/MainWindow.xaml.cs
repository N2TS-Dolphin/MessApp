using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MessApp.Config;
using MessApp.DB;
using MessApp.DB.Dao;
using MessApp.DB.Model;
using MessApp.UC;
using MessApp.UI.Sub;

namespace MessApp.UI.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MongoDBClient _dbClient;
        private int _currUser;
        private int _currConversationId;

        private readonly ConversationDao _conversationDao;
        private readonly ParticipantDao _participantDao;
        private readonly MessageDao _messageDao;
        private readonly AccountDao _accountDao;

        private IDisposable _currMessageStream;

        public MainWindow(int user_id)
        {
            InitializeComponent();

            _currUser = user_id;

            _dbClient = new MongoDBClient(new DBConfig());
            _conversationDao = new ConversationDao(_dbClient);
            _participantDao = new ParticipantDao(_dbClient);
            _messageDao = new MessageDao(_dbClient);
            _accountDao = new AccountDao(_dbClient);

            LoadConversation();
        }

        private void btn_Call_Clicked(object sender, RoutedEventArgs e)
        {
            CallWindow callWindow = new CallWindow();
            callWindow.Show();
        }

        private async void LoadConversation()
        {
            List<ConversationModel>conversations = await Task.Run(() => _conversationDao.GetAllConversationsByUID(_currUser));

            foreach (ConversationModel conversation in conversations)
            {
                ParticipantModel participant = await Task.Run(() => _participantDao.GetParticipant(_currUser, conversation.conversation_id));

                Dispatcher.Invoke(() =>
                {
                    var friendTag = new FriendTag(conversation, participant);
                    friendTag.OnFriendTagClick += OnFriendTagClicked;
                    friendTags.Children.Add(friendTag);
                });
            }    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conversation"></param>
        private async void LoadMessages(ConversationModel conversation)
        {
            List<MessageModel> messages = await Task.Run(() => _messageDao.GetAllMessagesByCID(conversation.conversation_id));

            foreach (MessageModel message in messages)
            {
                var messageTag = new MessageTag(message, _accountDao);
                messageTags.Children.Add(messageTag);
            }
        }

        /// <summary>
        /// Get Message in Chosen Conversation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="conversation"></param>
        private void OnFriendTagClicked (object sender, ConversationModel conversation)
        {
            Username.Text = conversation.name;
            messageTags.Children.Clear();
            TypingArea.IsEnabled = true;

            _currConversationId = conversation.conversation_id;

            // Dừng stream cuộc hội thoại
            _currMessageStream?.Dispose();

            LoadMessages(conversation);
            StartRealTimeUpdates(conversation);
        }

        private void btn_FriendReq_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Send_Clicked(object sender, RoutedEventArgs e)
        {
            SendAction();
        }

        private void btn_Setting_Clicked(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.ShowDialog();
            if(LoginState.Instance.Get()==0)
            {
                this.Close();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TypingArea.IsFocused)
                    SendAction();
                else if (SearchConversation.IsFocused)
                    SearchAction();
            }
        }


        /// <summary>
        /// Start Real Time Update Conversation
        /// </summary>
        /// <param name="conversation"></param>
        private void StartRealTimeUpdates(ConversationModel conversation)
        {
            _messageDao.StartMessageStream(conversation.conversation_id, (MessageModel newMessage) =>
            {
                // Update the UI on the main thread
                Dispatcher.Invoke(() =>
                {
                    var messageTag = new MessageTag(newMessage, _accountDao);
                    messageTags.Children.Add(messageTag);
                });
            });
        }

        private async void SendAction()
        {
            string messageContent = TypingArea.Text;

            if (!string.IsNullOrEmpty(messageContent))
            {
                var messageCount = await _messageDao.CountMessages();

                var newMessage = new MessageModel
                {
                    message_id = (int)messageCount + 1,
                    message = messageContent,
                    sender_id = _currUser,
                    conversation_id = _currConversationId,
                    send_At = DateTime.Now,
                };

                await _messageDao.AddMessage(newMessage);

                TypingArea.Text = string.Empty;
            }
        }

        private void SearchAction()
        {
            // Do Nothing
        }

        private void btn_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btn_Resize_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();

            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                bitmap.UriSource = new Uri("pack://application:,,,/MessApp;component/Resources/Images/maximize.png");
                img_Resize.Source = bitmap;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                bitmap.UriSource = new Uri("pack://application:,,,/MessApp;component/Resources/Images/shrink.png");
                img_Resize.Source = bitmap;
            }

            bitmap.EndInit();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}