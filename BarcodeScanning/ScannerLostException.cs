using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryScanner.BarcodeScanning
{
    public class ScannerLostException : Exception
    {
        public ScannerLostException() : base() { }
    }
}
