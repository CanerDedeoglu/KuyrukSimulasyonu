using KuyrukSimulation.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KuyrukSimulation
{
    public partial class QueueListWindow : Window
    {
        // QueueListWindow penceresinin başlatılması
        public QueueListWindow()
        {
            InitializeComponent();  // WPF bileşenlerinin yüklenmesi
            _ = LoadQueueEntriesAsync();  // Kuyruk girişlerini asenkron olarak yükleme işlemi başlatılır
        }

        // Veritabanından kuyruk girişlerini yükleyen ve liste görünümüne ekleyen metot
        private async Task LoadQueueEntriesAsync()
        {
            try
            {
                // DataContext sınıfı, veritabanı işlemleri için kullanılır
                using (var dataContext = new DataContext())
                {
                    // Veritabanındaki kuyruk girişlerini sorgula, gerekli alanları seç ve listeye dönüştür
                    var queueEntries = await dataContext.BusStopQueues
                                                        .Select(queue => new
                                                        {
                                                            BusStopName = queue.BusStopName,  // Otobüs durağının adı
                                                            QueueLength = queue.QueueLength,  // Kuyruk uzunluğu
                                                            Time = queue.Time.ToString("dd.MM.yyyy HH:mm")  // Zamanı belirtilen formatta string olarak al
                                                        })
                                                        .ToListAsync();

                    // Kuyruk girişlerini liste görünümüne (QueueListView) ekle
                    QueueListView.ItemsSource = queueEntries;
                }
            }
            catch (Exception exception)
            {
                // Veritabanı işlemi sırasında bir hata oluşursa kullanıcıya bir mesaj göster
                MessageBox.Show("Kuyruk girişleri yüklenirken bir hata oluştu: " + exception.Message);
            }
        }
    }
}
