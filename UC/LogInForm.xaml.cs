using MessApp.Controller;
using MessApp.DB;
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

using MessApp.Config;

namespace MessApp.UC
{
    /// <summary>
    /// Interaction logic for LogInForm.xaml
    /// </summary>
    public partial class LogInForm : UserControl
    {
        public event Action OnLogInSuccess;
        public event Action OnSignUpSwitch;
        public event Action OnForgotPasswordSwitch;
        private readonly MongoDBClient _client;
        private AuthenticateController _authenticatorController;

        public LogInForm()
        {
            InitializeComponent();
            _client = new MongoDBClient(new DBConfig());
            _authenticatorController = new AuthenticateController(_client);
        }

        private void username_TextChanged(object sender, TextChangedEventArgs e)
        {
            usernameHint.Visibility = string.IsNullOrEmpty(username.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordHint.Visibility = string.IsNullOrEmpty(password.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            LogInAction();
        }

        private void btn_SignUpSwitch_Click(object sender, RoutedEventArgs e)
        {
            OnSignUpSwitch?.Invoke();
        }

        private void btn_ForgotPasswordSwitch_Click(object sender, RoutedEventArgs e)
        {
            OnForgotPasswordSwitch?.Invoke();
        }

        public void LogInAction()
        {
            if (_authenticatorController.LogIn(username.Text, password.Password))
                OnLogInSuccess?.Invoke();
            else
                MessageBox.Show("Incorrect Username or Password");
        }
    }
}
