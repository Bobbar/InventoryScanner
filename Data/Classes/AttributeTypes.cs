using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryScanner.Data.Classes
{
    internal static class AttributeTypes
    {
        public sealed class Device
        {
            public const string Location = "LOCATION";
            public const string ChangeType = "CHANGETYPE";
            public const string EquipType = "EQ_TYPE";
            public const string OSType = "OS_TYPE";
            public const string StatusType = "STATUS_TYPE";
        }

    }
}
