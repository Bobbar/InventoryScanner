using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.IO.Ports;

namespace InventoryScanner.BarcodeScanning
{
    public sealed class SerialPortReader : IDisposable, IScannerInput
    {
        private SerialPort port;

        public event EventHandler<string> NewScanReceived;

        private List<byte> byteBuffer = new List<byte>();

        // private Queue<string> receiveData = new Queue<string>();

        private System.Threading.CancellationTokenSource readCancelTokenSource;

        private Timer parseReadsTimer = new Timer();

        private void OnNewScanReceived(string data)
        {
            NewScanReceived?.Invoke(this, data);
        }

        public SerialPortReader(string portName)
        {
            port = new SerialPort(portName);
            //  port.DataReceived += new SerialDataReceivedEventHandler(Port_DataReceived);
            port.Open();
            port.DtrEnable = true;

            parseReadsTimer.Interval = 100;
            parseReadsTimer.Elapsed += ParseReadsTimer_Elapsed;
            parseReadsTimer.Start();

            readCancelTokenSource = new System.Threading.CancellationTokenSource();

            ReadBytesAsync(readCancelTokenSource.Token);
        }

        private void ParseReadsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ParseData();
        }

        private async void ReadBytesAsync(System.Threading.CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested && port.IsOpen)
            {
                port.BaseStream.ReadTimeout = 0;
                var bytesToRead = 16;
                var receiveBuffer = new byte[bytesToRead];
                var numBytesRead = await port.BaseStream.ReadAsync(receiveBuffer, 0, bytesToRead, cancelToken);

                var bytesReceived = new byte[numBytesRead];
                Array.Copy(receiveBuffer, bytesReceived, numBytesRead);

                byteBuffer.AddRange(bytesReceived);

                //receiveData.Enqueue(Encoding.Default.GetString(receiveBuffer));
               // ParseData();

            }


        }

        private void EnqueueData(byte[] data)
        {
            byteBuffer.AddRange(data);
        }

        private void ParseData()
        {
            // var dataArray = data.Split(Convert.ToChar("|"));

            // int currentPos = 0;

            // bool completePacket = false;



            int packetLength = GetNextPacketLength();

            if (packetLength > 0)
            {
                string packet = "";

                for (int i = 0; i < packetLength; i++)
                {
                    packet += Convert.ToChar(byteBuffer[0]).ToString();
                    byteBuffer.RemoveAt(0);

                }
                byteBuffer.RemoveAt(0);

                Console.WriteLine("Buffer Len: " + byteBuffer.Count);

                OnNewScanReceived(packet);
            }






        }

        private int GetNextPacketLength()
        {
            int packetLength = 0;

            foreach (var chunk in byteBuffer)
            {
                //  var byteString = Convert.ToString(chunk);

                if (chunk != 124)// if (byteString != "|")
                {
                    packetLength++;
                }
                else
                {
                    return packetLength;

                }
            }
            return -1;
        }



        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var data = port.ReadExisting();
            Console.WriteLine(data);
            OnNewScanReceived(data);
        }

        public void Dispose()
        {
            readCancelTokenSource.Cancel();
            parseReadsTimer.Stop();
            parseReadsTimer.Dispose();
            port.Close();
            port.Dispose();
           

        }
    }
}
