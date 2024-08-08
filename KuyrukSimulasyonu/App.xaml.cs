using KuyrukSimulasyonu.Views;
using System.Linq;
using System.Windows;

namespace KuyrukSimulasyonu
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // MainWindow'un sadece bir kez başlatıldığını kontrol et
            var existingWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (existingWindow == null)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }
    }
}
