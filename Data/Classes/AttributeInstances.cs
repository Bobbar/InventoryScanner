﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryScanner.Data.Classes
{
    public static class AttributeInstances
    {
        public static class DeviceAttributes
        {
            public static DbAttributes Locations;
            public static DbAttributes ChangeType;
            public static DbAttributes EquipType;
            public static DbAttributes OSType;
            public static DbAttributes StatusType;
            public static DbAttributes SubnetLocation;
        }

        public static class MunisAttributes
        {
            public static DbAttributes MunisLocations;
            public static DbAttributes MunisToAssetLocations;

        }
    }
}
