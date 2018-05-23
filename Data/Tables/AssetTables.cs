namespace InventoryScanner.Data.Tables
{
    public class DeviceTable
    {
        public const string TableName = "devices";
        public const string Id = "dev_UID";
        public const string Description = "dev_description";
        public const string Serial = "dev_serial";
        public const string Location = "dev_location";
        public const string DeviceType = "dev_eq_type";
        public const string CurrentUser = "dev_cur_user";
        public const string Status = "dev_status";
    }

    public class MunisDepartmentsTable
    {
        public const string TableName = "munis_departments";
        public const string Id = "id";
        public const string AssetLocation = "asset_location_code";
        public const string Department = "munis_department_code";
        public const string MunisLocation = "munis_location_code";
    }

    public class ScansTable
    {
        public const string TableName = "asset_scans";
        public const string Id = "id";
        public const string Datestamp = "datestamp";
        public const string Location = "location";
        public const string User = "scan_user";
    }

    public class PingHistoryTable
    {
        public const string TableName = "device_ping_history";
        public const string Id = "id";
        public const string DeviceGuid = "device_guid";
        public const string IPAddress = "ip";
        public const string Hostname = "hostname";
        public const string Timestamp = "timestamp";
    }

    public class SubnetLocationsTable
    {
        public const string TableName = "subnet_locations";
        public const string Id = "id";
        public const string Subnet = "subnet";
        public const string Description = "description";
    }

    public class ItemDetailTable
    {
        public const string TableName = "item_detail";
        public const string ScanLocation = "scan_location";
        public const string ScanDate = "scan_date";
        public const string Scanned = "was_scanned";
        public const string ScanUser = "scan_user";
        public const string ScanType = "scan_type";
    }

    public class ScanItemsTable
    {
        public const string TableName = "asset_scan_items";
        public const string Id = "id";

        public const string AssetTag = "asset_tag";
        public const string Serial = "serial";
        public const string Location = "scan_location";
        public const string ScanType = "scan_type";
        public const string ScanUser = "scan_user";
        public const string Datestamp = "scan_date";
        public const string ScanId = "scan_id";
        public const string ScanYear = "scan_year";
        public const string ScanStatus = "scan_status";

    }

    public class ScanItemDuplicatesTable
    {
        public const string TableName = "asset_scan_duplicates";
        public const string Id = "id";

        public const string AssetTag = "asset_tag";
        public const string Serial = "serial";
        public const string Locaton = "scan_location";
        public const string ScanType = "scan_type";
        public const string ScanUser = "scan_user";
        public const string Datestamp = "scan_date";
        public const string ScanId = "scan_id";
        public const string ScanYear = "scan_year";
       
    }

    
    public static class DeviceCodesTable
    {
        public const string TableName = "dev_codes";
        public const string Type = "type_name";
        public const string HumanValue = "human_value";
        public const string DBvalue = "db_value";
        public const string Color = "hex_color";
        public const string ID = "id";
    }

    

    public class ComboCodesBaseCols
    {
        public const string TypeName = "type_name";
        public const string DisplayValue = "human_value";
        public const string CodeValue = "db_value";
        public const string Id = "id";
        public const string Color = "hex_color";
    }

    public class DeviceComboCodesCols : ComboCodesBaseCols
    {
        public const string TableName = "dev_codes";
        public const string MunisCode = "munis_code";
    }
}