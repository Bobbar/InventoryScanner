using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InventoryScanner
{
    public static class Paths
    {
        public static readonly string AppDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\InventoryScanner\";//AppDomain.CurrentDomain.BaseDirectory;
        public const string LogName = "log.log";
        public static readonly string LogPath = AppDir + LogName;

        public static readonly string SQLiteScanPath = Directory.GetCurrentDirectory() + @"\PreviousScans\";
    }
}
