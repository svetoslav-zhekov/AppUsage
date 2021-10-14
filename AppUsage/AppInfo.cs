using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUsage
{
    /// AppInfo Class Which Converts Into Json Object And Is Added In Json File
    class AppInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AppUsageTime Time { get; set; }
        public bool IsRunning { get; set; }

        public AppInfo()
        {
        }

    }
}
