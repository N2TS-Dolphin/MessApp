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

using MessApp.Controller;
using MessApp.UC;
using MessApp.DB;
using MessApp.Config;

namespace MessApp.UI.Main
{
    /// <summary>
    /// Interaction logic for AuthenticationWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        private readonly MongoDBClient _client;
        private readonly AuthenticateController _authenticateController;

        public AuthenticationWindow()
        {
            InitializeComponent();

            _client = new MongoDBClient(new DBConfig());

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
            int user_id = LoginState.Instance.Get();

            MainWindow mainWindow = new MainWindow(user_id);
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(logInForm.Visibility == Visibility.Visible )
                { 
                    logInForm.LogInAction(); 
                }
                else if(signUpForm.Visibility == Visibility.Visible )
                {
                    signUpForm.SignUpAction();
                }
            }
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
    }
}
