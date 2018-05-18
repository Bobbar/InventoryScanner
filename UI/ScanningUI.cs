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
using InventoryScanner.Helpers;

namespace InventoryScanner
{
    public partial class ScanningUI : Form, IScanning
    {
        private ScanningController controller;
        private Location location;
        private DBControlParser controlParser;

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
            InitDBControls();
            controlParser = new DBControlParser(this);
            ScanItemsGrid.DoubleBuffered(true);
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

        private void DisplayDetails(string serial)
        {
            using (var detailData = controller.DetailOfAsset(serial))
            {
                PopulateControls(detailData);
            }

        }

        private void PopulateControls(DataTable data)
        {
            controlParser.FillDBFields(data);
        }

        private List<GridColumnAttrib> ScanItemsGridColumns()
        {
            var columnList = new List<GridColumnAttrib>();
            columnList.Add(new GridColumnAttrib(MunisFixedAssetTable.Asset, "Asset #"));
            columnList.Add(new GridColumnAttrib(MunisFixedAssetTable.Serial, "Serial"));
            columnList.Add(new GridColumnAttrib(MunisFixedAssetTable.Location, "Munis Location", AttributeInstances.MunisAttributes.Locations, ColumnFormatType.AttributeDisplayMemberOnly));
            columnList.Add(new GridColumnAttrib(MunisFixedAssetTable.Department, "Munis Department"));
            columnList.Add(new GridColumnAttrib(MunisFixedAssetTable.Description, "Munis Description"));
            columnList.Add(new GridColumnAttrib(DeviceTable.Description, "Asset Description"));
            columnList.Add(new GridColumnAttrib(DeviceTable.Location, "Asset Location", AttributeInstances.DeviceAttributes.Locations, ColumnFormatType.AttributeDisplayMemberOnly));
            columnList.Add(new GridColumnAttrib(DeviceTable.CurrentUser, "Current User"));
            columnList.Add(new GridColumnAttrib(DeviceTable.DeviceType, "Device Type", AttributeInstances.DeviceAttributes.EquipType, ColumnFormatType.AttributeDisplayMemberOnly));
            columnList.Add(new GridColumnAttrib(DeviceTable.Status, "Status", AttributeInstances.DeviceAttributes.StatusType, ColumnFormatType.AttributeDisplayMemberOnly));

            return columnList;
        }

        public void LoadScanItems(DataTable data)
        {
            ScanItemsGrid.Populate(data, ScanItemsGridColumns());
            ScanItemsGrid.FastAutoSizeColumns();
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

        private void StartScanButton_Click(object sender, EventArgs e)
        {
            controller.StartScan(ScanLocation, DateTime.Now, ScanEmployeeTextBox.Text.Trim());
        }

        private void ScanItemsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedSerial = ScanItemsGrid.CurrentRowStringValue(MunisFixedAssetTable.Serial);

            if (!string.IsNullOrEmpty(selectedSerial))
            {
                DisplayDetails(selectedSerial);
            }
        }
    }
}
