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

namespace MessApp.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _currUser;
        private int _currConversationId;
        private readonly ConversationDao _conversationDao;
        public MainWindow()
        {
            InitializeComponent();
            _currUser = LoginState.Instance.Get();
            _conversationDao = new ConversationDao(new MongoDBClient(new DBConfig()));
            LoadConversation();
        }

        private void btn_Call_Clicked(object sender, RoutedEventArgs e)
        {
            CallWindow callWindow = new CallWindow();
            callWindow.Show();
        }

        public void LoadConversation()
        {
            List<ConversationModel>conversations = _conversationDao.GetAllConversationsByUID(_currUser);

            foreach (ConversationModel conversation in conversations)
            {
                var friendTag = new FriendTag(conversation);
                friendTag.OnFriendTagClick += OnFriendTagClicked;
                friendTags.Children.Add(friendTag);
            }    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conversation"></param>
        public void LoadMessages(ConversationModel conversation)
        {
            List<MessageModel> messages = _conversationDao.GetAllMessagesByCID(conversation.conversation_id);

            foreach (MessageModel message in messages)
            {
                var messageTag = new MessageTag(message);
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

            _currConversationId = conversation.conversation_id;
            LoadMessages(conversation);
            StartRealTimeUpdates(conversation);
        }

        private void btn_FriendReq_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Start Real Time Update Conversation
        /// </summary>
        /// <param name="conversation"></param>
        private void StartRealTimeUpdates(ConversationModel conversation)
        {
            _conversationDao.StartMessageStream(conversation.conversation_id, (MessageModel newMessage) =>
            {
                // Update the UI on the main thread
                Dispatcher.Invoke(() =>
                {
                    var messageTag = new MessageTag(newMessage);
                    messageTags.Children.Add(messageTag);
                });
            });
        }

        private void btn_Send_Clicked(object sender, RoutedEventArgs e)
        {
            SendAction();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && TypingArea.IsFocused)
            {
                SendAction();
            }
        }

        private void SendAction()
        {
            string messageContent = TypingArea.Text;

            if (!string.IsNullOrEmpty(messageContent))
            {
                var newMessage = new MessageModel
                {
                    message_id = _conversationDao.CountMessages() + 1,
                    message = messageContent,
                    sender_id = _currUser,
                    conversation_id = _currConversationId,
                    send_At = DateTime.Now,
                };

                _conversationDao.InsertNewMessage(newMessage);

                TypingArea.Text = string.Empty;
            }
        }
    }
}