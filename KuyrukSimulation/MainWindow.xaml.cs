using KuyrukSimulation.Context;
using KuyrukSimulation.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KuyrukSimulation
{
    public partial class MainWindow : Window
    {
        // Ana pencerenin (MainWindow) başlatılması
        public MainWindow()
        {
            InitializeComponent();  // WPF bileşenlerinin yüklenmesi
            _ = LoadBusStopsAsync();  // Otobüs duraklarını asenkron olarak yükleme işlemi başlatılır
        }

        // Otobüs duraklarını veritabanından yükleyip ComboBox'a ekleyen metot
        private async Task LoadBusStopsAsync()
        {
            try
            {
                // DataContext sınıfı, veritabanı işlemleri için kullanılır
                using (var dataContext = new DataContext())
                {
                    // Veritabanındaki otobüs duraklarının isimlerini sorgula, tekrarlayan isimleri kaldır ve listeye dönüştür
                    var busStopNames = await dataContext.BusStopQueues
                                                        .Select(busStop => busStop.BusStopName)
                                                        .Distinct()
                                                        .ToListAsync();

                    // Otobüs duraklarının isimlerini ComboBox'a (BusStopComboBox) ekle
                    BusStopComboBox.ItemsSource = busStopNames;
                }
            }
            catch (Exception exception)
            {
                // Veritabanı işlemi sırasında bir hata oluşursa kullanıcıya bir mesaj göster
                MessageBox.Show("Otobüs durakları yüklenirken bir hata oluştu: " + exception.Message);
            }
        }

        // "Otobüs Durağı Ekle" butonuna tıklandığında çalışacak metot
        private async void AddBusStopButton_Click(object sender, RoutedEventArgs e)
        {
            // Kullanıcının girdiği otobüs durağı adını al
            string busStopName = BusStopNameTextBox.Text;

            // Eğer kullanıcı geçerli bir isim girmediyse bir uyarı mesajı göster
            if (string.IsNullOrWhiteSpace(busStopName))
            {
                MessageBox.Show("Lütfen otobüs durağı adı giriniz.");
                return;  
            }

            try
            {
                using (var dataContext = new DataContext())
                {
                    // Yeni bir otobüs durağı oluştur ve veritabanına ekle
                    var newBusStop = new BusStopQueues { BusStopName = busStopName };
                    dataContext.BusStopQueues.Add(newBusStop);
                    await dataContext.SaveChangesAsync();  // Değişiklikleri veritabanına kaydet

                    // Kullanıcıya başarılı bir ekleme mesajı göster
                    MessageBox.Show("Otobüs durağı başarıyla eklendi.");
                    BusStopNameTextBox.Clear();  // TextBox içeriğini temizle
                    await LoadBusStopsAsync();  // ComboBox'u güncellemek için durakları yeniden yükle
                }
            }
            catch (Exception exception)
            {
                // Ekleme işlemi sırasında bir hata oluşursa kullanıcıya bir mesaj göster
                MessageBox.Show("Otobüs durağı eklenirken bir hata oluştu: " + exception.Message);
            }
        }

        // "Kuyruk Girişi Ekle" butonuna tıklandığında çalışacak metot
        private async void AddEntryButton_Click(object sender, RoutedEventArgs e)
        {
            // Kullanıcıdan otobüs durağı seçmesini istemek için kontrol yap
            if (BusStopComboBox.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir otobüs durağı seçiniz.");
                return;  
            }

            // Kullanıcıdan geçerli bir kuyruk uzunluğu (sayı) girmesini kontrol et
            if (!int.TryParse(QueueLengthTextBox.Text, out int queueLength))
            {
                MessageBox.Show("Lütfen geçerli bir kuyruk uzunluğu giriniz.");
                return;  
            }

            try
            {
                using (var dataContext = new DataContext())
                {
                    // Kullanıcının seçtiği otobüs durağının adını al
                    var selectedBusStopName = BusStopComboBox.SelectedItem.ToString();

                    // Veritabanından seçilen otobüs durağını bul
                    var existingBusStop = await dataContext.BusStopQueues
                        .Where(busStop => busStop.BusStopName == selectedBusStopName)
                        .FirstOrDefaultAsync();

                    // Eğer seçilen durak bulunamazsa kullanıcıya bir uyarı göster
                    if (existingBusStop == null)
                    {
                        MessageBox.Show("Seçilen otobüs durağı veritabanında bulunamadı.");
                        return;  
                    }

                    // Yeni bir kuyruk girişi oluştur ve veritabanına ekle
                    var queueEntry = new BusStopQueues
                    {
                        // Id otomatik olarak ayarlanır, bu yüzden manuel olarak belirtilmez
                        BusStopName = existingBusStop.BusStopName,  // Mevcut durağın adıyla yeni giriş oluştur
                        QueueLength = queueLength,  // Kullanıcının girdiği kuyruk uzunluğunu ekle
                        Time = DateTime.Now  // Şu anki zamanı kuyruk girişine kaydet
                    };

                    dataContext.BusStopQueues.Add(queueEntry);  // Kuyruk girişini veritabanına ekle
                    await dataContext.SaveChangesAsync();  // Değişiklikleri veritabanına kaydet

                    // Kullanıcıya başarılı bir ekleme mesajı göster
                    MessageBox.Show("Kuyruk girişi başarıyla eklendi.");
                    QueueLengthTextBox.Clear();  // Kuyruk uzunluğu TextBox'unu temizle
                }
            }
            catch (Exception exception)
            {
                // Ekleme işlemi sırasında bir hata oluşursa, hatayı detaylı bir şekilde göster
                string errorMessage = "Kuyruk girişi eklenirken bir hata oluştu: " + exception.Message;
                if (exception.InnerException != null)
                {
                    errorMessage += "\nİçsel Hata: " + exception.InnerException.Message;
                    if (exception.InnerException.InnerException != null)
                    {
                        errorMessage += "\nİçsel İçsel Hata: " + exception.InnerException.InnerException.Message;
                    }
                }
                MessageBox.Show(errorMessage);  // Hata mesajını kullanıcıya göster
            }
        }

        // "Simülasyonu Başlat" butonuna tıklandığında çalışacak metot
        private void StartSimulationButton_Click(object sender, RoutedEventArgs e)
        {
            var simulationWindow = new SimulationWindow();  // Simülasyon penceresini oluştur
            simulationWindow.Show();  // Simülasyon penceresini göster
            this.Close();  // Ana pencereyi kapat
        }

        // "Kuyrukları Listele" butonuna tıklandığında çalışacak metot
        private void ShowQueueListButton_Click(object sender, RoutedEventArgs e)
        {
            var queueListWindow = new QueueListWindow();  // Kuyruk listesi penceresini oluştur
            queueListWindow.Show();  // Kuyruk listesi penceresini göster
        }
    }
}
