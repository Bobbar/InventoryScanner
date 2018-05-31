using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace InventoryScanner.BarcodeScanning
{
    public class SerialPortReader : IDisposable
    {
        private SerialPort port;

        public event EventHandler<string> NewScanReceived;

        private void OnNewScanReceived(string data)
        {
            NewScanReceived?.Invoke(this, data);
        }

        public SerialPortReader(string portName)
        {
            port = new SerialPort(portName);
            port.DataReceived += new SerialDataReceivedEventHandler(Port_DataReceived);
            port.Open();
            port.DtrEnable = true;
           
        }
       
        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var data = port.ReadExisting();
            Console.WriteLine(data);
            OnNewScanReceived(data);
        }

        public void Dispose()
        {
            port.Close();
            port.Dispose();
        }
    }
}
