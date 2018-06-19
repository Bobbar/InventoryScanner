using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryScanner
{
    public class ItemNotFoundException : Exception
    {
        public string AssetTag { get; private set; }

        public ItemNotFoundException(string assetTag)
        {
            AssetTag = assetTag;
        }
    }
}
