using InventoryScanner.Data;
using InventoryScanner.Data.Functions;
using InventoryScanner.Data.Munis;
using InventoryScanner.Data.Tables;
using System;
using System.Data;
using System.Linq;

namespace InventoryScanner
{
    public class ScanningController
    {
        private IScanning view;
        private string scanId;

        public ScanningController(IScanning view)
        {
            this.view = view;
            view.SetController(this);
        }

        public async void StartScan(Location location, DateTime datestamp, string scanEmployee)
        {
            scanId = InsertNewScan(location, datestamp, scanEmployee).ToString();

            var scanItemsQuery = Queries.Munis.SelectScanItemsByDepartment(location.DepartmentCode);

            using (var munisResults = await MunisDatabase.ReturnSqlTableAsync(scanItemsQuery))
            using (var assetResults = GetAssetManagerResults(munisResults))
            {
                view.LoadScanItems(JoinResults(munisResults, assetResults));

                munisResults.TableName = MunisFixedAssetTable.TableName;
                SqliteFunctions.AddTableToDB(munisResults, MunisFixedAssetTable.Asset, scanId);
                SqliteFunctions.AddTableToDB(assetResults, DeviceTable.Id, scanId);
            }

            CachePingHistory();
        }

        public DataTable DetailOfAsset(string serial)
        {
            using (var results = DBFactory.GetSqliteDatabase(scanId).DataTableFromQueryString(Queries.Sqlite.SelectAssetDetailBySerial(serial)))
            {
                return results;
            }
        }

        private void CachePingHistory()
        {
            using (var results = DBFactory.GetMySqlDatabase().DataTableFromQueryString(Queries.Assets.SelectMaxPingHistory()))
            {
                results.TableName = "device_ping_history";
                SqliteFunctions.AddTableToDB(results, "id", scanId);
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

        public Location GetLocation(string locationCode)
        {
            using (var results = DBFactory.GetMySqlDatabase().DataTableFromQueryString(Queries.Munis.SelectDepartmentByLocation(locationCode)))
            {
                return new Location(results);
            }
        }

        private int InsertNewScan(Location location, DateTime datestamp, string scanEmployee)
        {
            var newScan = new Scan(datestamp, scanEmployee, location);
            MapObjectFunctions.InsertMapObject(newScan);

            var scanId = (int)DBFactory.GetMySqlDatabase().ExecuteScalarFromQueryString("SELECT MAX(" + ScansTable.Id + ") FROM " + ScansTable.TableName);

            return scanId;
        }
    }
}