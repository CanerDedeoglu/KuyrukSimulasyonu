using KuyrukSimulation.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuyrukSimulation.Context
{
    public class DataContext : DbContext
    {
        // BusStopQueues tablosunu DbSet olarak tanımlıyoruz
        public DbSet<BusStopQueues> BusStopQueues { get; set; }

        // Veritabanı bağlantısını yapılandırıyoruz
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // SQLite veritabanı ile bağlantı kuruyoruz
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\CANER\\source\\repos\\KuyrukSimulation\\KuyrukSimulation\\Simulasyon.db");
        }

        // Model oluşturma işlemleri burada yapılır
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeder verilerini ekliyoruz
            modelBuilder.Entity<BusStopQueues>().HasData(GenerateSeedData());
        }

        // Seed verilerini oluşturan yöntem
        private BusStopQueues[] GenerateSeedData()
        {
            // Bus stop isimleri
            var busStops = new[] { "B1", "B2", "B3", "B4", "B5" };
            // Seed veri dizisini tanımlıyoruz (5 durak x 100 kayıt)
            var seedData = new BusStopQueues[500];

            var random = new Random(); // Rastgele değerler için Random nesnesi
            int index = 0; // Dizideki mevcut indeks

            foreach (var stop in busStops)
            {
                for (int i = 0; i < 100; i++)
                {
                    // Seed verilerini oluşturuyoruz
                    seedData[index++] = new BusStopQueues
                    {
                        Id = index, // Her kayıt için benzersiz Id
                        BusStopName = stop, // Durak adı
                        QueueLength = random.Next(10, 100), // Rastgele kuyruk uzunluğu
                        Time = DateTime.Now.AddMinutes(-i) // Şu anki zamanın geçmişe doğru offset
                    };
                }
            }

            return seedData; // Oluşturulan seed verilerini döndürüyoruz
        }
    }
}
