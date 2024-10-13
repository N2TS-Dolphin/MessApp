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
using ZstdSharp.Unsafe;

using MessApp.Config;
using MessApp.DB;
using MessApp.DB.Dao;
using MessApp.DB.Model;
using MessApp.Controller;

namespace MessApp.UC
{
    /// <summary>
    /// Interaction logic for PeopleTag.xaml
    /// </summary>
    public partial class PeopleTag : UserControl
    {
        private int _currUser;
        private AccountModel _account;

        private readonly FriendDao _friendDao;
        private readonly FriendController _friendController;

        public PeopleTag(int currUser, AccountModel account)
        {
            InitializeComponent();

            _friendDao = new FriendDao(new MongoDBClient(new DBConfig()));
            _friendController = new FriendController();

            _currUser = currUser;
            _account = account;

            PeopleName.Text = _account.lastName + " " + _account.firstName;

            var Status = (TextBlock)btn_AddFriend.Template.FindName("Status", btn_AddFriend);
            if (GetStatus())
            {
                btn_AddFriend.IsEnabled = false;
                if (Status != null)
                    Status.Text = "Đã là bạn bè";
            }
            else
            {
                btn_AddFriend.IsEnabled = true;
                if (Status != null)
                    Status.Text = "Gửi lời mời kết bạn";
            }    

        }

        private bool GetStatus()
        {
            var result = _friendDao.GetFriend(_currUser, _account.user_id);

            if (result != null)
                return true;
            else
                return false;
        }

        private async void btn_AddFriend_Clicked(object sender, RoutedEventArgs e)
        {
            btn_AddFriend.IsEnabled = false; 
            var Status = (TextBlock)btn_AddFriend.Template.FindName("Status", btn_AddFriend);

            Status.Text = "Đã gửi lời mời";

            await _friendController.CreateNewFriendRequest(_currUser, _account.user_id);
        }
    }
}
