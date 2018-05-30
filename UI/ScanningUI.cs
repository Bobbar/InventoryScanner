using InventoryScanner.Data.Classes;
using InventoryScanner.Data.Tables;
using InventoryScanner.Helpers;
using InventoryScanner.Helpers.DataGridHelpers;
using InventoryScanner.PDFProcessing;
using InventoryScanner.UIManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace InventoryScanner.UI
{
    public partial class ScanningUI : Form, IScanning
    {
        private ScanningController controller;
        private Location location;
        private DBControlParser controlParser;

        public List<string> LocationFilters
        {
            get
            {
                return GetFilters();
            }
        }

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
            AttachFilterMenuEvents();
            // this.Show();
        }

        public void SetController(ScanningController controller)
        {
            this.controller = controller;
        }

        private void ScanningUI_Load(object sender, EventArgs e)
        {
            //controller.GetPreviousScansList();
        }

        private void AttachFilterMenuEvents()
        {
            FilterAllMenuItem.CheckedChanged += FilterItem_CheckedChanged;
            FilterAdminMenuItem.CheckedChanged += FilterItem_CheckedChanged;
            FilterFRSMenuItem.CheckedChanged += FilterItem_CheckedChanged;
            FilterPROMenuItem.CheckedChanged += FilterItem_CheckedChanged;
            FilterOCMenuItem.CheckedChanged += FilterItem_CheckedChanged;
            FilterACMenuItem.CheckedChanged += FilterItem_CheckedChanged;
            FilterDUMenuItem.CheckedChanged += FilterItem_CheckedChanged;
            filtersToolStripMenuItem.DropDown.AutoClose = false;
            filtersToolStripMenuItem.DropDown.MouseLeave += DropDown_MouseLeave;
        }

        private void DropDown_MouseLeave(object sender, EventArgs e)
        {
            filtersToolStripMenuItem.DropDown.Close();
        }

        private void PopulateLocationsCombo()
        {
            ScanLocationCombo.FillComboBox(AttributeInstances.DeviceAttributes.Locations);
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
            AssetLastLocationTextBox.SetDBInfo(PingHistoryTable.IPAddress, AttributeInstances.DeviceAttributes.SubnetLocation);

            AssetTagTextBox.SetDBInfo(MunisFixedAssetTable.Asset);
        }

        private void DisplayDetailsOfSelected()
        {
            var selectedAssetTag = ScanItemsGrid.CurrentRowStringValue(MunisFixedAssetTable.Asset);

            if (!string.IsNullOrEmpty(selectedAssetTag))
            {
                using (var detailData = controller.DetailOfAsset(selectedAssetTag))
                {
                    PopulateControls(detailData);
                }
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
            columnList.Add(new GridColumnAttrib(MunisFixedAssetTable.Location, "Munis Location", AttributeInstances.MunisAttributes.MunisToAssetLocations, ColumnFormatType.AttributeDisplayMemberOnly));
            columnList.Add(new GridColumnAttrib(MunisFixedAssetTable.Department, "Munis Department"));
            columnList.Add(new GridColumnAttrib(MunisFixedAssetTable.Description, "Munis Description"));
            //   columnList.Add(new GridColumnAttrib(DeviceTable.Description, "Asset Description"));
            //  columnList.Add(new GridColumnAttrib(DeviceTable.Location, "Asset Location", AttributeInstances.DeviceAttributes.Locations, ColumnFormatType.AttributeDisplayMemberOnly));
            columnList.Add(new GridColumnAttrib(DeviceTable.DeviceType, "Device Type", AttributeInstances.DeviceAttributes.EquipType, ColumnFormatType.AttributeDisplayMemberOnly));
            columnList.Add(new GridColumnAttrib(DeviceTable.CurrentUser, "Current User"));
            //      columnList.Add(new GridColumnAttrib(DeviceTable.Status, "Status", AttributeInstances.DeviceAttributes.StatusType, ColumnFormatType.AttributeDisplayMemberOnly));
            columnList.Add(new GridColumnAttrib(ScanItemsTable.Location, "Scan Location", AttributeInstances.MunisAttributes.MunisToAssetLocations, ColumnFormatType.AttributeDisplayMemberOnly));
            columnList.Add(new GridColumnAttrib(ScanItemsTable.Datestamp, "Scan Datestamp"));
            columnList.Add(new GridColumnAttrib(ScanItemsTable.ScanUser, "Scan User"));
            columnList.Add(new GridColumnAttrib(ScanItemsTable.ScanType, "Scan Type"));
            columnList.Add(new GridColumnAttrib(ScanItemsTable.ScanStatus, "Scan Status"));

            //columnList.Add(new GridColumnAttrib(ItemDetailTable.Scanned, "Scanned"));
            //columnList.Add(new GridColumnAttrib(ItemDetailTable.ScanLocation, "Scan Location"));
            //columnList.Add(new GridColumnAttrib(ItemDetailTable.ScanDate, "Scan Time"));
            //columnList.Add(new GridColumnAttrib(ItemDetailTable.ScanUser, "Scan User"));
            //columnList.Add(new GridColumnAttrib(ItemDetailTable.ScanType, "Scan Type"));

            return columnList;
        }

        public void LockControls()
        {
            ScanLocationCombo.Enabled = false;
            ScanDateTimeTextBox.Enabled = false;
            ScanEmployeeTextBox.Enabled = false;
            StartScanButton.Enabled = false;
            SelectPreviousScanButton.Enabled = false;
        }

        public void StartScan()
        {
            controller.StartScan(ScanLocation, DateTime.Now, ScanEmployeeTextBox.Text.Trim());
            LockControls();
        }

        public void LoadScanItems(DataTable data)
        {
            if (ScanItemsGrid.InvokeRequired)
            {
                var del = new Action(() => LoadScanItems(data));
                ScanItemsGrid.BeginInvoke(del);
            }
            else
            {
                ScanItemsGrid.SuspendLayout();

                var gridState = new GridState(ScanItemsGrid);

                ScanItemsGrid.Populate(data, ScanItemsGridColumns());
                ScanItemsGrid.FastAutoSizeColumns();

                gridState.RestoreState();

                SetRowColors();

                ScanItemsGrid.ResumeLayout();

                DisplayDetailsOfSelected();
            }
        }

        private void SetRowColors()
        {
            foreach (DataGridViewRow row in ScanItemsGrid.Rows)
            {
                var statusCell = row.Cells[ScanItemsTable.ScanStatus];

                if (statusCell.Value != null)
                {
                    var backColor = row.DefaultCellStyle.BackColor;
                    var foreColor = row.DefaultCellStyle.ForeColor;
                    var selectColor = row.DefaultCellStyle.SelectionBackColor;
                    var selectForeColor = row.DefaultCellStyle.SelectionForeColor;

                    if (statusCell.Value.ToString() == ScanStatus.OK.ToString())
                    {
                        backColor = Color.DarkGreen;
                        selectColor = Color.LightGreen;
                        selectForeColor = Color.Black;
                    }
                    else if (statusCell.Value.ToString() == ScanStatus.Duplicate.ToString())
                    {
                        backColor = Color.DarkOrange;
                        selectColor = Color.Orange;
                        selectForeColor = Color.Black;
                    }
                    else if (statusCell.Value.ToString() == ScanStatus.Error.ToString())
                    {
                        backColor = Color.DarkRed;
                    }
                    else if (statusCell.Value.ToString() == ScanStatus.LocationMismatch.ToString())
                    {
                        backColor = Color.Gold;
                        foreColor = Color.Black;
                        selectColor = Color.Yellow;
                        selectForeColor = Color.Black;
                    }

                    row.DefaultCellStyle.BackColor = backColor;
                    row.DefaultCellStyle.ForeColor = foreColor;
                    row.DefaultCellStyle.SelectionBackColor = selectColor;
                    row.DefaultCellStyle.SelectionForeColor = selectForeColor;
                }
            }
        }

        private void SetupContextMenu()
        {
            var selectedStatus = ScanItemsGrid.CurrentRowStringValue(ScanItemsTable.ScanStatus);

            if (string.IsNullOrEmpty(selectedStatus))
            {
                ResolveDupMenuItem.Enabled = false;
            }
            else
            {
                if (selectedStatus == ScanStatus.Duplicate.ToString())
                {
                    ResolveDupMenuItem.Enabled = true;
                }
                else
                {
                    ResolveDupMenuItem.Enabled = false;
                }
            }
        }

        private void SelectPreviousScan()
        {
            var scanList = controller.GetPreviousScansList();

            var scanSelector = new SelectScanForm();
            scanSelector.SetScanSelection(scanList);
            scanSelector.ShowDialog();

            if (scanSelector.DialogResult == DialogResult.OK)
            {
                var selectedScan = scanSelector.SelectedScan;
                var location = controller.GetLocation(selectedScan.ScanLocation);
                selectedScan.MunisLocation = location;
                ScanLocationCombo.SetSelectedAttributeByValue(selectedScan.ScanLocation);
                ScanDateTimeTextBox.Text = selectedScan.Datestamp.ToString();
                ScanEmployeeTextBox.Text = selectedScan.User;
                controller.StartScan(selectedScan);
                LockControls();
            }
        }

        private void ProcessWorksheet()
        {
            controller.PauseSync();

            var processor = new WorksheetProcessor();
            var itemList = controller.GetListOfScannedItems();
            var workSheetPath = processor.FillWorksheet(itemList);

            controller.ResumeSync();

            System.Diagnostics.Process.Start(workSheetPath);
        }

        public void UpdateScanItem(string serial, ScanType scanType)
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

        private List<string> GetFilters()
        {
            var filterList = new List<string>();

            foreach (ToolStripMenuItem filter in filtersToolStripMenuItem.DropDownItems)
            {
                if (filter.Tag.ToString() == "All" && filter.Checked) return null;

                if (filter.Tag.ToString() != "All" && filter.Checked)
                {
                    filterList.Add(filter.Tag.ToString());
                }
            }

            return filterList;
        }

        private void SetAllFilterMenuItems(bool checkedValue)
        {
            foreach (ToolStripMenuItem filter in filtersToolStripMenuItem.DropDownItems)
            {
                if (filter.Tag.ToString() != "All")
                {
                    filter.Checked = checkedValue;
                }
            }
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
            StartScan();
        }

        private void ScanItemsGrid_SelectionChanged(object sender, EventArgs e)
        {
            DisplayDetailsOfSelected();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            controller.SyncDataAsync();
        }

        private void SubmitScanButton_Click(object sender, EventArgs e)
        {
            var selectedAssetTag = ScanItemsGrid.CurrentRowStringValue(MunisFixedAssetTable.Asset);

            if (!string.IsNullOrEmpty(selectedAssetTag))
            {
                try
                {
                    controller.SubmitNewScanItem(selectedAssetTag, ScanType.Scanned);
                }
                catch (LocationMismatchException lme)
                {
                    var prompt = "Asset Tag: " + lme.ItemAssetTag +
                        " was scanned at an unexpected location. \n \n Expected location: " +
                        lme.ExpectedLocation + "\n Scan Location: " + lme.ScannedLocation;
                    OtherFunctions.Message(prompt, MessageBoxButtons.OK, MessageBoxIcon.Warning, "Location Mismatch", this);
                }
            }
        }

        private void ScanItemsGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 & e.RowIndex >= 0)
            {
                if (e.Button == MouseButtons.Right & !ScanItemsGrid[e.ColumnIndex, e.RowIndex].Selected)
                {
                    ScanItemsGrid.Rows[e.RowIndex].Selected = true;
                    ScanItemsGrid.CurrentCell = ScanItemsGrid[e.ColumnIndex, e.RowIndex];
                }
            }
            SetupContextMenu();
        }

        private void FilterItem_CheckedChanged(object sender, EventArgs e)
        {
            var senderMenuItem = (ToolStripMenuItem)sender;

            if (senderMenuItem.Tag.ToString() == "All")
            {
                if (senderMenuItem.Checked)
                {
                    SetAllFilterMenuItems(false);
                }
            }
            else
            {
                if (senderMenuItem.Checked)
                {
                    FilterAllMenuItem.Checked = false;
                }
            }

            controller.RefreshCurrentItems(GetFilters());
        }

        private void SelectPreviousScanButton_Click(object sender, EventArgs e)
        {
            SelectPreviousScan();
        }

        private void processWorksheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessWorksheet();
        }

        private void ScanItemsGrid_Sorted(object sender, EventArgs e)
        {
            SetRowColors();
        }
    }
}