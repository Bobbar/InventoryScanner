using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryScanner.Data.ClassMapping;
using InventoryScanner.Data.Tables;
using System.Data;

namespace InventoryScanner.Data.Classes
{
    public class Location : DataMapObject
    {
        [DataColumnName(MunisDepartmentsTable.AssetLocation)]
        public string AssetCode { get; private set; }

        [DataColumnName(MunisDepartmentsTable.Department)]
        public string DepartmentCode { get; private set; }

        [DataColumnName(MunisDepartmentsTable.MunisLocation)]
        public string MunisCode { get; private set; }

        [DataColumnName(MunisDepartmentsTable.Id)]
        public override string Guid
        {
            get; set;
        }

        // [DataColumnName(MunisDepartments.TableName)]
        public override string TableName { get; set; } = MunisDepartmentsTable.TableName;

        public Location(string assetCode, string departmentCode, string munisCode)
        {
            AssetCode = assetCode;
            DepartmentCode = departmentCode;
            MunisCode = munisCode;
        }

        public Location(string assetCode)
        {
            AssetCode = assetCode;
        }

        public Location(DataTable data) : base(data) { }
    }
}
