using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MessApp.DB.Dao;
using MessApp.DB.Model;
using MessApp.DB;
using MessApp.Config;

namespace MessApp.UC
{
    /// <summary>
    /// Interaction logic for MessageTag.xaml
    /// </summary>
    public partial class MessageTag : UserControl
    {
        public MessageModel Message {  get; private set; }
        private readonly AccountDao _accountDao;
        public MessageTag(MessageModel message, AccountDao accountDao)
        {
            InitializeComponent();
            _accountDao = accountDao;
            Message = message;

            LoadMessage();
        }

        private async void LoadMessage()
        {
            var senderAccount = await Task.Run(() => _accountDao.GetAccountByUID(Message.sender_id));

            // Cập nhật giao diện sau khi có kết quả
            if (senderAccount != null)
            {
                SenderName.Text = senderAccount.firstName;
                MessageContent.Text = Message.message;
            }
            else
            {
                SenderName.Text = "Unknown Sender";
                MessageContent.Text = Message.message;
            }
        }
    }
}
