using KuyrukSimulasyonu.Context;
using KuyrukSimulasyonu.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KuyrukSimulasyonu.Views
{
    public partial class SimulationWindow : Window
    {
        private const double BaseDiameter = 50;
        private const double CircleSpacing = 10;
        private const double MinDiameter = 30;
        private const double MaxDiameter = 100;
        private double _simulationSpeed = 1.0;
        private Random _random = new Random();
        private DispatcherTimer _timer;
        private int _currentIndex = 0;
        private bool _dataCompleted = false;

        public SimulationWindow()
        {
            InitializeComponent();
            StartSimulation();
        }

        private void StartSimulation()
        {
            InitializeTimer();
            DrawQueueCircles();
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1 / _simulationSpeed);
            _timer.Tick += (sender, args) => UpdateSimulation();
            _timer.Start();
        }

        private void DrawQueueCircles()
        {
            simulationCanvas.Children.Clear();
            _currentIndex = 0;
            _dataCompleted = false;

            using (var context = new DataContext())
            {
                var queues = context.Kuyruklar.OrderBy(q => q.Id).ToList();

                if (queues.Count == 0)
                {
                    MessageBox.Show("Kuyruk verisi bulunamadı.");
                    _dataCompleted = true;
                    _timer.Stop();
                    return;
                }
            }
        }

        private void UpdateSimulation()
        {
            if (_dataCompleted) return;

            using (var context = new DataContext())
            {
                var queues = context.Kuyruklar.OrderBy(q => q.Id).ToList();

                if (queues.Count == 0)
                {
                    MessageBox.Show("Kuyruk verisi bulunamadı.");
                    _dataCompleted = true;
                    _timer.Stop();
                    return;
                }

                if (_currentIndex >= queues.Count)
                {
                    _dataCompleted = true;
                    _timer.Stop();
                    MessageBox.Show("Simülasyon tamamlandı. Veriler bitti.");
                    return;
                }

                var queue = queues[_currentIndex];
                double normalizedQueueLength = NormalizeQueueLength(queue.KuyrukSuresi ?? 0, queues);
                double diameter = MinDiameter + normalizedQueueLength;
                diameter = Math.Min(diameter, MaxDiameter);

                Ellipse ellipse = new Ellipse
                {
                    Width = diameter,
                    Height = diameter,
                    Fill = GetRandomColor(),
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };

                TextBlock textBlock = new TextBlock
                {
                    Text = queue.BeklemeNoktasi ?? "Bilinmiyor",
                    Foreground = Brushes.Black,
                    FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                PlaceCircle(ellipse, textBlock);

                simulationCanvas.Children.Add(ellipse);
                simulationCanvas.Children.Add(textBlock);

                _currentIndex++;
            }
        }

        private double NormalizeQueueLength(int queueLength, System.Collections.Generic.List<Kuyruk> queues)
        {
            var maxQueueLength = queues.Max(q => q.KuyrukSuresi ?? 0);
            var minQueueLength = queues.Min(q => q.KuyrukSuresi ?? 0);
            return ((double)(queueLength - minQueueLength) / (maxQueueLength - minQueueLength)) * (MaxDiameter - MinDiameter);
        }

        private Brush GetRandomColor()
        {
            byte[] rgb = new byte[3];
            _random.NextBytes(rgb);
            return new SolidColorBrush(Color.FromRgb(rgb[0], rgb[1], rgb[2]));
        }

        private void PlaceCircle(Ellipse ellipse, TextBlock textBlock)
        {
            double canvasWidth = simulationCanvas.ActualWidth;
            double canvasHeight = simulationCanvas.ActualHeight;
            double diameter = ellipse.Width;

            double x, y;
            bool placed = false;

            while (!placed)
            {
                x = _random.NextDouble() * (canvasWidth - diameter);
                y = _random.NextDouble() * (canvasHeight - diameter);

                Rect newRect = new Rect(x, y, diameter, diameter);
                bool overlap = simulationCanvas.Children.OfType<Ellipse>()
                                    .Any(existing => newRect.IntersectsWith(new Rect(Canvas.GetLeft(existing), Canvas.GetTop(existing), existing.Width, existing.Height)));

                if (!overlap)
                {
                    placed = true;
                    Canvas.SetLeft(ellipse, x);
                    Canvas.SetTop(ellipse, y);

                    // TextBlock'ı da aynı konuma yerleştir
                    Canvas.SetLeft(textBlock, x + (diameter - textBlock.ActualWidth) / 2);
                    Canvas.SetTop(textBlock, y + (diameter - textBlock.ActualHeight) / 2);
                }
            }
        }

        private void SpeedUpButton_Click(object sender, RoutedEventArgs e)
        {
            _simulationSpeed += 0.5;
            _timer.Interval = TimeSpan.FromSeconds(1 / _simulationSpeed);
        }

        private void SlowDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (_simulationSpeed > 0.5)
            {
                _simulationSpeed -= 0.5;
                _timer.Interval = TimeSpan.FromSeconds(1 / _simulationSpeed);
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            StartSimulation();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Önceki sayfaya dön
            QueueInputWindow queueInputWindow = new QueueInputWindow();
            queueInputWindow.Show();
            this.Close();
        }
    }
}
