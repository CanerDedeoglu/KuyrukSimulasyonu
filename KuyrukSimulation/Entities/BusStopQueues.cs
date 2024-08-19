using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuyrukSimulation.Entities
{

    // Bekleme Noktası sınıfı
    public class BusStopQueues
    {
        public int Id { get; set; }
        public string BusStopName { get; set; }
        public int QueueLength { get; set; }
        public DateTime Time { get; set; }
    }
}
