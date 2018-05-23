using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryScanner.Data.Classes
{
    public enum ScanStatus
    {
        OK,
        Error,
        Duplicate,
        LocationMismatch
    }
}
