using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Timers;

namespace InventoryScanner.BarcodeScanning
{
    /// <summary>
    /// Provides serial port reading and parsing for high speed scanners.
    /// New serial data is buffered so that it can be parsed at a slower rate.
    /// </summary>
    public sealed class HighSpeedSerialPortReader : IDisposable, IScannerInput
    {
        private SerialPort port;
        private List<byte> byteBuffer = new List<byte>();
        private System.Threading.CancellationTokenSource readCancelTokenSource;
        private Timer parseReadsTimer = new Timer();
        private int parseInterval = 100; // How fast the data buffer will be parsed to the new scan event.

        public event EventHandler<string> NewScanReceived;
        public event EventHandler<Exception> ExceptionOccured;

        private void OnNewScanReceived(string data)
        {
            NewScanReceived?.Invoke(this, data);
        }

        public HighSpeedSerialPortReader(string portName)
        {
            port = new SerialPort(portName);
            port.Open();
            port.DtrEnable = true;

            // Start the parser timer.
            parseReadsTimer.Interval = 100;
            parseReadsTimer.Elapsed += ParseReadsTimer_Elapsed;
            parseReadsTimer.Start();

            readCancelTokenSource = new System.Threading.CancellationTokenSource();

            // Start the async reader.
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
                var bytesToRead = 64;
                var receiveBuffer = new byte[bytesToRead];
                var numBytesRead = await port.BaseStream.ReadAsync(receiveBuffer, 0, bytesToRead, cancelToken);
                var bytesReceived = new byte[numBytesRead];

                Array.Copy(receiveBuffer, bytesReceived, numBytesRead);

                // Add the data to a buffer to be parsed when possible.
                byteBuffer.AddRange(bytesReceived);
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

        public void Dispose()
        {
            readCancelTokenSource.Cancel();
            parseReadsTimer.Stop();
            parseReadsTimer.Dispose();
            port.Close();
            port.Dispose();
        }

        public void StartScanner()
        {
            throw new NotImplementedException();
        }
    }
}