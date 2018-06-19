using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventoryScanner.Data.Functions;
using InventoryScanner.UI;
using InventoryScanner.ScanController;
using InventoryScanner.Helpers;
using InventoryScanner.Data;

namespace InventoryScanner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logging.Logger("Starting application...");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;

            try
            {
                // GlobalSwitches.ServerOnline = CanReachServer();

                Logging.Logger("Loading attributes...");
                AttributeFunctions.PopulateAttributeIndexes();

                //Logging.Logger("Caching tables...");
                // if (GlobalSwitches.ServerOnline) CacheFunctions.CacheTables();

                Logging.Logger("Init Scanning UI...");
                var scanUI = new ScanningUI();

                Logging.Logger("Init Scanning Controller...");
                var scanController = new ScanningController(scanUI);

                Logging.Logger("Launch UI...");
                Application.Run(scanUI);
            }
            catch (Exception ex)
            {
                Logging.Logger("ERROR: " + ex.ToString());

                Console.WriteLine(ex.ToString());
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("connect"))
                    {
                        OtherFunctions.Message("Cannot connect to server.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, "No Connection");
                        Application.Exit();
                    }
                }
                else
                {
                    OtherFunctions.Message(ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Stop, "Error");
                }

            }

        }

        private static bool CanReachServer()
        {
            try
            {
                using (var conn = DBFactory.GetMySqlDatabase().NewConnection())
                {
                    return DBFactory.GetMySqlDatabase().OpenConnection(conn, true);
                }
            }
            catch
            {
                return false;
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Logging.Logger("UNHANDLED EX: " + e.Exception.ToString());
            Console.WriteLine("UNHANDLED EX: " + e.Exception.ToString());
        }
    }
}
