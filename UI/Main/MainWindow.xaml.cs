﻿using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
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
using MessApp.Controller;
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
        private readonly FriendDao _friendDao;

        private readonly AccountController _accountController;

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

            _accountController = new AccountController();

            TypingArea.IsEnabled = false;

            LoadConversationList();
        }

        /// <summary>
        /// 
        /// </summary>
        private async void LoadConversationList()
        {
            List<ConversationModel>conversations = await Task.Run(() => _conversationDao.GetAllConversationsByUID(_currUser));

            foreach (ConversationModel conversation in conversations)
            {
                ParticipantModel participant = await Task.Run(() => _participantDao.GetParticipant(_currUser, conversation.conversation_id));

                Dispatcher.Invoke(() =>
                {
                    var conversationTag = new ConversationTag(conversation, participant);
                    conversationTag.OnConversationTagClick += OnFriendTagClicked;
                    Tags.Children.Add(conversationTag);
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async void LoadPeopleList(string keyword)
        {
            Tags.Children.Clear();

            var result = await _accountDao.GetAccountByPhone(keyword);

            if(result == null || result.user_id == _currUser)
            {
                var emptyTag = new TextBlock();
                emptyTag.Text = "Không có ai ở đây";
                emptyTag.VerticalAlignment = VerticalAlignment.Center;
                emptyTag.HorizontalAlignment = HorizontalAlignment.Center;
                Tags.Children.Add(emptyTag);
            }
            else
            {
                var peopleTag = new PeopleTag(_currUser, result);
                Tags.Children.Add(peopleTag);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user_id"></param>
        private async void LoadFriendRequestList(int user_id)
        {
            var friend_list = await _accountController.GetAllFriendRequest(user_id);

            foreach (var friend in friend_list)
            {
                Dispatcher.Invoke(() =>
                {
                    var friendRequestTag = new FriendRequestTag(friend);

                    friendRequestTag.onResponseRequest += () =>
                    {
                        Tags.Children.Clear();
                        LoadFriendRequestList(_currUser);
                    };

                    Tags.Children.Add(friendRequestTag);
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conversation"></param>
        private async void LoadMessages(ConversationModel conversation)
        {
            var messages = await Task.Run(() => _messageDao.GetAllMessagesByCID(conversation.conversation_id));

            foreach (MessageModel message in messages)
            {
                if(message.sender_id == _currUser)
                {
                    var userMessageTag = new UserMessageTag(message);
                    messageTags.Children.Add(userMessageTag);
                }
                else
                {
                    var friendMessageTag = new FriendMessageTag(message, _accountDao);
                    messageTags.Children.Add(friendMessageTag);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="conversation"></param>
        private async void OnFriendTagClicked (object sender, ConversationModel conversation)
        {
            // Lấy thông tin người tham gia cuộc trò chuyện
            var temp = await Task.Run(() => _participantDao.GetParticipant(_currUser, conversation.conversation_id));

            if (conversation.name == null) // Nếu người dùng đã rename cuộc trò chuyện sẽ hiển thị tên đã được thay đổi
                Username.Text = conversation.name;
            else // Nếu người dùng chưa thay đổi tên cuộc trò chuyện sẽ hiển thị tên người trò chuyện với người dùng
                Username.Text = temp.chatname;

            // Xóa hết UI cuộc trò chuyện trc khi chuyển qua cuộc trò chuyện khác
            messageTags.Children.Clear();
            // Sau khi chọn cuộc trò chuyện TypingArea mới cho phép nhập
            TypingArea.IsEnabled = true;
            btn_Call.Visibility = Visibility.Visible;

            // Lưu trữ id cuộc trò chuyện mới được chọn
            _currConversationId = conversation.conversation_id;

            // Dừng stream cuộc trò chuyện cũ
            _currMessageStream?.Dispose();

            // Load cuộc trò chuyện mới
            LoadMessages(conversation);
            // Bắt đầu stream cuộc trò chuyện mới
            StartMessageUpdates(conversation);
        }

        /// <summary>
        /// Start Real Time Update Message in Conversation
        /// </summary>
        /// <param name="conversation"></param>
        private void StartMessageUpdates(ConversationModel conversation)
        {
            _messageDao.StartMessageStream(conversation.conversation_id, (MessageModel newMessage) =>
            {
                // Update the UI on the main thread
                Dispatcher.Invoke(() =>
                {
                    if (newMessage.sender_id == _currUser)
                    {
                        var userMessageTag = new UserMessageTag(newMessage);
                        messageTags.Children.Add(userMessageTag);
                    }
                    else
                    {
                        var friendMessageTag = new FriendMessageTag(newMessage, _accountDao);
                        messageTags.Children.Add(friendMessageTag);
                    }
                });
            });
        }

        /// <summary>
        /// Handle Send Action
        /// </summary>
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

        /// <summary>
        /// Handle Search Action
        /// </summary>
        private async void SearchPeopleAction()
        {
            // TODO
            var keyword = SearchPeople.Text;

            LoadPeopleList(keyword);
        }

        private void btn_Call_Clicked(object sender, RoutedEventArgs e)
        {
            CallWindow callWindow = new CallWindow();
            callWindow.Show();
        }
        
        private void btn_Conversation_Clicked(object sender, RoutedEventArgs e)
        {
            TypingArea.IsEnabled = false;
            PageTitle.Text = "Conversation";
            btn_CreateConversation.Visibility = Visibility.Visible;
            btn_Call.Visibility = Visibility.Hidden;

            SearchBar.Visibility = Visibility.Visible;
            SearchPeople.Text = "";
            SearchPeople.Visibility = Visibility.Collapsed;
            SearchConversation.Text = "";
            SearchConversation.Visibility = Visibility.Visible;

            Tags.Children.Clear();
            messageTags.Children.Clear();

            LoadConversationList();
        }

        private void btn_FindPeople_Clicked(object sender, RoutedEventArgs e)
        {
            TypingArea.IsEnabled = false;
            PageTitle.Text = "Find People";
            btn_CreateConversation.Visibility = Visibility.Hidden;
            btn_Call.Visibility = Visibility.Hidden;

            SearchBar.Visibility = Visibility.Visible;
            SearchPeople.Text = "";
            SearchPeople.Visibility = Visibility.Visible;
            SearchConversation.Text = "";
            SearchConversation.Visibility = Visibility.Collapsed;

            Tags.Children.Clear();
            messageTags.Children.Clear();
        }

        private void btn_FriendRequest_Clicked(object sender, RoutedEventArgs e)
        {
            TypingArea.IsEnabled = false;
            PageTitle.Text = "Friend Request";
            btn_CreateConversation.Visibility = Visibility.Hidden;
            btn_Call.Visibility = Visibility.Hidden;

            SearchBar.Visibility = Visibility.Hidden;

            Tags.Children.Clear();
            messageTags.Children.Clear();

            LoadFriendRequestList(_currUser);
        }

        private void btn_Setting_Clicked(object sender, RoutedEventArgs e)
        {
            TypingArea.IsEnabled = false;

            SettingWindow settingWindow = new SettingWindow();
            settingWindow.ShowDialog();
            if (LoginState.Instance.Get() == 0)
                this.Close();
        }

        private void btn_Send_Clicked(object sender, RoutedEventArgs e)
        {
            SendAction();
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TypingArea.IsFocused)
                    SendAction();
                else if (SearchPeople.IsFocused)
                    SearchPeopleAction();
            }
        }

        private void SearchFriend_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}