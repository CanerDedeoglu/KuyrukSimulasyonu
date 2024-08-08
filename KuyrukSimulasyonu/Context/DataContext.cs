using KuyrukSimulasyonu.Entities;
using KuyrukSimulasyonu.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KuyrukSimulasyonu.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Kuyruk> Kuyruklar { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: File path should be dynamic
            //var appDir = FileHelper.GetApplicationDirectory();
            //var databasePath = Path.Combine(appDir, "KuyrukSimulasyonuData.db");
            var filePath = @"C:\Users\CANER\source\repos\KuyrukSimulasyonu\KuyrukSimulasyonu\KuyrukSimulasyonuData.db";
            optionsBuilder.UseSqlite($"Data Source={filePath}");
        }
    }
}
