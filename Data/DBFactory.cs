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
        private static string sqlitePass = "";


        public static IDatabase GetMySqlDatabase()
        {
            return new MySqlDatabase(serverIp, "asset_mgr_usr", "A553tP455", "test_db");
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

            return Paths.SQLiteScanPath + "scan_" + scanIdInt.ToString("0000") + ".db";

        }
    }
}
