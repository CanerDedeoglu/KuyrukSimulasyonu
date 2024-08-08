using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using KuyrukSimulasyonu.Context;
using KuyrukSimulasyonu.Entities;

namespace KuyrukSimulasyonu.Views
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
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

        private void RemoveEmailText(object sender, RoutedEventArgs e)
        {
            if (emailTextBox.Text == "E-posta")
            {
                emailTextBox.Text = "";
                emailTextBox.Foreground = Brushes.Black;
            }
        }

        private void AddEmailText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(emailTextBox.Text))
            {
                emailTextBox.Text = "E-posta";
                emailTextBox.Foreground = Brushes.Gray;
            }
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
            else
            {
                passwordPlaceholder.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdatePasswordPlaceholder();
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

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string email = emailTextBox.Text;
            string password = passwordBox.Password;

            // Kullanıcı adı, e-posta ve şifrenin boş olup olmadığını kontrol et
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Kullanıcı adı, e-posta ve şifre boş olamaz.");
                return;
            }

            // E-posta adresinin geçerliliğini kontrol et
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Geçerli bir e-posta adresi girin.");
                return;
            }

            using (var context = new DataContext())
            {
                var existingUser = context.Users.SingleOrDefault(u => u.Username == username);
                if (existingUser != null)
                {
                    MessageBox.Show("Bu kullanıcı adı zaten mevcut.");
                    return;
                }

                User newUser = new User
                {
                    Username = username,
                    Email = email,
                    Password = Hashing.HashPassword(password)
                };

                context.Users.Add(newUser);
                context.SaveChanges();
            }

            MessageBox.Show("Kayıt başarılı. Giriş ekranına yönlendiriliyorsunuz.");
            this.Close(); // Kayıt işlemi tamamlandığında pencereyi kapat
        }

        private bool IsValidEmail(string email)
        {
            // Basit bir regex ile e-posta geçerliliğini kontrol et
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}
