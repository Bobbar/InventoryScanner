﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventoryScanner.Data.Functions;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AttributeFunctions.PopulateAttributeIndexes();
            var scanUI = new ScanningUI();
            var scanController = new ScanningController(scanUI);

            Application.Run(scanUI);
        }
    }
}
