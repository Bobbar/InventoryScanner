using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryScanner.Data;
using InventoryScanner.Data.Functions;
using System.Data;
using System.Data.Common;
using InventoryScanner.Data.Munis;
using InventoryScanner.Data.Tables;

namespace InventoryScanner.Data.Functions
{
    public static class CacheFunctions
    {
        public static void CacheAssetTable(string tableName, string primaryKey, DbTransaction trans = null)
        {
            using (var results = DBFactory.GetMySqlDatabase().DataTableFromQueryString("SELECT * FROM " + tableName))
            {
                results.TableName = tableName;
                SqliteFunctions.AddTableToCacheDB(results, primaryKey, trans);
            }
        }

        public static void CacheMunisTable(string tableName, string primaryKey, DbTransaction trans = null)
        {
            using (var results = MunisDatabase.ReturnSqlTable("SELECT * FROM " + tableName))
            {
                results.TableName = tableName;
                SqliteFunctions.AddTableToCacheDB(results, primaryKey, trans);
            }
        }

        private static void CacheFATable(DbTransaction trans = null)
        {
            using (var results = MunisDatabase.ReturnSqlTable(Queries.Munis.SelectAllScanItems()))
            {
                results.TableName = MunisFixedAssetTable.TableName;
                SqliteFunctions.AddTableToCacheDB(results, MunisFixedAssetTable.Asset, trans);
            }
        }


        public static void CacheTables()
        {
            DropTables();

            using (var trans = DBFactory.GetSqliteCacheDatabase().StartTransaction())
            {
                try
                {
                    foreach (var table in TablesToCache())
                    {
                        if (table.TableType == TableType.AssetManager)
                        {
                            CacheAssetTable(table.TableName, table.PrimaryKey, trans);
                        }
                        else if (table.TableType == TableType.Munis)
                        {
                            CacheMunisTable(table.TableName, table.PrimaryKey, trans);
                        }
                    }

                    CacheFATable(trans);
                    
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                }
            }
        }

        private static void DropTables()
        {
            string query = "SELECT * FROM sqlite_master WHERE type='table'";

            using (var trans = DBFactory.GetSqliteCacheDatabase().StartTransaction())
            using (var conn = trans.Connection)
            using (var results = DBFactory.GetSqliteCacheDatabase().DataTableFromQueryString(query))
            {
                try
                {
                    foreach (DataRow row in results.Rows)
                    {
                        string dropQuery = "DROP TABLE " + row["name"];
                        DBFactory.GetSqliteCacheDatabase().ExecuteNonQuery(dropQuery, trans);
                    }
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                }
            }
        }


        private static List<CacheTable> TablesToCache()
        {
            var tables = new List<CacheTable>();
            tables.Add(new CacheTable("dev_codes", "id", TableType.AssetManager));
            tables.Add(new CacheTable("subnet_locations", "id", TableType.AssetManager));
            tables.Add(new CacheTable("munis_codes", "id", TableType.AssetManager));
            tables.Add(new CacheTable("munis_departments", "id", TableType.AssetManager));


            tables.Add(new CacheTable(MunisLocations.TableName, MunisLocations.Code, TableType.Munis));
           // tables.Add(new CacheTable(MunisFixedAssetTable.TableName, MunisFixedAssetTable.Asset, TableType.Munis));

            return tables;
        }

        private class CacheTable
        {
            public string TableName { get; private set; }
            public string PrimaryKey { get; private set; }
            public TableType TableType { get; private set; }

            public CacheTable(string tableName, string primaryKey, TableType tableType)
            {
                this.TableName = tableName;
                this.PrimaryKey = primaryKey;
                this.TableType = tableType;
            }

        }

        private enum TableType
        {
            AssetManager,
            Munis
        }
    }
}
