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
        private AuthenticateController _authenticatorController;

        public LogInForm()
        {
            InitializeComponent();

            _authenticatorController = new AuthenticateController(new MongoDBClient(new DBConfig()));
        }

        private void username_TextChanged(object sender, TextChangedEventArgs e)
        {
            usernameHint.Visibility = string.IsNullOrEmpty(username.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordHint.Visibility = string.IsNullOrEmpty(password.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            btn_Login.IsEnabled = btn_ForgotPassword.IsEnabled = btn_Create.IsEnabled = false;

            await LogInAction();
        }

        private void btn_SignUpSwitch_Click(object sender, RoutedEventArgs e)
        {
            OnSignUpSwitch?.Invoke();
        }

        private void btn_ForgotPasswordSwitch_Click(object sender, RoutedEventArgs e)
        {
            OnForgotPasswordSwitch?.Invoke();
        }

        public async Task LogInAction()
        {
            try
            {
                if (string.IsNullOrEmpty(username.Text) || string.IsNullOrEmpty(password.Password))
                {
                    MessageBox.Show("Please enter both username and password.");
                    btn_Login.IsEnabled = btn_ForgotPassword.IsEnabled = btn_Create.IsEnabled = true;

                    return;
                }

                bool isSuccess = await _authenticatorController.LogIn(username.Text, password.Password);

                if (isSuccess)
                    OnLogInSuccess?.Invoke();
                else
                {
                    MessageBox.Show("Incorrect Username or Password");
                    btn_Login.IsEnabled = btn_ForgotPassword.IsEnabled = btn_Create.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured: {ex.Message}");
                btn_Login.IsEnabled = btn_ForgotPassword.IsEnabled = btn_Create.IsEnabled = true;
            }
        }
    }
}
