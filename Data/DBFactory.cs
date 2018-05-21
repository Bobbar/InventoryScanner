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

        public static IDatabase GetSqliteDatabase(string scanId)
        {
            return new SqliteDatabase(SqlitePath(scanId), sqlitePass);
        }

        private static string SqlitePath(string scanId)
        {
            return Directory.GetCurrentDirectory() + "scan" + scanId + "cache.db";
        }
    }
}
