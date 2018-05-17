using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventoryScanner.UIManagement;
using InventoryScanner.Data;
using InventoryScanner.Data.Classes;
using InventoryScanner.Data.Tables;
using InventoryScanner.Helpers.DataGridHelpers;

namespace InventoryScanner
{
    public partial class ScanningUI : Form, IScanning
    {
        private ScanningController controller;
        private Location location;

        public Location ScanLocation
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }

        public ScanningUI()
        {
            InitializeComponent();
            PopulateLocationsCombo();
            ScanDateTimeTextBox.Text = DateTime.Now.ToString();
           // this.Show();
        }

        public void SetController(ScanningController controller)
        {
            this.controller = controller;
        }


        private void ScanningUI_Load(object sender, EventArgs e)
        {

        }

        private void PopulateLocationsCombo()
        {
            ScanLocationCombo.FillComboBox(AttributeInstances.DeviceAttributes.Locations);

            //var locations = new Dictionary<int, string>
            //{
            //    { 0, "" },
            //    { 750, "Admin" },
            //    { 757, "Art & Clay" },
            //    { 759, "Discover U" },
            //    { 758, "Pickerington Regional Office" },
            //    { 754, "Opportunity Center" },
            //    { 753, "Forest Rose School" }
            //};

            //ScanLocationCombo.DataSource = new BindingSource(locations, null);
            //ScanLocationCombo.DisplayMember = "Value";
            //ScanLocationCombo.ValueMember = "Key";
        }

        private void InitDBControls()
        {
            MunisDescriptionTextBox.SetDBInfo(MunisFixedAssetTable.Description);
            MunisSerialTextBox.SetDBInfo(MunisFixedAssetTable.Serial);
            MunisLocationTextBox.SetDBInfo(MunisFixedAssetTable.Location);
            MunisDepartmentTextBox.SetDBInfo(MunisFixedAssetTable.Department);
            MunisPurchaseDtTextBox.SetDBInfo(MunisFixedAssetTable.PurchaseDate);

            AssetDescriptionTextBox.SetDBInfo(DeviceTable.Description);
            AssetSerialTextBox.SetDBInfo(DeviceTable.Serial);
            AssetLocationTextBox.SetDBInfo(DeviceTable.Location, AttributeInstances.DeviceAttributes.Locations);
            AssetTypeTextBox.SetDBInfo(DeviceTable.DeviceType, AttributeInstances.DeviceAttributes.EquipType);
            AssetCurUserTextBox.SetDBInfo(DeviceTable.CurrentUser);
            AssetStatusTextBox.SetDBInfo(DeviceTable.Status, AttributeInstances.DeviceAttributes.StatusType);



        }

        private List<GridColumnAttrib> ScanItemsGridColumns()
        {
            var columnList = new List<GridColumnAttrib>();
            columnList.Add(new GridColumnAttrib(MunisFixedAssetTable.Asset, "Asset #"));
            columnList.Add(new GridColumnAttrib(MunisFixedAssetTable.Serial, "Serial"));
            columnList.Add(new GridColumnAttrib(MunisFixedAssetTable.Description, "Description"));
            return columnList;
        }

        public void LoadScanItems(DataTable data)
        {
            
        }

        public void LockScanInfoUI()
        {
            throw new NotImplementedException();
        }

        public void SetScanInfo(Scan scan)
        {
            throw new NotImplementedException();
        }

        private void ScanLocationCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ScanLocationCombo.SelectedIndex > -1 && controller != null)
            {
                ScanLocation = controller.GetLocation(ScanLocationCombo.SelectedValue.ToString());
            }
        }
    }
}
