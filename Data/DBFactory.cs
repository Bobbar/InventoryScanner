using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Data;

namespace InventoryScanner.Data
{
    class DBFactory
    {
        private static string serverIp = "10.10.0.89";
        
        public static IDatabase GetDatabase()
        {
            return new MySQLDatabase(serverIp, "asset_mgr_user", "A553tP455", "asset_manager");
        }
    }
}
