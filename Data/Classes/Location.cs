﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryScanner.Data.ClassMapping;
using InventoryScanner.Data.Tables;
using System.Data;

namespace InventoryScanner
{
   public  class Location : DataMapObject
    {
        [DataColumnName(MunisDepartments.AssetLocation)]
        public string AssetCode { get; private set; }

        [DataColumnName(MunisDepartments.Department)]
        public string DepartmentCode { get; private set; }

        [DataColumnName(MunisDepartments.MunisLocation)]
        public string MunisCode { get; private set; }

        [DataColumnName(MunisDepartments.Id)]
        public override string Guid
        {
            get; set;
        }

        [DataColumnName(MunisDepartments.TableName)]
        public override string TableName
        {
            get; set;
        }

        public Location(string assetCode, string departmentCode, string munisCode)
        {
            AssetCode = assetCode;
            DepartmentCode = departmentCode;
            MunisCode = munisCode;
        }

        public Location(DataTable data) : base(data) { }
    }
}