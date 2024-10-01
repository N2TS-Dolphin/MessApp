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
using MessApp.Controller;
using MessApp.DB;
using MessApp.DB.Model;

namespace MessApp.UC
{
    /// <summary>
    /// Interaction logic for SignUpForm.xaml
    /// </summary>
    public partial class SignUpForm : UserControl
    {
        public event Action OnRegisterSuccess;
        public event Action OnLogInSwitch;
        private readonly MongoDBClient _client;
        private AuthenticateController _authenticatorController;

        public SignUpForm()
        {
            InitializeComponent();
            _client = new MongoDBClient(new DBConfig());
            _authenticatorController = new AuthenticateController(_client);
        }

        private void Handle_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Name == "username")
                usernameHint.Visibility = string.IsNullOrEmpty(username.Text) ? Visibility.Visible : Visibility.Collapsed;
            else if (textBox.Name == "phone")
                phoneHint.Visibility = string.IsNullOrEmpty(phone.Text) ? Visibility.Visible : Visibility.Collapsed;
            else if (textBox.Name == "firstName")
                firstNameHint.Visibility = string.IsNullOrEmpty(firstName.Text) ? Visibility.Visible : Visibility.Collapsed;
            else if (textBox.Name == "lastName")
                lastNameHint.Visibility = string.IsNullOrEmpty(lastName.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Handle_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordHint.Visibility = string.IsNullOrEmpty(password.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btn_Register_Click(object sender, RoutedEventArgs e)
        {
            SignUpAction();
            OnRegisterSuccess?.Invoke();
        }

        private void btn_LogInSwitch_Click(object sender, RoutedEventArgs e)
        {
            OnLogInSwitch?.Invoke();
        }

        public void SignUpAction()
        {
            _authenticatorController.SignUp(username.Text, password.Password, phone.Text, firstName.Text, lastName.Text, birthDate.SelectedDate ?? DateTime.Today);
        }
    }
}
