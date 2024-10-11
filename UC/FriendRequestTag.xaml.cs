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

using MessApp.Controller;
using MessApp.DB.Model;
using MessApp.Config;

namespace MessApp.UC
{
    /// <summary>
    /// Interaction logic for FriendTag.xaml
    /// </summary>
    public partial class FriendRequestTag : UserControl
    {
        private readonly AccountModel _account;
        private readonly FriendController _friendController;

        public FriendRequestTag(AccountModel account)
        {
            InitializeComponent();

            _account = account;
            _friendController = new FriendController();

            LoadFriendInfo();
        }

        private void LoadFriendInfo()
        {
            FriendName.Text = _account.lastName + " " + _account.firstName;
            gender.Text = _account.isFemale ? "Nữ" : "Nam";
            phone.Text = _account.phone;
        }

        private async void btn_Accept_Clicked(object sender, RoutedEventArgs e)
        {
            await _friendController.AcceptFriendRequest(_account.user_id, LoginState.Instance.Get());
        }

        private async void btn_Reject_Clicked(object sender, RoutedEventArgs e)
        {
            await _friendController.RejectFriendRequest(_account.user_id, LoginState.Instance.Get());
        }
    }
}
