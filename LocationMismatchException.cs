using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryScanner
{
    public class LocationMismatchException : Exception
    {
        public string ExpectedLocation { get; private set; }

        public string ScannedLocation { get; private set; }

        public string ItemAssetTag { get; private set; }

        public LocationMismatchException(string expectedLocation, string scanLocation, string itemAssetTag)
        {
            ExpectedLocation = expectedLocation;
            ScannedLocation = scanLocation;
            ItemAssetTag = itemAssetTag;
        }

    }
}
