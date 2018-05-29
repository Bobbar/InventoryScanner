using InventoryScanner.Data.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace InventoryScanner.UI
{
    public partial class SelectScanForm : Form
    {
        public Scan SelectedScan { get; private set; }

        public SelectScanForm()
        {
            InitializeComponent();
        }

        public void SetScanSelection(List<Scan> scans)
        {
            var selectionDict = new Dictionary<Scan, string>();

            foreach (var scan in scans)
            {
                selectionDict.Add(scan, scan.ID + " - " + scan.User + " - " + scan.ScanLocation + " - " + scan.Datestamp);
            }

            if (selectionDict.Count > 0)
            {
                ScansCombo.DataSource = new BindingSource(selectionDict, null);
                ScansCombo.DisplayMember = "Value";
                ScansCombo.ValueMember = "Key";
            }
            else
            {
                ScansCombo.Items.Add("No scans found.");
                ScansCombo.SelectedIndex = 0;
                ScansCombo.Enabled = false;
                AcceptScanButton.Enabled = false;
            }

        }

        private void AcceptScanButton_Click(object sender, EventArgs e)
        {
            SelectedScan = (Scan)ScansCombo.SelectedValue;
            this.DialogResult = DialogResult.OK;
        }
    }
}