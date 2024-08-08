using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuyrukSimulasyonu.Entities
{
    public class Kuyruk
    {
        [Key]
        public int Id { get; set; }

        public string? BeklemeNoktasi { get; set; }

        public DateTime FotoTarihi { get; set; }

        public int? KuyrukSuresi { get; set; }

        public Kuyruk()
        {
            FotoTarihi = DateTime.Now;
        }
    }
}
