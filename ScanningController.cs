using InventoryScanner.Data;
using InventoryScanner.Data.Functions;
using InventoryScanner.Data.Munis;
using InventoryScanner.Data.Tables;
using InventoryScanner.Data.Classes;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace InventoryScanner
{
    public class ScanningController
    {
        private IScanning view;
        // private string scanId;
        private Scan currentScan;

        public ScanningController(IScanning view)
        {
            this.view = view;
            view.SetController(this);
        }

        public async void StartScan(Location location, DateTime datestamp, string scanEmployee)
        {
            currentScan = InsertNewScan(location, datestamp, scanEmployee);

            //var scanItemsQuery = Queries.Munis.SelectScanItemsByDepartment(location.DepartmentCode);
            var scanItemsQuery = Queries.Munis.SelectScanItemsByLocation(location.MunisCode);


            using (var munisResults = await MunisDatabase.ReturnSqlTableAsync(scanItemsQuery))
            using (var assetResults = GetAssetManagerResults(munisResults))
            {
                munisResults.TableName = MunisFixedAssetTable.TableName;
                CleanMunisFields(munisResults);
                CacheScanDetails(munisResults, MunisFixedAssetTable.Asset, assetResults, DeviceTable.Id, currentScan.ID);

                LoadCurrentScanItems();

                //using (var detailResults = DBFactory.GetSqliteDatabase(currentScan.ID).DataTableFromQueryString(Queries.Sqlite.SelectAllAssetDetails()))
                //{
                //    view.LoadScanItems(detailResults);
                //}
            }
        }

        private void LoadCurrentScanItems()
        {
            using (var detailResults = DBFactory.GetSqliteDatabase(currentScan.ID).DataTableFromQueryString(Queries.Sqlite.SelectAllAssetDetails()))
            {
                view.LoadScanItems(detailResults);
            }
        }

        public DataTable DetailOfAsset(string serial)
        {
            using (var results = DBFactory.GetSqliteDatabase(currentScan.ID).DataTableFromQueryString(Queries.Sqlite.SelectAssetDetailBySerial(serial)))
            {
                return results;
            }
        }

        private void CacheScanDetails(DataTable munisResults, string munisKeyColumn, DataTable assetResults, string assetKeyColumn, string scanId)
        {
            using (var trans = DBFactory.GetSqliteDatabase(scanId).StartTransaction())
            {
                // Add tables to Sqlite cache.
                SqliteFunctions.AddTableToDB(munisResults, munisKeyColumn, scanId, trans);
                SqliteFunctions.AddTableToDB(assetResults, assetKeyColumn, scanId, trans);
                CachePingHistory(trans);

                // Join the tables and add the result to the Sqlite cache.
                using (var cmd = DBFactory.GetSqliteDatabase(scanId).GetCommand(Queries.Sqlite.JoinAllAssetDetails()))
                using (var allDetails = DBFactory.GetSqliteDatabase(scanId).DataTableFromCommand(cmd, trans))
                {
                    allDetails.TableName = ItemDetailTable.TableName;
                    AddScanStatusColumns(allDetails);
                    SqliteFunctions.AddTableToDB(allDetails, MunisFixedAssetTable.Asset, scanId, trans);
                }

                trans.Commit();
            }
        }

        private void CachePingHistory(DbTransaction trans)
        {
            using (var results = DBFactory.GetMySqlDatabase().DataTableFromQueryString(Queries.Assets.SelectMaxPingHistory()))
            {
                results.TableName = PingHistoryTable.TableName;
                SetSubnets(results);
                SqliteFunctions.AddTableToDB(results, PingHistoryTable.Id, currentScan.ID, trans);
            }
        }

        private void AddScanStatusColumns(DataTable detailsResults)
        {
          //  detailsResults.Columns.Add(ItemDetailTable.Scanned, typeof(bool));
            detailsResults.Columns.Add(ScanItemsTable.Locaton, typeof(string));
            detailsResults.Columns.Add(ScanItemsTable.Datestamp, typeof(DateTime));
            detailsResults.Columns.Add(ScanItemsTable.ScanType, typeof(string));
            detailsResults.Columns.Add(ScanItemsTable.ScanUser, typeof(string));
            detailsResults.Columns.Add(ScanItemsTable.ScanStatus, typeof(string));
            detailsResults.Columns.Add(ScanItemsTable.ScanYear, typeof(string));
        }

        private void CleanMunisFields(DataTable munisResults)
        {
            foreach (DataRow row in munisResults.Rows)
            {
                for (int i = 0; i < munisResults.Columns.Count; i++)
                {
                    if (row[i].GetType() == typeof(string))
                    {
                        row[i] = row[i].ToString().Trim();
                    }
                }
            }
        }

        private void SetSubnets(DataTable pinghistoryResults)
        {
            foreach (DataRow row in pinghistoryResults.Rows)
            {
                row[PingHistoryTable.IPAddress] = row[PingHistoryTable.IPAddress].ToString().Substring(0, 8) + ".0";
            }
        }

        private DataTable JoinResults(DataTable munisResults, DataTable assetResults)
        {
            var joinedResults = new DataTable();

            foreach (DataColumn column in munisResults.Columns)
            {
                joinedResults.Columns.Add(column.ColumnName, column.DataType);
            }

            foreach (DataColumn column in assetResults.Columns)
            {
                joinedResults.Columns.Add(column.ColumnName, column.DataType);
            }

            //// Works but doesn't include missing munis rows.
            //var joinQuery = from munis in munisResults.AsEnumerable()
            //                join asset in assetResults.AsEnumerable() on munis.Field<string>(MunisFixedAssetTable.Serial)?.ToString().Trim() equals asset.Field<string>(DeviceTable.Serial)
            //                where !string.IsNullOrEmpty(munis.Field<string>(MunisFixedAssetTable.Serial))
            //                select joinedResults.LoadDataRow(BuildDataRowObject(munis, asset, joinedResults), false);

            //FINALLY!!!
            var joinQuery = from munis in munisResults.AsEnumerable()
                            join asset in assetResults.AsEnumerable() on munis.Field<string>(MunisFixedAssetTable.Serial)?.ToString().Trim() equals asset.Field<string>(DeviceTable.Serial)
                            into joined
                            from jt in joined.DefaultIfEmpty()
                            select joinedResults.LoadDataRow(BuildDataRowObject(munis, jt, joinedResults), false);

            joinedResults = joinQuery.CopyToDataTable();

            return joinedResults;
        }

        private object[] BuildDataRowObject(DataRow munisRow, DataRow assetRow, DataTable targetTable)
        {
            var columnCount = targetTable.Columns.Count;//munisRow.Table.Columns.Count + assetRow.Table.Columns.Count;
            var tmpObject = new object[columnCount];
            //int columnSeq = 0;

            for (int i = 0; i < targetTable.Columns.Count; i++)
            {
                DataColumn column = targetTable.Columns[i];

                if (munisRow != null)
                {
                    if (munisRow.Table.Columns.Contains(column.ColumnName))
                    {
                        if (munisRow[column.ColumnName] is string)
                        {
                            tmpObject[i] = munisRow[column.ColumnName].ToString().Trim();
                        }
                        else
                        {
                            tmpObject[i] = munisRow[column.ColumnName];
                        }
                    }
                }

                if (assetRow != null)
                {
                    if (assetRow.Table.Columns.Contains(column.ColumnName))
                    {
                        tmpObject[i] = assetRow[column.ColumnName];
                    }
                }
            }

            return tmpObject;
        }

        private DataTable GetAssetManagerResults(DataTable munisResults)
        {
            var assetTable = new DataTable(DeviceTable.TableName);

            foreach (DataRow row in munisResults.Rows)
            {
                using (var assetResult = DBFactory.GetMySqlDatabase().DataTableFromQueryString(Queries.Assets.SelectDeviceBySerial(row[MunisFixedAssetTable.Serial].ToString())))
                {
                    if (assetResult.Rows.Count > 1) throw new Exception("Duplicate Asset Device records found.");

                    if (assetResult.Rows.Count == 1)
                    {
                        assetTable.Merge(assetResult);
                        // assetTable.Rows.Add(assetResult.Rows[0]);
                    }
                }
            }

            return assetTable;
        }

        public void SubmitNewScanItem(string serial, ScanType scanType)
        {
            using (var itemDetail = DetailOfAsset(serial))
            {

                var itemRow = itemDetail.Rows[0];

                itemRow[ScanItemsTable.Locaton] = currentScan.MunisLocation.MunisCode;
                itemRow[ScanItemsTable.ScanType] = scanType.ToString();
                itemRow[ScanItemsTable.ScanUser] = currentScan.User;
                itemRow[ScanItemsTable.Datestamp] = DateTime.Now.ToString(DataConsistency.DBDateTimeFormat);
                itemRow[ScanItemsTable.ScanStatus] = ScanStatus.OK.ToString();

                var updatedRows = DBFactory.GetSqliteDatabase(currentScan.ID).UpdateTable(Queries.Sqlite.SelectAssetDetailBySerial(serial), itemDetail);

                LoadCurrentScanItems();
                //var newScanItem = new ScanItem(itemDetail);
                //newScanItem.ScanType = scanType;
                //newScanItem.Scan = currentScan;
                //newScanItem.ScanLocation = currentScan.MunisLocation.MunisCode;
                //newScanItem.Datestamp = DateTime.Now;
                //newScanItem.ScanId = currentScan.ID;
                //newScanItem.ScanYear = DateTime.Now.Year.ToString();
            }
        }

        public void SyncScans()
        {
            //1. Update remote db scans from backing store.
            // Non existant items are added, existing items are added and marked duplicate?

            using (var localResults = DBFactory.GetSqliteDatabase(currentScan.ID).DataTableFromQueryString(Queries.Sqlite.SelectAllAssetDetails()))
            using (var remoteResults = DBFactory.GetMySqlDatabase().DataTableFromQueryString(Queries.Assets.SelectScanItemsByScanYear(currentScan.Datestamp.Year.ToString())))
            {

                foreach (DataRow row in localResults.Rows)
                {
                    var remoteScan = remoteResults.Rows.Find(row[MunisFixedAssetTable.Serial]);

                }


            }

            //2. Update backing store from remote db.
            // Iterate rows and update scan info if changed.
        }

        public Location GetLocation(string locationCode)
        {
            using (var results = DBFactory.GetMySqlDatabase().DataTableFromQueryString(Queries.Munis.SelectDepartmentByLocation(locationCode)))
            {
                return new Location(results);
            }
        }


        private Scan InsertNewScan(Location location, DateTime datestamp, string scanEmployee)
        {
            var newScan = new Scan(datestamp, scanEmployee, location);
            MapObjectFunctions.InsertMapObject(newScan);

            var scanId = (int)DBFactory.GetMySqlDatabase().ExecuteScalarFromQueryString("SELECT MAX(" + ScansTable.Id + ") FROM " + ScansTable.TableName);

            newScan.ID = scanId.ToString();

            return newScan;
        }
    }
}