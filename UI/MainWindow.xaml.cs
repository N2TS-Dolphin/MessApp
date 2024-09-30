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

namespace MessApp.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _currUser;
        public MainWindow()
        {
            InitializeComponent();
            _currUser = LoginState.Instance.Get();
        }

        private void btn_Call_Clicked(object sender, RoutedEventArgs e)
        {
            CallWindow callWindow = new CallWindow();
            callWindow.Show();
        }

        private void btn_FriendReq_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}