using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Timers;

namespace InventoryScanner.BarcodeScanning
{
    class SerialPortReader : IDisposable, IScannerInput
    {
        private SerialPort port;
        private List<byte> byteBuffer = new List<byte>();
        private System.Threading.CancellationTokenSource readCancelTokenSource;

        public event EventHandler<string> NewScanReceived;
        public event EventHandler<Exception> ExceptionOccured;

        public bool SupportsFeedback
        {
            get
            {
                return true;
            }
        }

        private void OnNewScanReceived(string data)
        {
            NewScanReceived?.Invoke(this, data);

            Console.WriteLine("Scan: " + data);
        }

        private void OnExceptionOccured(Exception ex)
        {
            ExceptionOccured?.Invoke(this, ex);
        }

        public SerialPortReader(string portName)
        {
            port = new SerialPort(portName);
        }

        public bool StartScanner()
        {
            // Try to open the port.
            try
            {
                port.Open();
                port.DtrEnable = true;
                port.PinChanged += Port_PinChanged;
                port.ErrorReceived += Port_ErrorReceived;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Start the async reader.
            readCancelTokenSource = new System.Threading.CancellationTokenSource();
            ReadBytesAsync(readCancelTokenSource.Token);

            return true;
        }

        public void GoodScan()
        {
            if (!SupportsFeedback) return;

            // Send ACK character.
            var ack = new char[1];
            ack[0] = System.Convert.ToChar(0x06);
            port.Write(ack, 0, 1);

            Console.WriteLine("Good Scan.");
        }

        public void BadScan()
        {
            if (!SupportsFeedback) return;

            // Send NAK character.
            var ack = new char[1];
            ack[0] = System.Convert.ToChar(0x15);
            port.Write(ack, 0, 1);

            Console.WriteLine("Bad Scan.");

        }
        private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine("Error: " + e.EventType.ToString());
        }

        private void Port_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            Console.WriteLine("Pin Event: " + e.EventType.ToString());
        }

        private void ParseReadsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ParseData();
        }

        private async void ReadBytesAsync(System.Threading.CancellationToken cancelToken)
        {
            try
            {
                while (!cancelToken.IsCancellationRequested && port.IsOpen)
                {
                    port.BaseStream.ReadTimeout = 0;
                    var bytesToRead = 64;
                    var receiveBuffer = new byte[bytesToRead];
                    var numBytesRead = await port.BaseStream.ReadAsync(receiveBuffer, 0, bytesToRead, cancelToken);
                    var bytesReceived = new byte[numBytesRead];

                    Array.Copy(receiveBuffer, bytesReceived, numBytesRead);

                    // Add the data to a buffer to be parsed when possible.
                    byteBuffer.AddRange(bytesReceived);

                    ParseData();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                if (ex is System.IO.IOException)
                {
                    if (!disposedValue)
                        OnExceptionOccured(new ScannerLostException());
                }
            }
        }

        private void ParseData()
        {
            int packetLength = GetNextPacketLength();

            // Make sure we have a complete packet.
            if (packetLength > 0)
            {
                string packet = "";

                // Build the packet string from the byte data, removing the bytes from the buffer as we go.
                for (int i = 0; i < packetLength; i++)
                {
                    packet += Convert.ToChar(byteBuffer[0]).ToString();
                    byteBuffer.RemoveAt(0);
                }
                // Remove the next byte after the packet. This should contain the delimiter character.
                byteBuffer.RemoveAt(0);

                // Console.WriteLine(packet);
                OnNewScanReceived(packet);
            }
        }

        /// <summary>
        /// Counts each byte in the buffer until a delimiter character is found and then returns the length.
        /// </summary>
        /// <returns></returns>
        private int GetNextPacketLength()
        {
            int packetLength = 0;

            // Iterate the buffer and count until we reach a delimiter.
            foreach (var chunk in byteBuffer)
            {
                if (chunk != 13)
                {
                    packetLength++;
                }
                else
                {
                    // If a delimiter is found, return the length.
                    return packetLength;
                }
            }
            // If no delimiter found, then there is no complete packet in the buffer.
            return -1;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    readCancelTokenSource?.Cancel();
                    port?.DiscardInBuffer();
                    port?.DiscardOutBuffer();
                    port?.Close();
                    port?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion IDisposable Support

        //public void Dispose()
        //{
        //    readCancelTokenSource.Cancel();
        //    port.Close();
        //    port.Dispose();
        //}
    }
}