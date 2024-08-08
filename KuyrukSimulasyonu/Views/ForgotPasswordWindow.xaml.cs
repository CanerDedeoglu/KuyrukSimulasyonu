using System.Linq;
using System.Windows;
using System.Windows.Media;
using KuyrukSimulasyonu.Context;
using KuyrukSimulasyonu.Entities;

namespace KuyrukSimulasyonu.Views
{
    public partial class ForgotPasswordWindow : Window
    {
        public ForgotPasswordWindow()
        {
            InitializeComponent();
        }

        private void RemoveText(object sender, RoutedEventArgs e)
        {
            if (emailTextBox.Text == "E-posta Adresi")
            {
                emailTextBox.Text = "";
                emailTextBox.Foreground = Brushes.Black;
            }
        }

        private void AddText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(emailTextBox.Text))
            {
                emailTextBox.Text = "E-posta Adresi";
                emailTextBox.Foreground = Brushes.Gray;
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;

            if (string.IsNullOrWhiteSpace(email) || email == "E-posta Adresi")
            {
                MessageBox.Show("E-posta adresi boş olamaz.");
                return;
            }

            try
            {
                using (var context = new DataContext())
                {
                    var user = context.Users.SingleOrDefault(u => u.Email == email);
                    if (user == null)
                    {
                        MessageBox.Show("Bu e-posta adresi ile kayıtlı kullanıcı bulunamadı.");
                        return;
                    }

                    // Burada şifre sıfırlama e-postası gönderme işlemi yapılacak
                    MessageBox.Show("Şifre sıfırlama bağlantısı e-posta adresinize gönderilmiştir.");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya mesaj göster
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }

    }
}
