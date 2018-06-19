using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System.Windows.Forms;

namespace InventoryScanner.UI
{
    public partial class SelectScanner : Form
    {
        public string SelectedPortName { get; private set; }

        public SelectScanner()
        {
            InitializeComponent();
            ListPorts();
        }

        private void ListPorts()
        {
            // We want to get a list of the COM ports with descriptions.
            // The SerialPort class only provides the name/ID, so we'll
            // have to do a WMI query to get some decent descriptions.
            // We then compare the port names/IDs from the GetPortNames
            // method to the WMI results 'Name' value and look for names
            // that contain the port name/ID. We then grab the descriptive
            // name and add it to a container struct collection.

            ComPortListBox.DataSource = null;
            ComPortListBox.Items.Clear();

            // Get list of port names.
            var serialPorts = SerialPort.GetPortNames();

            // Collection for the container struct.
            var portList = new List<CommPortInfo>();


            try
            {
                // Query WMI for PnP ports.
                var mos = new ManagementObjectSearcher("Select * From Win32_PnpEntity WHERE PNPClass = 'Ports'");
                ManagementObjectCollection mocList = mos.Get();

                // Iterate the WMI results.
                foreach (var mo in mocList)
                {
                    // Also iterate the serial port names/IDs.
                    foreach (var port in serialPorts)
                    {
                        // If the WMI result 'Name' value contains the port name/ID,
                        // add the SerialPort name/ID and WMI descriptive Name to
                        // the collection.
                        if (mo["Name"].ToString().Contains(port))
                        {
                            portList.Add(new CommPortInfo(port, mo["Name"].ToString()));
                        }
                    }
                }
            }
            catch (System.Management.ManagementException ex)
            {
                // If an error occurs, just add the port IDs.
                foreach (var port in serialPorts)
                {
                    portList.Add(new CommPortInfo(port, port));
                }
            }
           

            // Set the value and display members to the container stuct properties
            // and set the datasource to the collection.
            ComPortListBox.ValueMember = nameof(CommPortInfo.ID);
            ComPortListBox.DisplayMember = nameof(CommPortInfo.Description);
            ComPortListBox.DataSource = portList;
        }

        private void SelectCurrentPort()
        {
            SelectedPortName = ((CommPortInfo)ComPortListBox.SelectedItem).ID;

            this.DialogResult = DialogResult.OK;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            SelectCurrentPort();
        }

        private void ComPortListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectCurrentPort();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            ListPorts();
        }

        private struct CommPortInfo
        {
            public string ID { get; private set; }
            public string Description { get; private set; }

            public CommPortInfo(string id, string description)
            {
                this.ID = id;
                this.Description = description;
            }
        }

    }
}