using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryScanner.Data.Tables
{
    public static class MunisFixedAssetTable
    {
        public const string TableName = "fa_master";

        public const string Description = "a_asset_desc";
        public const string Serial = "fa_serial_number";
        public const string Asset = "a_asset_number";

        public const string Location = "a_location";
        public const string Department = "a_department_code";
        public const string PurchaseDate = "fa_acquire_date";
    }
}
