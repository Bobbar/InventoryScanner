using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using InventoryScanner.Data.Classes;

namespace InventoryScanner
{
    public interface IScanning
    {
        void SetController(ScanningController controller);

        Location ScanLocation { get; set; }

        void LoadScanItems(DataTable data);

        void LockScanInfoUI();

        void SetScanInfo(Scan scan);

        List<string> LocationFilters { get; }

    }
}
