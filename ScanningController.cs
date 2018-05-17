using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryScanner.Data;
using InventoryScanner.Data.Munis;
using InventoryScanner.Data.Functions;
using InventoryScanner.Data.Tables;
using System.Data;
using Database.Data;

namespace InventoryScanner
{
    public class ScanningController
    {
        private IScanning view;

        public ScanningController(IScanning view)
        {
            this.view = view;
            view.SetController(this);

           
        }


        public async void StartScan(Location location, DateTime datestamp, string scanEmployee)
        {
            InsertNewScan(location, datestamp, scanEmployee);

            var scanItemsQuery = Queries.SelectScanItemsByDepartment(location.DepartmentCode);

            using (var results = await MunisDatabase.ReturnSqlTableAsync(scanItemsQuery))
            {
                view.LoadScanItems(results);
            }
           
        }

        public Location GetLocation(string locationCode)
        {
            using (var results = DBFactory.GetMySqlDatabase().DataTableFromQueryString(Queries.SelectDepartmentByLocation(locationCode)))
            {
                return new Location(results);
            }
        }

        private void InsertNewScan(Location location, DateTime datestamp, string scanEmployee)
        {
            var newScan = new Scan(datestamp, scanEmployee, location);
            MapObjectFunctions.InsertMapObject(newScan);
        }
    }
}
