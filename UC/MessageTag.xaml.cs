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
        public MessageTag(MessageModel message)
        {
            InitializeComponent();
            _accountDao = new AccountDao(new MongoDBClient(new DBConfig()));
            Message = message;
            SenderName.Text = _accountDao.GetAccountByUID(message.sender_id).firstName;
            MessageContent.Text = message.message;
        }
    }
}
