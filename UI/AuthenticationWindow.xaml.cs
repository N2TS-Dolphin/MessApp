using MessApp.UC;
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
using System.Windows.Shapes;

namespace MessApp.UI
{
    /// <summary>
    /// Interaction logic for AuthenticationWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow()
        {
            InitializeComponent();

            logInForm.OnLogInSuccess += HandleLogInSuccess;
            logInForm.OnForgotPasswordSwitch += HandleForgotPasswordSwitch;
            logInForm.OnSignUpSwitch += HandleSignUpSwitch;

            signUpForm.OnRegisterSuccess += HandleLogInSwitch;
            signUpForm.OnLogInSwitch += HandleLogInSwitch;
            
        }

        public void HandleLogInSwitch()
        {
            signUpForm.Visibility = Visibility.Collapsed;
            logInForm.Visibility = Visibility.Visible;
        }

        public void HandleLogInSuccess()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        public void HandleSignUpSwitch()
        {
            signUpForm.Visibility = Visibility.Visible;
            logInForm.Visibility = Visibility.Collapsed;
        }

        public void HandleForgotPasswordSuccess()
        {

        }

        public void HandleForgotPasswordSwitch()
        {

        }
    }
}
