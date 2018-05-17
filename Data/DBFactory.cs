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
        private static string sqlitePath = Directory.GetCurrentDirectory() + "dbcache.db";
        private static string sqlitePass = "scancachepass";


        public static IDatabase GetMySqlDatabase()
        {
            return new MySQLDatabase(serverIp, "asset_mgr_usr", "A553tP455", "asset_manager");
        }

        public static IDatabase GetSqliteDatabase()
        {
            return new SqliteDatabase(sqlitePath, sqlitePass);
        }
    }
}
