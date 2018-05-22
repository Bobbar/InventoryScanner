using InventoryScanner.Data;
using InventoryScanner.Data.Classes;
using InventoryScanner.Data.Functions;
using InventoryScanner.Data.Munis;
using InventoryScanner.Data.Tables;
using InventoryScanner.Helpers;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace InventoryScanner
{
    public class ScanningController
    {
        private IScanning view;
        private Scan currentScan;
        private Timer syncTimer;
        private bool syncRunning = false;

        public ScanningController(IScanning view)
        {
            this.view = view;
            view.SetController(this);
            InitSyncTimer();
        }

        private void InitSyncTimer()
        {
            syncTimer = new Timer();
            syncTimer.Interval = 1000;
            syncTimer.Elapsed += SyncTimer_Elapsed;
        }

        private void SyncTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (currentScan != null) SyncDataAsync();
        }

        public async void StartScan(Location location, DateTime datestamp, string scanEmployee)
        {
            currentScan = InsertNewScan(location, datestamp, scanEmployee);

            //var scanItemsQuery = Queries.Munis.SelectScanItemsByDepartment(location.DepartmentCode);
            //var scanItemsQuery = Queries.Munis.SelectScanItemsByLocation(location.MunisCode);
            var scanItemsQuery = Queries.Munis.SelectAllScanItems();

            using (var munisResults = await MunisDatabase.ReturnSqlTableAsync(scanItemsQuery))
            using (var assetResults = GetAssetManagerResults(munisResults))
            {
                munisResults.TableName = MunisFixedAssetTable.TableName;
                CleanMunisFields(munisResults);
                CacheScanDetails(munisResults, MunisFixedAssetTable.Asset, assetResults, DeviceTable.Id, currentScan.ID);

                SyncDataAsync();

                LoadCurrentScanItems();
            }
            syncTimer.Start();
        }

        private void LoadCurrentScanItems()
        {
            using (var detailResults = DBFactory.GetSqliteDatabase(currentScan.ID).DataTableFromQueryString(Queries.Sqlite.SelectAllAssetDetails()))
            {
                view.LoadScanItems(detailResults);
            }
        }

        public DataTable DetailOfAsset(string assetTag)
        {
            using (var results = DBFactory.GetSqliteDatabase(currentScan.ID).DataTableFromQueryString(Queries.Sqlite.SelectAssetDetailByAssetTag(assetTag)))
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
            detailsResults.Columns.Add(ScanItemsTable.ScanId, typeof(string));
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

        public void SubmitNewScanItem(string assetTag, ScanType scanType)
        {
            using (var itemDetail = DetailOfAsset(assetTag))
            {
                var itemRow = itemDetail.Rows[0];

                itemRow[ScanItemsTable.Locaton] = currentScan.MunisLocation.MunisCode;
                itemRow[ScanItemsTable.ScanType] = scanType.ToString();
                itemRow[ScanItemsTable.ScanUser] = currentScan.User;
                itemRow[ScanItemsTable.Datestamp] = DateTime.Now.ToString(DataConsistency.DBDateTimeFormat);
                itemRow[ScanItemsTable.ScanYear] = DateTime.Now.Year.ToString();
                itemRow[ScanItemsTable.ScanStatus] = ScanStatus.OK.ToString();
                itemRow[ScanItemsTable.ScanId] = currentScan.ID;

                var updatedRows = DBFactory.GetSqliteDatabase(currentScan.ID).UpdateTable(Queries.Sqlite.SelectAssetDetailByAssetTag(assetTag), itemDetail);

                LoadCurrentScanItems();
            }
        }

        public async void SyncDataAsync()
        {
            if (syncRunning) return;

            OtherFunctions.StartTimer();
            try
            {
                syncRunning = true;
                var hasChanged = await Task.Run(() => { return TrySyncData(); });

                // Refresh view.
                if (hasChanged) LoadCurrentScanItems();
            }
            catch (Exception)
            {
                // We want this to fail silently.
            }
            finally
            {
                syncRunning = false;
            }

            OtherFunctions.StopTimer();
        }

        private bool TrySyncData()
        {
            bool hasChanged = false;
            var localQuery = Queries.Sqlite.SelectAllAssetDetails();
            var remoteQuery = Queries.Assets.SelectScanItemsByScanYear(currentScan.Datestamp.Year.ToString());

            using (var remoteTrans = DBFactory.GetMySqlDatabase().StartTransaction())
            using (var remoteResults = DBFactory.GetMySqlDatabase().DataTableFromQueryString(remoteQuery))
            using (var localTrans = DBFactory.GetSqliteDatabase(currentScan.ID).StartTransaction())
            using (var localResults = DBFactory.GetSqliteDatabase(currentScan.ID).DataTableFromQueryString(localQuery))
            {
                try
                {
                    foreach (DataRow localRow in localResults.Rows)
                    {
                        var remoteRow = remoteResults.AsEnumerable().Where(r => r[ScanItemsTable.AssetTag].ToString() == localRow[MunisFixedAssetTable.Asset].ToString()).SingleOrDefault();

                        // If a matching entry on remote exists.
                        if (remoteRow != null)
                        {
                            // If local entry DOES NOT HAVE a scan.
                            if (string.IsNullOrEmpty(localRow[ScanItemsTable.Datestamp].ToString()))
                            {
                                // Update local with remote.
                                hasChanged = true;
                                localRow[ScanItemsTable.Locaton] = remoteRow[ScanItemsTable.Locaton];
                                localRow[ScanItemsTable.ScanType] = remoteRow[ScanItemsTable.ScanType];
                                localRow[ScanItemsTable.ScanUser] = remoteRow[ScanItemsTable.ScanUser];
                                localRow[ScanItemsTable.Datestamp] = remoteRow[ScanItemsTable.Datestamp];
                                localRow[ScanItemsTable.ScanId] = remoteRow[ScanItemsTable.ScanId];
                                localRow[ScanItemsTable.ScanStatus] = remoteRow[ScanItemsTable.ScanStatus];
                            }
                            else
                            {
                                // If local HAS a scan and the locations match.
                                if (localRow[ScanItemsTable.Locaton].ToString() == remoteRow[ScanItemsTable.Locaton].ToString())
                                {
                                    // If the entry is from another scan.
                                    if (localRow[ScanItemsTable.ScanId].ToString() != remoteRow[ScanItemsTable.ScanId].ToString() || localRow[ScanItemsTable.ScanStatus].ToString() != remoteRow[ScanItemsTable.ScanStatus].ToString())
                                    {
                                        // Update local with remote.
                                        hasChanged = true;
                                        localRow[ScanItemsTable.Locaton] = remoteRow[ScanItemsTable.Locaton];
                                        localRow[ScanItemsTable.ScanType] = remoteRow[ScanItemsTable.ScanType];
                                        localRow[ScanItemsTable.ScanUser] = remoteRow[ScanItemsTable.ScanUser];
                                        localRow[ScanItemsTable.Datestamp] = remoteRow[ScanItemsTable.Datestamp];
                                        localRow[ScanItemsTable.ScanId] = remoteRow[ScanItemsTable.ScanId];
                                        localRow[ScanItemsTable.ScanStatus] = remoteRow[ScanItemsTable.ScanStatus];
                                    }
                                }
                                else
                                {
                                    // If local and remote locations DO NOT MATCH and not already marked duplicate,
                                    // set both to duplicate and add entries to duplicate table for later resolution.
                                    if (remoteRow[ScanItemsTable.ScanStatus].ToString() != ScanStatus.Duplicate.ToString())
                                    {
                                        hasChanged = true;
                                        localRow[ScanItemsTable.ScanStatus] = ScanStatus.Duplicate.ToString();
                                        remoteRow[ScanItemsTable.ScanStatus] = ScanStatus.Duplicate.ToString();

                                        AddDuplicateScanEntries(localRow, remoteRow, remoteTrans);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // If local item has been scanned, add it to remote.
                            if (!string.IsNullOrEmpty(localRow[ScanItemsTable.Datestamp].ToString()))
                            {
                                // Map local scan fields to remote fields.
                                var newRow = remoteResults.Rows.Add();
                                newRow[ScanItemsTable.AssetTag] = localRow[MunisFixedAssetTable.Asset];
                                newRow[ScanItemsTable.Serial] = localRow[MunisFixedAssetTable.Serial];
                                newRow[ScanItemsTable.Locaton] = localRow[ScanItemsTable.Locaton];
                                newRow[ScanItemsTable.ScanType] = localRow[ScanItemsTable.ScanType];
                                newRow[ScanItemsTable.ScanUser] = localRow[ScanItemsTable.ScanUser];
                                newRow[ScanItemsTable.Datestamp] = localRow[ScanItemsTable.Datestamp];
                                newRow[ScanItemsTable.ScanId] = localRow[ScanItemsTable.ScanId];
                                newRow[ScanItemsTable.ScanYear] = localRow[ScanItemsTable.ScanYear];
                                newRow[ScanItemsTable.ScanStatus] = localRow[ScanItemsTable.ScanStatus];
                            }
                        }
                    }

                    // Update local and remote tables.
                    DBFactory.GetSqliteDatabase(currentScan.ID).UpdateTable(localQuery, localResults, localTrans);
                    DBFactory.GetMySqlDatabase().UpdateTable(remoteQuery, remoteResults, remoteTrans);

                    localTrans.Commit();
                    remoteTrans.Commit();
                    return hasChanged;
                }
                catch (Exception)
                {
                    localTrans.Rollback();
                    remoteTrans.Rollback();
                    return false;
                }
            }
        }

        private void AddDuplicateScanEntries(DataRow localRow, DataRow remoteRow, DbTransaction remoteTrans)
        {
            var emptyTableQuery = "SELECT * FROM " + ScanItemDuplicatesTable.TableName + " LIMIT 0";
            using (var dupTable = DBFactory.GetMySqlDatabase().DataTableFromQueryString(emptyTableQuery))
            {
                var localDupRow = dupTable.Rows.Add();
                localDupRow[ScanItemDuplicatesTable.AssetTag] = localRow[MunisFixedAssetTable.Asset];
                localDupRow[ScanItemDuplicatesTable.Serial] = localRow[MunisFixedAssetTable.Serial];
                localDupRow[ScanItemDuplicatesTable.Locaton] = localRow[ScanItemsTable.Locaton];
                localDupRow[ScanItemDuplicatesTable.ScanType] = localRow[ScanItemsTable.ScanType];
                localDupRow[ScanItemDuplicatesTable.ScanUser] = localRow[ScanItemsTable.ScanUser];
                localDupRow[ScanItemDuplicatesTable.Datestamp] = localRow[ScanItemsTable.Datestamp];
                localDupRow[ScanItemDuplicatesTable.ScanId] = localRow[ScanItemsTable.ScanId];
                localDupRow[ScanItemDuplicatesTable.ScanYear] = localRow[ScanItemsTable.ScanYear];

                var remoteDupRow = dupTable.Rows.Add();
                remoteDupRow[ScanItemDuplicatesTable.AssetTag] = remoteRow[ScanItemsTable.AssetTag];
                remoteDupRow[ScanItemDuplicatesTable.Serial] = remoteRow[ScanItemsTable.Serial];
                remoteDupRow[ScanItemDuplicatesTable.Locaton] = remoteRow[ScanItemsTable.Locaton];
                remoteDupRow[ScanItemDuplicatesTable.ScanType] = remoteRow[ScanItemsTable.ScanType];
                remoteDupRow[ScanItemDuplicatesTable.ScanUser] = remoteRow[ScanItemsTable.ScanUser];
                remoteDupRow[ScanItemDuplicatesTable.Datestamp] = remoteRow[ScanItemsTable.Datestamp];
                remoteDupRow[ScanItemDuplicatesTable.ScanId] = remoteRow[ScanItemsTable.ScanId];
                remoteDupRow[ScanItemDuplicatesTable.ScanYear] = remoteRow[ScanItemsTable.ScanYear];

                DBFactory.GetMySqlDatabase().UpdateTable(emptyTableQuery, dupTable, remoteTrans);
            }
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