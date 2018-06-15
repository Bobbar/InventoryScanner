using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryScanner
{
    public interface IScannerInput
    {
        void StartScanner();

        event EventHandler<string> NewScanReceived;

        void Dispose();
    }
}
