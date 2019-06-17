using InventoryScanner.Data.Classes;
using InventoryScanner.Data.Functions;
using InventoryScanner.Data.Tables;
using InventoryScanner.Helpers;
using InventoryScanner.Helpers.DataGridHelpers;
using InventoryScanner.PDFProcessing;
using InventoryScanner.ScanController;
using InventoryScanner.UI.CustomControls;
using InventoryScanner.UIManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DialogToast;

namespace InventoryScanner.UI
{
    public partial class ScanningUI : Form, IScanningUI
    {
        private ScanningController controller;
        private Location scanLocation;
        private DBControlParser controlParser;
        //private SliderLabel statusLabel = new SliderLabel();
        private Toast statusLabel;// = new Toast();

        public List<string> LocationFilters
        {
            get
            {
                return GetFilters();
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
            statusLabel = new Toast(ScanItemsGrid);
            //statusLabel.FlashStripOnNewMessage = true;
            //statusLabel.AutoSize = true;
            //statusStrip1.Items.Insert(0, statusLabel.ToToolStripControl(statusStrip1));

            this.Show();
        }

        public void SelectScannerPort()
        {
            var selectPort = new SelectScanner();
            if (selectPort.ShowDialog() == DialogResult.OK)
            {
                controller.InitScanner(selectPort.SelectedPortName);
            }
        }

        private void StatusMessage(string text, Color color)
        {
            statusLabel.Clear();
            statusLabel.QueueMessage(text, Color.Black, color, DialogToast.SlideDirection.Up, DialogToast.SlideDirection.Left, 4);
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

        private void PopulateLocationsCombo()
        {
            ScanLocationCombo.FillComboBox(AttributeInstances.DeviceAttributes.Locations);
        }

        private void InitDBControls()
        {
            MunisDescriptionTextBox.SetDBInfo(MunisFixedAssetTable.Description);
            MunisSerialTextBox.SetDBInfo(MunisFixedAssetTable.Serial);
            MunisLocationTextBox.SetDBInfo(MunisFixedAssetTable.Location, AttributeInstances.MunisAttributes.MunisLocations);
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

        /// <summary>
        /// Highlights and scrolls to the row containing the specified asset tag.
        /// </summary>
        /// <param name="assetTag"></param>
        private void SelectGridItem(string assetTag)
        {
            int itemRowIndex = -1;

            foreach (DataGridViewRow row in ScanItemsGrid.Rows)
            {
                var rowValue = row.Cells[MunisFixedAssetTable.Asset].Value;

                if (rowValue != null && rowValue.ToString() == assetTag)
                {
                    itemRowIndex = row.Index;
                }
            }

            if (itemRowIndex > 0)
            {
                ScanItemsGrid.ClearSelection();
                ScanItemsGrid.Rows[itemRowIndex].Selected = true;

                // If the item row index is outside the currenly displayed rows,
                // scroll the grid so that the selected row is in the middle of the window.

                var topRow = ScanItemsGrid.FirstDisplayedScrollingRowIndex;
                var displayedRows = ScanItemsGrid.DisplayedRowCount(true);
                var bottomRow = topRow + displayedRows;

                if (itemRowIndex > bottomRow || itemRowIndex < topRow)
                {
                    var scrollIndex = itemRowIndex - (int)(ScanItemsGrid.DisplayedRowCount(true) / 2);

                    if (scrollIndex > -1)
                    {
                        ScanItemsGrid.FirstDisplayedScrollingRowIndex = scrollIndex;
                    }
                    else
                    {
                        ScanItemsGrid.FirstDisplayedScrollingRowIndex = 0;
                    }
                }
            }
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

        private void DisplayDetailsOfAsset(string assetTag)
        {
            if (!string.IsNullOrEmpty(assetTag))
            {
                using (var detailData = controller.DetailOfAsset(assetTag))
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
        }

        public void StartScan()
        {
            controller.StartScan(scanLocation, DateTime.Now, ScanEmployeeTextBox.Text.Trim());
            LockControls();
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

        public void SubmitManualScan()
        {
            var enteredAssetTag = AssetTagTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(enteredAssetTag))
            {
                var prompt = OtherFunctions.Message("Submit new manual scan for Tag # " + enteredAssetTag + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, "Manual Scan", this);

                if (prompt == DialogResult.Yes)
                {
                    controller.SubmitNewScanItem(enteredAssetTag, ScanType.Manual);
                }
            }
        }

        #region Controller Events

        private void Controller_SuccessfulSync(object sender, bool e)
        {
            if (this.InvokeRequired)
            {
                var del = new Action(() => Controller_SuccessfulSync(sender, e));
                this.BeginInvoke(del);
            }
            else
            {
                if (e)
                {
                    SyncStatusLabel.ForeColor = Color.Black;
                    SyncStatusLabel.Text = "Last Sync: " + DateTime.Now.ToLongTimeString();
                }
                else
                {
                    SyncStatusLabel.ForeColor = Color.DarkRed;
                }
            }
        }

        private void Controller_ScannerStatusChanged(object sender, ScannerStatusEvent e)
        {
            if (this.InvokeRequired)
            {
                var del = new Action(() => Controller_ScannerStatusChanged(sender, e));
                this.BeginInvoke(del);
            }
            else
            {
                switch (e)
                {
                    case ScannerStatusEvent.Connected:
                        ScannerStatusLabel.Text = "Scanner: Connected";
                        ScannerStatusLabel.ForeColor = Color.DarkGreen;
                        break;

                    case ScannerStatusEvent.LostConnection:
                        ScannerStatusLabel.Text = "Scanner: Disconnected";
                        ScannerStatusLabel.ForeColor = Color.DarkRed;
                        break;

                    case ScannerStatusEvent.Error:
                        ScannerStatusLabel.Text = "Scanner: Error!";
                        ScannerStatusLabel.ForeColor = Color.DarkRed;
                        break;
                }
            }
        }

        private void Controller_ExceptionOccured(object sender, Exception e)
        {
            if (this.InvokeRequired)
            {
                var del = new Action(() => Controller_ExceptionOccured(sender, e));
                this.BeginInvoke(del);
            }
            else
            {
                if (e is LocationMismatchException)
                {
                    //var lme = (LocationMismatchException)e;

                    //var prompt = "Asset Tag: " + lme.ItemAssetTag +
                    //    " was scanned at an unexpected location. \n \n Expected location: " +
                    //    lme.ExpectedLocation + "\n Scan Location: " + lme.ScannedLocation;
                    //OtherFunctions.Message(prompt, MessageBoxButtons.OK, MessageBoxIcon.Warning, "Location Mismatch", this);
                    StatusMessage("Location Mismatch!", Color.FromArgb(255, 231, 76));
                }
                else if (e is ItemNotFoundException)
                {
                    StatusMessage("Asset not found!", Color.FromArgb(255,76,76));
                    var infe = (ItemNotFoundException)e;

                    var prompt = "Asset Tag: " + infe.AssetTag + " was not found in the list of scan items.";
                    OtherFunctions.Message(prompt, MessageBoxButtons.OK, MessageBoxIcon.Warning, "Asset Tag Not Found", this);
                }
                else if (e is BarcodeScanning.ScannerLostException)
                {
                    var prompt = "An error has occured with the scanner, it may have been disconnected. \n \n Please check the connection and select the scanner again.";
                    OtherFunctions.Message(prompt, MessageBoxButtons.OK, MessageBoxIcon.Warning, "Scanner Error", this);
                    SelectScannerPort();
                }
                else if (e is DuplicateScanException)
                {
                    StatusMessage("Duplicate scan!", Color.FromArgb(255, 76, 76));
                }
                else if (e is ScanNotStartedException)
                {
                    var prompt = "Please start a new scan or select a previous one before scanning.";
                    OtherFunctions.Message(prompt, MessageBoxButtons.OK, MessageBoxIcon.Warning, "Scan Not Started", this);
                }
            }
        }

        #endregion Controller Events

        #region IScanningUI

        public void SetController(ScanningController controller)
        {
            this.controller = controller;
            this.controller.ExceptionOccured += Controller_ExceptionOccured;
            this.controller.ScannerStatusChanged += Controller_ScannerStatusChanged;
            this.controller.SyncEvent += Controller_SuccessfulSync;
            SelectScannerPort();
        }

        public void PopulateNewScan(string assetTag, DataTable itemDetail)
        {
            if (this.InvokeRequired)
            {
                var del = new Action(() => PopulateNewScan(assetTag, itemDetail));
                this.BeginInvoke(del);
            }
            else
            {
                PopulateControls(itemDetail);
                SelectGridItem(assetTag);
            }
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

                // Bold tag and serial columns.
                ScanItemsGrid.Columns[MunisFixedAssetTable.Asset].DefaultCellStyle.Font = new Font(ScanItemsGrid.Font, FontStyle.Bold);
                ScanItemsGrid.Columns[MunisFixedAssetTable.Asset].HeaderCell.Style.Font = new Font(ScanItemsGrid.Font, FontStyle.Bold);

                ScanItemsGrid.Columns[MunisFixedAssetTable.Serial].DefaultCellStyle.Font = new Font(ScanItemsGrid.Font, FontStyle.Bold);
                ScanItemsGrid.Columns[MunisFixedAssetTable.Serial].HeaderCell.Style.Font = new Font(ScanItemsGrid.Font, FontStyle.Bold);

                ScanItemsGrid.FastAutoSizeColumns();

                gridState.RestoreState();

                SetRowColors();

                ScanItemsGrid.ResumeLayout();

                DisplayDetailsOfSelected();
            }
        }

        #endregion IScanningUI

        #region Control Events

        private void ScanLocationCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ScanLocationCombo.SelectedIndex > -1 && controller != null)
            {
                scanLocation = controller.GetLocation(ScanLocationCombo.SelectedValue.ToString());
            }
        }

        private void StartScanButton_Click(object sender, EventArgs e)
        {
            StartScan();
        }

        private void ScanItemsGrid_SelectionChanged(object sender, EventArgs e)
        {
            // DisplayDetailsOfSelected();
        }

        private void SubmitScanButton_Click(object sender, EventArgs e)
        {
            SubmitManualScan();
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

        private void ScanItemsGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DisplayDetailsOfSelected();
        }

        private void ScanItemsGrid_KeyUp(object sender, KeyEventArgs e)
        {
            DisplayDetailsOfSelected();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            controlParser.ClearFields();
        }

        private void selectPreviousScanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectPreviousScan();
        }

        private void selectScannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectScannerPort();
        }

        private void rebuildCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CacheFunctions.CacheTables();
        }

        private void DropDown_MouseLeave(object sender, EventArgs e)
        {
            filtersToolStripMenuItem.DropDown.Close();
        }

        #endregion Control Events

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                controller.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}