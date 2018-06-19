using InventoryScanner.BarcodeScanning;
using InventoryScanner.Data;
using InventoryScanner.Data.Classes;
using InventoryScanner.Data.Functions;
using InventoryScanner.Data.Munis;
using InventoryScanner.Data.Tables;
using InventoryScanner.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace InventoryScanner.ScanController
{
    public class ScanningController : IDisposable
    {
        private IScanningUI view;
        private Scan currentScan;
        private Timer syncTimer;
        private bool syncRunning = false;
        private IScannerInput scannerInput;

        public event EventHandler<Exception> ExceptionOccured;

        private void OnExceptionOccured(Exception ex)
        {
            ExceptionOccured?.Invoke(this, ex);
        }

        public event EventHandler<ScannerStatusEvent> ScannerStatusChanged;

        private void OnScannerStatusChanged(ScannerStatusEvent type)
        {
            ScannerStatusChanged?.Invoke(this, type);
        }

        public ScanningController(IScanningUI view)
        {
            this.view = view;
            view.SetController(this);
            InitSyncTimer();
        }

        public void InitScanner(string portName)
        {
            if (string.IsNullOrEmpty(portName)) return;

            try
            {
                if (scannerInput != null)
                {
                    scannerInput.Dispose();
                }

                scannerInput = new SerialPortReader(portName);
                scannerInput.NewScanReceived += ScannerInput_NewScanReceived;
                scannerInput.ExceptionOccured += ScannerInput_ExceptionOccured;

                if (scannerInput.StartScanner())
                    OnScannerStatusChanged(ScannerStatusEvent.Connected);

            }
            catch (Exception ex)
            {
                Logging.Logger("ERROR: " + ex.ToString());
                Console.WriteLine(ex.ToString());
                OnExceptionOccured(ex);
                OnScannerStatusChanged(ScannerStatusEvent.Error);
            }
        }

        private void ScannerInput_ExceptionOccured(object sender, Exception e)
        {
            OnExceptionOccured(e);
            OnScannerStatusChanged(ScannerStatusEvent.LostConnection);
        }



        private void ScannerInput_NewScanReceived(object sender, string e)
        {
            // TODO: Validate data

            SubmitNewScanItem(e, ScanType.Scanned);
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

        public void StartScan(Scan scan)
        {
            currentScan = scan;
            SyncDataAsync();
            LoadCurrentScanItems();
            syncTimer.Start();
        }

        public void RefreshCurrentItems(List<string> filterList = null)
        {
            LoadCurrentScanItems(filterList);
        }

        public void PauseSync()
        {
            syncTimer.Stop();
        }

        public void ResumeSync()
        {
            syncTimer.Start();
        }

        private void LoadCurrentScanItems(List<string> filterList = null)
        {
            if (currentScan == null) return;

            var query = "";

            if (filterList != null)
            {
                query = Queries.Sqlite.SelectAllAssetDetailsWithLocationFilter(filterList);
            }
            else
            {
                query = Queries.Sqlite.SelectAllAssetDetails();
            }

            using (var detailResults = DBFactory.GetSqliteScanDatabase(currentScan.ID).DataTableFromQueryString(query))
            {
                view.LoadScanItems(detailResults);
            }
        }

        public DataTable DetailOfAsset(string assetTag)
        {
            using (var results = DBFactory.GetSqliteScanDatabase(currentScan.ID).DataTableFromQueryString(Queries.Sqlite.SelectAssetDetailByAssetTag(assetTag)))
            {
                return results;
            }
        }

        private void CacheScanDetails(DataTable munisResults, string munisKeyColumn, DataTable assetResults, string assetKeyColumn, string scanId)
        {
            using (var trans = DBFactory.GetSqliteScanDatabase(scanId).StartTransaction())
            {
                // Add tables to Sqlite cache.
                SqliteFunctions.AddTableToScanDB(munisResults, munisKeyColumn, scanId, trans);
                SqliteFunctions.AddTableToScanDB(assetResults, assetKeyColumn, scanId, trans);
                CachePingHistory(trans);

                // Join the tables and add the result to the Sqlite cache.
                using (var cmd = DBFactory.GetSqliteScanDatabase(scanId).GetCommand(Queries.Sqlite.JoinAllAssetDetails()))
                using (var allDetails = DBFactory.GetSqliteScanDatabase(scanId).DataTableFromCommand(cmd, trans))
                {
                    allDetails.TableName = ItemDetailTable.TableName;
                    AddScanStatusColumns(allDetails);
                    SqliteFunctions.AddTableToScanDB(allDetails, MunisFixedAssetTable.Asset, scanId, trans);
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
                SqliteFunctions.AddTableToScanDB(results, PingHistoryTable.Id, currentScan.ID, trans);
            }
        }

        private void AddScanStatusColumns(DataTable detailsResults)
        {
            //  detailsResults.Columns.Add(ItemDetailTable.Scanned, typeof(bool));
            detailsResults.Columns.Add(ScanItemsTable.Location, typeof(string));
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

        private DataTable GetAssetManagerResults(DataTable munisResults)
        {
            var assetTable = new DataTable();
            var deviceCache = new Dictionary<string, DataRow>();

            // Cache result rows into a dictionary.
            using (var allAssets = DBFactory.GetMySqlDatabase().DataTableFromQueryString(Queries.Assets.SelectAllDevices()))
            {
                // Clone the table stucture.
                assetTable = allAssets.Clone();

                foreach (DataRow row in allAssets.Rows)
                {
                    if (!deviceCache.ContainsKey(row[DeviceTable.Serial].ToString()))
                    {
                        deviceCache.Add(row[DeviceTable.Serial].ToString(), row);
                    }
                }
            }

            // Iterate the munis rows and add the matches from the dictionary cache.
            foreach (DataRow row in munisResults.Rows)
            {
                if (deviceCache.ContainsKey(row[MunisFixedAssetTable.Serial].ToString().Trim()))
                {
                    assetTable.Rows.Add(deviceCache[row[MunisFixedAssetTable.Serial].ToString().Trim()].ItemArray);
                }
            }

            assetTable.TableName = DeviceTable.TableName;
            return assetTable;
        }

        public void SubmitNewScanItem(string assetTag, ScanType scanType)
        {
            if (currentScan == null)
            {
                scannerInput?.BadScan();
                OnExceptionOccured(new ScanNotStartedException());
                return;
            }

            using (var itemDetail = DetailOfAsset(assetTag))
            {
                if (itemDetail.Rows.Count < 1)
                {
                    scannerInput?.BadScan();
                    OnExceptionOccured(new ItemNotFoundException(assetTag));
                    return;
                }

                var itemRow = itemDetail.Rows[0];

                // Return silently if the item has already been scanned.
                if (!string.IsNullOrEmpty(itemRow[ScanItemsTable.ScanStatus].ToString()))
                {
                    scannerInput?.BadScan();
                    OnExceptionOccured(new DuplicateScanException());
                    return;
                }

                bool locationMismatch = false;

                // Check if the scan location matches the location in inventory.
                // Set the scan status and throw exception if there's a mismatch.
                if (itemRow[MunisFixedAssetTable.Location].ToString() != currentScan.MunisLocation.MunisCode)
                {
                    locationMismatch = true;
                    itemRow[ScanItemsTable.ScanStatus] = ScanStatus.LocationMismatch.ToString();
                }
                else
                {
                    locationMismatch = false;
                    itemRow[ScanItemsTable.ScanStatus] = ScanStatus.OK.ToString();
                }

                itemRow[ScanItemsTable.Location] = currentScan.MunisLocation.MunisCode;
                itemRow[ScanItemsTable.ScanType] = scanType.ToString();
                itemRow[ScanItemsTable.ScanUser] = currentScan.User;
                itemRow[ScanItemsTable.Datestamp] = DateTime.Now.ToString(DataConsistency.DBDateTimeFormat);
                itemRow[ScanItemsTable.ScanYear] = DateTime.Now.Year.ToString();
                itemRow[ScanItemsTable.ScanId] = currentScan.ID;

                var updatedRows = DBFactory.GetSqliteScanDatabase(currentScan.ID).UpdateTable(Queries.Sqlite.SelectAssetDetailByAssetTag(assetTag), itemDetail);

                LoadCurrentScanItems(view.LocationFilters);
                view.PopulateNewScan(assetTag, itemDetail);

                // Throw mismatch exception after adding the scan to the DB.
                if (locationMismatch)
                {
                    var expectedLocation = AttributeInstances.MunisAttributes.MunisToAssetLocations[itemRow[MunisFixedAssetTable.Location].ToString()];
                    var scanLocation = AttributeInstances.MunisAttributes.MunisToAssetLocations[currentScan.MunisLocation.MunisCode];

                    scannerInput?.BadScan();
                    OnExceptionOccured(new LocationMismatchException(expectedLocation.DisplayValue, scanLocation.DisplayValue, assetTag));
                    return;
                }

                scannerInput?.GoodScan();
            }
        }

        public async void SyncDataAsync()
        {
            if (syncRunning) return;

            try
            {
                syncRunning = true;
                var hasChanged = await Task.Run(() => { return TrySyncData(); });

                // Refresh view.
                if (hasChanged) LoadCurrentScanItems(view.LocationFilters);

            }
            catch (Exception)
            {
                // We want this to fail silently.
            }
            finally
            {
                syncRunning = false;
            }
        }

        private bool TrySyncData()
        {
            // TODO: Flatted and DRY this.
            bool hasChanged = false;
            var localQuery = Queries.Sqlite.SelectAllAssetDetails();
            var remoteQuery = Queries.Assets.SelectScanItemsByScanYear(currentScan.Datestamp.Year.ToString());

            using (var remoteTrans = DBFactory.GetMySqlDatabase().StartTransaction())
            using (var remoteResults = DBFactory.GetMySqlDatabase().DataTableFromQueryString(remoteQuery))
            using (var localTrans = DBFactory.GetSqliteScanDatabase(currentScan.ID).StartTransaction())
            using (var localResults = DBFactory.GetSqliteScanDatabase(currentScan.ID).DataTableFromQueryString(localQuery))
            {
                try
                {
                    foreach (DataRow localRow in localResults.Rows)
                    {
                        // Try to find a row on the remote results which cooresponds with tht current local row.
                        var remoteRow = remoteResults.AsEnumerable().Where(r => r[ScanItemsTable.AssetTag].ToString() == localRow[MunisFixedAssetTable.Asset].ToString()).SingleOrDefault();

                        // If a matching entry on remote exists.
                        if (remoteRow != null)
                        {
                            // If local entry DOES NOT HAVE a scan.
                            if (string.IsNullOrEmpty(localRow[ScanItemsTable.Datestamp].ToString()))
                            {
                                // Update local with remote.
                                hasChanged = true;

                                UpdateLocalFromRemote(localRow, remoteRow);
                            }
                            else
                            {
                                // If local HAS a scan and the locations match.
                                if (localRow[ScanItemsTable.Location].ToString() == remoteRow[ScanItemsTable.Location].ToString())
                                {
                                    // If the entry is from another scan.
                                    if (localRow[ScanItemsTable.ScanId].ToString() != remoteRow[ScanItemsTable.ScanId].ToString() || localRow[ScanItemsTable.ScanStatus].ToString() != remoteRow[ScanItemsTable.ScanStatus].ToString())
                                    {
                                        // Update local with remote.
                                        hasChanged = true;

                                        UpdateLocalFromRemote(localRow, remoteRow);
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
                                    else
                                    {
                                        // Update local with remote.
                                        hasChanged = true;

                                        UpdateLocalFromRemote(localRow, remoteRow);
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
                                newRow[ScanItemsTable.Location] = localRow[ScanItemsTable.Location];
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
                    DBFactory.GetSqliteScanDatabase(currentScan.ID).UpdateTable(localQuery, localResults, localTrans);
                    DBFactory.GetMySqlDatabase().UpdateTable(remoteQuery, remoteResults, remoteTrans);

                    localTrans.Commit();
                    remoteTrans.Commit();
                    GlobalSwitches.ServerOnline = true;
                    return hasChanged;
                }
                catch (Exception)
                {
                    localTrans.Rollback();
                    remoteTrans.Rollback();
                    GlobalSwitches.ServerOnline = false;
                    return false;
                }
            }
        }

        private void UpdateLocalFromRemote(DataRow localRow, DataRow remoteRow)
        {
            localRow[ScanItemsTable.Location] = remoteRow[ScanItemsTable.Location];
            localRow[ScanItemsTable.ScanType] = remoteRow[ScanItemsTable.ScanType];
            localRow[ScanItemsTable.ScanUser] = remoteRow[ScanItemsTable.ScanUser];
            localRow[ScanItemsTable.Datestamp] = remoteRow[ScanItemsTable.Datestamp];
            localRow[ScanItemsTable.ScanId] = remoteRow[ScanItemsTable.ScanId];
            localRow[ScanItemsTable.ScanStatus] = remoteRow[ScanItemsTable.ScanStatus];
        }

        private void AddDuplicateScanEntries(DataRow localRow, DataRow remoteRow, DbTransaction remoteTrans)
        {
            var emptyTableQuery = "SELECT * FROM " + ScanItemDuplicatesTable.TableName + " LIMIT 0";
            using (var dupTable = DBFactory.GetMySqlDatabase().DataTableFromQueryString(emptyTableQuery))
            {
                var localDupRow = dupTable.Rows.Add();
                localDupRow[ScanItemDuplicatesTable.AssetTag] = localRow[MunisFixedAssetTable.Asset];
                localDupRow[ScanItemDuplicatesTable.Serial] = localRow[MunisFixedAssetTable.Serial];
                localDupRow[ScanItemDuplicatesTable.Locaton] = localRow[ScanItemsTable.Location];
                localDupRow[ScanItemDuplicatesTable.ScanType] = localRow[ScanItemsTable.ScanType];
                localDupRow[ScanItemDuplicatesTable.ScanUser] = localRow[ScanItemsTable.ScanUser];
                localDupRow[ScanItemDuplicatesTable.Datestamp] = localRow[ScanItemsTable.Datestamp];
                localDupRow[ScanItemDuplicatesTable.ScanId] = localRow[ScanItemsTable.ScanId];
                localDupRow[ScanItemDuplicatesTable.ScanYear] = localRow[ScanItemsTable.ScanYear];

                var remoteDupRow = dupTable.Rows.Add();
                remoteDupRow[ScanItemDuplicatesTable.AssetTag] = remoteRow[ScanItemsTable.AssetTag];
                remoteDupRow[ScanItemDuplicatesTable.Serial] = remoteRow[ScanItemsTable.Serial];
                remoteDupRow[ScanItemDuplicatesTable.Locaton] = remoteRow[ScanItemsTable.Location];
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
            using (var results = DBFactory.GetSqliteCacheDatabase().DataTableFromQueryString(Queries.Munis.SelectDepartmentByLocation(locationCode)))
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

            AddScanInfoTable(newScan);

            return newScan;
        }

        private void AddScanInfoTable(Scan scan)
        {
            using (var scanTable = DBFactory.GetMySqlDatabase().DataTableFromQueryString(Queries.Assets.SelectScanById(scan.ID)))
            {
                scanTable.TableName = ScansTable.TableName;
                SqliteFunctions.AddTableToScanDB(scanTable, ScansTable.Id, scan.ID);
            }
        }

        public List<Scan> GetPreviousScansList()
        {
            var scanList = new List<Scan>();
            var scanFiles = Directory.GetFiles(Paths.SQLiteScanPath, "*.db").ToList();

            foreach (var scan in scanFiles)
            {
                var file = new FileInfo(scan);
                var scanIdString = file.Name.Substring(5, 4);
                var scanId = Convert.ToInt32(scanIdString);

                using (var scanDetail = DBFactory.GetSqliteScanDatabase(scanId.ToString()).DataTableFromQueryString("SELECT * FROM " + ScansTable.TableName))
                {
                    var row = scanDetail.Rows[0];
                    scanList.Add(new Scan(row[ScansTable.Id].ToString(), (DateTime)row[ScansTable.Datestamp], row[ScansTable.User].ToString(), new Location(row[ScansTable.Location].ToString())));
                }
            }

            return scanList;
        }

        public List<ScanItem> GetListOfScannedItems()
        {
            var itemList = new List<ScanItem>();

            using (var results = DBFactory.GetMySqlDatabase().DataTableFromQueryString(Queries.Assets.SelectCompletedScansByYear(currentScan.Datestamp.Year.ToString())))
            {
                foreach (DataRow row in results.Rows)
                {
                    itemList.Add(new ScanItem(row));
                }
            }

            return itemList;
        }

        public void Dispose()
        {
            syncTimer.Stop();
            syncTimer.Dispose();
            scannerInput?.Dispose();
        }
    }
}