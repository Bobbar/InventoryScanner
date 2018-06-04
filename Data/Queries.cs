using InventoryScanner.Data.Tables;
using InventoryScanner.Data.Classes;
using System.Collections.Generic;

namespace InventoryScanner.Data
{
    public static class Queries
    {
        public static class Assets
        {
            /// <summary>
            /// SELECT * FROM <paramref name="attribTable"/> LEFT OUTER JOIN munis_codes on <paramref name="attribName"/>.db_value = munis_codes.asset_man_code WHERE type_name ='<paramref name="attribName"/>' ORDER BY <see cref="ComboCodesBaseCols.DisplayValue"/>
            /// </summary>
            /// <param name="attribTable"></param>
            /// <param name="attribName"></param>
            /// <returns></returns>
            public static string SelectAttributeCodes(string attribTable, string attribName)
            {
                return "SELECT * FROM " + attribTable + " LEFT OUTER JOIN munis_codes on " + attribTable + ".db_value = munis_codes.asset_man_code WHERE type_name ='" + attribName + "' ORDER BY " + ComboCodesBaseCols.DisplayValue;
            }

            public static string SelectDeviceBySerial(string deviceSerial)
            {
                return "SELECT * FROM " + DeviceTable.TableName + " WHERE " + DeviceTable.Serial + " = '" + deviceSerial + "'";
            }

            public static string SelectMaxPingHistory()
            {
                var query = @"SELECT ping.*
 FROM device_ping_history ping
 INNER JOIN (SELECT device_guid, MAX(`timestamp`) AS MaxDateTime
 FROM device_ping_history
 GROUP BY device_guid) groupdev
 ON ping.device_guid = groupdev.device_guid
 AND ping.timestamp = groupdev.MaxDateTime";

                return query;
            }

            public static string SelectSubnetLocations()
            {
                return "SELECT * FROM " + SubnetLocationsTable.TableName;
            }

            public static string SelectScanItemsByScanYear(string year)
            {
                var query = "SELECT * FROM " + ScanItemsTable.TableName + " WHERE " + ScanItemsTable.ScanYear + " = '" + year + "' LOCK IN SHARE MODE";
                return query;
            }

            public static string SelectScanById(string id)
            {
                var query = "SELECT * FROM " + ScansTable.TableName + " WHERE " + ScansTable.Id + " = '" + id + "'";
                return query;
            }

            public static string SelectCompletedScansByYear(string year)
            {
                var query = "SELECT * FROM " + ScanItemsTable.TableName + " WHERE " + ScanItemsTable.ScanYear + " = '" + year + "' AND " + ScanItemsTable.ScanStatus + " = '" + ScanStatus.OK.ToString() + "'";
                return query;
            }

            public static string SelectMunisAndAssetLocationCodes()
            {
                var query = "SELECT * FROM " + MunisDepartmentsTable.TableName + " INNER JOIN " + DeviceCodesTable.TableName + " ON " + DeviceCodesTable.Type + " = 'LOCATION' AND " + MunisDepartmentsTable.AssetLocation + " = " + DeviceCodesTable.DBvalue;
                return query;
            }

            public static string SelectAllDevices()
            {
                return "SELECT * FROM " + DeviceTable.TableName + " ORDER BY " + DeviceTable.Serial;
            }
        }

        public static class Munis
        {
            public static string SelectDepartmentByLocation(string locationCode)
            {
                return "SELECT * FROM " + MunisDepartmentsTable.TableName + " WHERE " + MunisDepartmentsTable.AssetLocation + " = '" + locationCode + "'";
            }

            public static string SelectScanItemsByDepartment(string departmentCode)
            {
                var query = "";
                query += "SELECT a_asset_number, a_department_code, fa_acquire_date, fa_status,fa_class_code,fs_subclass_code,fa_tag_number,fa_serial_number,a_asset_desc,a_location,fa_purchase_memo";
                query += " FROM fa_master";
                query += " WHERE a_department_code = '" + departmentCode + "' AND fa_status = 'A' AND fs_subclass_code IN (411,403,422,410,430,400,438,415,437,416,429)";
                query += " ORDER BY a_location";

                return query;
            }

            public static string SelectAllScanItems()
            {
                var query = "";
                query += "SELECT a_asset_number, a_department_code, fa_acquire_date, fa_status,fa_class_code,fs_subclass_code,fa_tag_number,fa_serial_number,a_asset_desc,a_location,fa_purchase_memo";
                query += " FROM fa_master";
                query += " WHERE a_department_code IN ('5200','5205','5210') AND fa_status = 'A' AND fs_subclass_code IN (411,403,422,410,430,400,438,415,437,416,429)";
                query += " ORDER BY a_location";

                return query;
            }


            public static string SelectScanItemsByLocation(string locationCode)
            {
                var query = "";
                query += "SELECT a_asset_number, a_department_code, fa_acquire_date, fa_status,fa_class_code,fs_subclass_code,fa_tag_number,fa_serial_number,a_asset_desc,a_location,fa_purchase_memo";
                query += " FROM fa_master";
                query += " WHERE a_location = '" + locationCode + "' AND fa_status = 'A' AND fs_subclass_code IN (411,403,422,410,430,400,438,415,437,416,429)";
                query += " ORDER BY a_asset_number";

                return query;
            }

            public static string SelectLocations()
            {
                var query = "SELECT * FROM " + MunisLocations.TableName;

                return query;
            }
        }

        public static class Sqlite
        {
            //           public static string SelectAssetDetailBySerial(string serial)
            //           {
            //               var query = @"SELECT * FROM fa_master
            //LEFT JOIN devices
            //ON TRIM(fa_master.fa_serial_number) = devices.dev_serial
            //LEFT JOIN device_ping_history
            //ON device_ping_history.device_guid = devices.dev_UID
            //WHERE fa_master.fa_serial_number LIKE '" + serial + "%'";

            //               return query;
            //           }

            public static string SelectAssetDetailBySerial(string serial)
            {
                var query = "SELECT * FROM " + ItemDetailTable.TableName + " WHERE " + MunisFixedAssetTable.Serial + " = '" + serial + "'";

                return query;
            }

            public static string SelectAssetDetailByAssetTag(string assetTag)
            {
                var query = "SELECT * FROM " + ItemDetailTable.TableName + " WHERE " + MunisFixedAssetTable.Asset + " = '" + assetTag + "'";

                return query;
            }

            public static string SelectAllAssetDetails()
            {
                var query = "SELECT * FROM " + ItemDetailTable.TableName;

                return query;
            }

            public static string SelectAllAssetDetailsWithLocationFilter(List<string> locationFilters)
            {
                string locations = "(";

                foreach (var filter in locationFilters)
                {
                    locations += filter;

                    if (locationFilters.IndexOf(filter) != (locationFilters.Count - 1)) locations += ", ";
                }

                locations += ")";

                var query = "SELECT * FROM " + ItemDetailTable.TableName + " WHERE " + MunisFixedAssetTable.Location + " IN " + locations;

                return query;
            }

            public static string JoinAllAssetDetails()
            {
                var query = @"SELECT * FROM fa_master
 LEFT JOIN devices
 ON TRIM(fa_master.fa_serial_number) = devices.dev_serial
 LEFT JOIN device_ping_history
 ON device_ping_history.device_guid = devices.dev_UID";

                return query;
            }
        }
    }
}