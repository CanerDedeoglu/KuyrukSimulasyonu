using KuyrukSimulasyonu.Context;
using KuyrukSimulasyonu.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KuyrukSimulasyonu.Views
{
    public partial class QueueInputWindow : Window
    {
        public QueueInputWindow()
        {
            InitializeComponent();
        }

        private void AddQueueLength_Click(object sender, RoutedEventArgs e)
        {
            // Kuyruk uzunluğunu ekleyin veya güncelleyin
            string lengthText = queueLengthTextBox.Text;
            string location = locationTextBox.Text;

            if (int.TryParse(lengthText, out int queueLength))
            {
                using (var context = new DataContext())
                {
                    var existingQueue = context.Kuyruklar
                        .FirstOrDefault(q => q.BeklemeNoktasi == location);

                    if (existingQueue != null)
                    {
                        // Var olan kuyruk uzunluğunu güncelle
                        existingQueue.KuyrukSuresi = queueLength;
                        existingQueue.FotoTarihi = DateTime.Now;
                        context.SaveChanges();
                        MessageBox.Show("Bekleme noktası güncellendi.");
                    }
                    else
                    {
                        // Yeni kuyruk ekle
                        Kuyruk newQueue = new Kuyruk
                        {
                            BeklemeNoktasi = location,
                            KuyrukSuresi = queueLength,
                            FotoTarihi = DateTime.Now
                        };
                        context.Kuyruklar.Add(newQueue);
                        context.SaveChanges();
                        MessageBox.Show("Kuyruk başarıyla eklendi.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Kuyruk uzunluğunu geçerli bir sayı olarak girin.");
            }
        }

        private void StartSimulation_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DataContext())
            {
                // Kuyruk verisi olup olmadığını kontrol edin
                var hasQueues = context.Kuyruklar.Any();

                if (hasQueues)
                {
                    // Kuyruk verisi varsa simülasyon sayfasına yönlendirin
                    SimulationWindow simulationWindow = new SimulationWindow();
                    simulationWindow.Show();
                    this.Close();
                }
                else
                {
                    // Kuyruk verisi yoksa uyarı mesajı gösterin
                    MessageBox.Show("Simülasyonu başlatmak için en az bir kuyruk ekleyin.");
                }
            }
        }

        private void RemoveText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Kuyruk Uzunluğu" || textBox.Text == "Bekleme Noktası"))
            {
                textBox.Text = "";
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void AddText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "queueLengthTextBox")
                {
                    textBox.Text = "Kuyruk Uzunluğu";
                }
                else if (textBox.Name == "locationTextBox")
                {
                    textBox.Text = "Bekleme Noktası";
                }
                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
    }
}
