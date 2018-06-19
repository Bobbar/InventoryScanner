using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using InventoryScanner.Data.Classes;
using InventoryScanner.ScanController;

namespace InventoryScanner
{
    public interface IScanningUI
    {
        void SetController(ScanningController controller);

        void LoadScanItems(DataTable data);

        List<string> LocationFilters { get; }

        void PopulateNewScan(string assetTag, DataTable itemDetail);

    }
}
