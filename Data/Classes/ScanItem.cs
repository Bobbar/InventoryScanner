using System;
using InventoryScanner.Data.ClassMapping;
using InventoryScanner.Data.Tables;
using System.Data;
using System.Data.Common;


namespace InventoryScanner.Data.Classes
{
    public class ScanItem : DataMapObject
    {

        public string ID { get; set; }

        [DataColumnName(ScanItemsTable.Serial)]
        public string Serial { get; set; }

        [DataColumnName(ScanItemsTable.AssetTag)]
        public string AssetTag { get; set; }

        [DataColumnName(ScanItemsTable.Location)]
        public string ScanLocation { get; set; }

        [DataColumnName(ScanItemsTable.ScanType)]
        public ScanType ScanType { get; set; }

        [DataColumnName(ScanItemsTable.ScanUser)]
        public string ScanUser { get; set; }

        [DataColumnName(ScanItemsTable.ScanId)]
        public string ScanId { get; set; }

        [DataColumnName(ScanItemsTable.ScanYear)]
        public string ScanYear { get; set; }

        [DataColumnName(ScanItemsTable.Datestamp)]
        public DateTime Datestamp { get; set; }


        public Scan Scan { get; set; }


        public override string TableName { get; set; } = ScanItemsTable.TableName;

        [DataColumnName(ScanItemsTable.Id)]
        public override string Guid
        {
            get { return ID; }
            set { ID = value; }
        }

        public ScanItem()
        {

        }

        public ScanItem(DataTable data) : base(data)
        {

        }

        public ScanItem(DataRow data) : base(data)
        {

        }


    }
}