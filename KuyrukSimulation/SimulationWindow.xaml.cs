using KuyrukSimulation.Context;
using KuyrukSimulation.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KuyrukSimulation
{
    public partial class SimulationWindow : Window
    {
        private double _intervalDuration = 1000; // Başlangıç simülasyon hızı (1x)
        private DispatcherTimer _updateTimer; // UI güncellemeleri için zamanlayıcı
        private static readonly Brush[] _colors =  // Kuyruk daireleri için renk paleti
        {
            Brushes.Red, Brushes.Green, Brushes.Blue, Brushes.Orange, Brushes.Purple,
            Brushes.Pink, Brushes.Yellow, Brushes.Cyan, Brushes.Magenta
        };
        private readonly DataContext _dataContext; // Veritabanı bağlamı
        private double _maxSize; // En büyük kuyruk uzunluğuna sahip dairenin boyutu
        private Dictionary<string, (Ellipse Circle, TextBlock TextBlock, List<BusStopQueues> Data, int Index)> _uiElements;

        public SimulationWindow()
        {
            InitializeComponent();
            _dataContext = new DataContext();
            Loaded += OnWindowLoaded;
        }

        // Pencere yüklendiğinde çalıştırılan olay
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Pencere yüklendiğinde yapılacak işlemler burada yer alır.
        }

        // Simülasyonu başlatan buton tıklama olayı
        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => RunSimulationAsync());
        }

        // Simülasyonu çalıştıran asenkron metot
        private async Task RunSimulationAsync()
        {
            // Veritabanından en büyük kuyruk uzunluğunu alır
            _maxSize = await GetMaxQueueLengthAsync();

            await Dispatcher.InvokeAsync(() =>
            {
                LoadingProgressBar.Visibility = Visibility.Visible; // Yükleme çubuğunu göster
                SimulationPanel.Children.Clear(); // Simülasyon panelini temizle
            });

            _uiElements = new Dictionary<string, (Ellipse, TextBlock, List<BusStopQueues>, int)>();
            DateTime fiveMinutesAgo = DateTime.Now.AddMinutes(-5);

            Random random = new Random();
            bool timerStarted = false;

            // Veritabanından otobüs durakları ve kuyruk verilerini gruplar halinde alır
            await foreach (var group in (from item in _dataContext.BusStopQueues.AsNoTracking()
                                         where item.Time >= fiveMinutesAgo
                                         group item by item.BusStopName into g
                                         select new
                                         {
                                             g.Key,
                                             Value = g.ToList(),
                                         }).AsAsyncEnumerable())
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    // Her grup için bir grid ve daire oluştur
                    var grid = new Grid
                    {
                        Width = 120,
                        Height = 120,
                        Margin = new Thickness(20)
                    };

                    var circle = new Ellipse
                    {
                        Width = 120,
                        Height = 120,
                        Fill = _colors[random.Next(_colors.Length)] // Dairenin rengini rastgele seç
                    };

                    var textBlock = new TextBlock
                    {
                        Text = $"{group.Key}\n0", // Otobüs durağı adı ve başlangıç kuyruk uzunluğu
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Foreground = Brushes.White,
                        TextAlignment = TextAlignment.Center,
                        FontWeight = FontWeights.Bold
                    };

                    // Grid'e daireyi ve metin bloğunu ekle
                    grid.Children.Add(circle);
                    grid.Children.Add(textBlock);

                    LoadingProgressBar.Visibility = Visibility.Collapsed; // Yükleme çubuğunu gizle
                    SimulationPanel.Children.Add(grid); // Simülasyon paneline grid'i ekle

                    // UI öğelerini sözlüğe ekle
                    _uiElements[group.Key] = (circle, textBlock, group.Value, 0);

                    if (!timerStarted)
                    {
                        StartTimer(); // Zamanlayıcıyı başlat
                        timerStarted = true;
                    }
                });
            }
        }

        // Simülasyon zamanlayıcısını başlatan metot
        private void StartTimer()
        {
            if (_updateTimer == null)
            {
                _updateTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(_intervalDuration)
                };

                _updateTimer.Tick += UpdateUI; // Zamanlayıcı her tetiklendiğinde UI'yi günceller
                _updateTimer.Start();
            }
        }

        // Simülasyon penceresindeki UI'yi güncelleyen metot
        private void UpdateUI(object sender, EventArgs e)
        {
            Stopwatch uiStopwatch = Stopwatch.StartNew(); // UI güncellemeleri için süre ölçer

            foreach (var key in _uiElements.Keys.ToList())
            {
                var (circle, textBlock, data, index) = _uiElements[key];
                var busStopQueueItem = data[index];
                double size = (busStopQueueItem.QueueLength / _maxSize) * 120; // Dairenin boyutunu hesapla

                circle.Width = size;
                circle.Height = size;
                textBlock.Text = $"{busStopQueueItem.BusStopName}\n{busStopQueueItem.QueueLength}";
                textBlock.FontSize = Math.Max(5, size / 5); // Yazı boyutunu dairenin boyutuna göre ayarla

                // Sonraki veri öğesine geç
                _uiElements[key] = (circle, textBlock, data, (index + 1) % data.Count);
            }

            uiStopwatch.Stop();
            Console.WriteLine($"UI güncelleme süresi: {uiStopwatch.ElapsedMilliseconds} ms"); // Güncelleme süresini konsola yazdır
        }

        // Veritabanından en büyük kuyruk uzunluğunu asenkron olarak getiren metot
        private async Task<int> GetMaxQueueLengthAsync()
        {
            DateTime fiveMinutesAgo = DateTime.Now.AddMinutes(-5);
            return await _dataContext.BusStopQueues
                                     .AsNoTracking()
                                     .Where(p => p.Time >= fiveMinutesAgo)
                                     .Select(p => p.QueueLength)
                                     .MaxAsync();
        }

        // Hız ayarı yapan butonun tıklama olayı
        private void SpeedButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (double.TryParse(button.Tag.ToString(), out double speedFactor))
                {
                    if (speedFactor == 0)
                    {
                        _updateTimer.Stop(); // Zamanlayıcıyı durdur
                    }
                    else
                    {
                        _updateTimer.Stop(); // Zamanlayıcıyı durdur
                        double newIntervalDuration = _intervalDuration / speedFactor; // Yeni hız faktörünü hesapla
                        _updateTimer.Interval = TimeSpan.FromMilliseconds(newIntervalDuration); // Zamanlayıcı aralığını güncelle
                        _updateTimer.Start(); // Zamanlayıcıyı tekrar başlat
                    }
                }
            }
        }
    }
}
