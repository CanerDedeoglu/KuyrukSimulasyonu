using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuyrukSimulasyonu.Helpers
{
    public static class FileHelper
    {
        public static string GetApplicationDirectory() => Process.GetCurrentProcess().MainModule!.FileName;
    }
}
