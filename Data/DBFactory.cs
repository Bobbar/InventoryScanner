using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Data;
using System.IO;

namespace InventoryScanner.Data
{
    class DBFactory
    {
        private static string serverIp = "10.10.0.89";
        // private static string sqlitePath = Directory.GetCurrentDirectory() + "dbcache.db";
        private static string sqlitePass = "";


        public static IDatabase GetMySqlDatabase()
        {
            return new MySqlDatabase(serverIp, "asset_mgr_usr", "A553tP455", "test_db");
        }

        public static IDatabase GetDatabase()
        {
            if (GlobalSwitches.ServerOnline)
            {
                Console.WriteLine("Got")
                return new MySqlDatabase(serverIp, "asset_mgr_usr", "A553tP455", "test_db");
            }
            else
            {
                return GetSqliteCacheDatabase();
            }
        }

        public static IDatabase GetSqliteScanDatabase(string scanId)
        {
            return new SqliteDatabase(SqlitePath(scanId), sqlitePass);
        }

        public static IDatabase GetSqliteCacheDatabase()
        {
            var cachePath = Directory.GetCurrentDirectory() + @"\cache.db";
            return new SqliteDatabase(cachePath, sqlitePass);
        }

        private static string SqlitePath(string scanId)
        {
            if (!Directory.Exists(Paths.SQLiteScanPath))
            {
                Directory.CreateDirectory(Paths.SQLiteScanPath);
            }

            var scanIdInt = Convert.ToInt32(scanId);

            // return Directory.GetCurrentDirectory() + @"\scan_" + scanIdInt.ToString("0000") + ".db";
            return Paths.SQLiteScanPath + "scan_" + scanIdInt.ToString("0000") + ".db";

        }
    }
}
