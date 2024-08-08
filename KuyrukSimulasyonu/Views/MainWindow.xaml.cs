using KuyrukSimulasyonu.Context;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KuyrukSimulasyonu.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdatePasswordPlaceholder();
        }

        private void RemoveText(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text == "Kullanıcı Adı")
            {
                usernameTextBox.Text = "";
                usernameTextBox.Foreground = Brushes.Black;
            }
        }

        private void AddText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(usernameTextBox.Text))
            {
                usernameTextBox.Text = "Kullanıcı Adı";
                usernameTextBox.Foreground = Brushes.Gray;
            }
        }

        private void UpdatePasswordPlaceholder()
        {
            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                passwordPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                passwordPlaceholder.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdatePasswordPlaceholder();
        }

        private void RemovePasswordText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                passwordPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void AddPasswordText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                passwordPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text == "Kullanıcı Adı" ? string.Empty : usernameTextBox.Text;
            string password = passwordBox.Password;

            if (IsLoginValid(username, password))
            {
                var existingWindow = Application.Current.Windows.OfType<QueueInputWindow>().FirstOrDefault();
                if (existingWindow == null)
                {
                    var simulationWindow = new QueueInputWindow();
                    simulationWindow.Show();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre yanlış.");
            }
        }

        private bool IsLoginValid(string username, string password)
        {
            using (var context = new DataContext())
            {
                var hashedPassword = Hashing.HashPassword(password);
                var user = context.Users.SingleOrDefault(u => u.Username == username && u.Password == hashedPassword);
                return user != null;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow();
            forgotPasswordWindow.Show();
        }
    }
}
