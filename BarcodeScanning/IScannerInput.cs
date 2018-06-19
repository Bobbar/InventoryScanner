using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryScanner
{
    public interface IScannerInput
    {
        /// <summary>
        /// Connect/open scanner.
        /// </summary>
        /// <returns>Returns true if successful./</returns>
        bool StartScanner();

        bool SupportsFeedback { get; }

        event EventHandler<string> NewScanReceived;

        event EventHandler<Exception> ExceptionOccured;

        void GoodScan();

        void BadScan();

        void Dispose();
    }
}
