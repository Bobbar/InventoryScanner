﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryScanner.Data.Tables;
using Databases.Data;
using Databases.Data.Mapping;

namespace InventoryScanner.Data.Classes
{
    public class Scan : MappedObject
    {
        //[DataColumnName(Scans.Id)]
        public string ID { get; set; }

        [DataColumnName(ScansTable.User)]
        public string User { get; private set; }

        [DataColumnName(ScansTable.Datestamp)]
        public DateTime Datestamp { get; private set; }

        [DataColumnName(ScansTable.Location)]
        public string ScanLocation { get; private set; }

        public Location MunisLocation { get; set; }

        [DataColumnName(ScansTable.Id)]
        public override string Guid
        {
            get { return ID; } set { ID = value; }
        }

        // [DataColumnName(Scans.TableName)]
        public override string TableName { get; set; } = ScansTable.TableName;

        public override IDatabase Database
        {
            get
            {
                return DBFactory.GetMySqlDatabase();
            }
        }

        public Scan(string id, string employee, Location location)
        {
            ID = id;
            User = employee;
            MunisLocation = location;
            ScanLocation = location.AssetCode;
        }

        public Scan(DateTime datestamp, string employee, Location location)
        {
            ID = null;
            Datestamp = datestamp;
            User = employee;
            MunisLocation = location;
            ScanLocation = location.AssetCode;
        }

        public Scan(string id, DateTime datestamp, string employee, Location location)
        {
            ID = id;
            Datestamp = datestamp;
            User = employee;
            MunisLocation = location;
            ScanLocation = location.AssetCode;
        }

    }
}
