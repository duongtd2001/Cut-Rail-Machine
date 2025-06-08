using SerialPortFX.Profinet;
using SerialPortFX;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CUT_RAIL_MACHINE.Models;

namespace CUT_RAIL_MACHINE.Services
{
    public class SerialPortPLC
    {
        MelsecFxSerial serialPort;
        public SerialPortPLC()
        {
            serialPort = new MelsecFxSerial();
            Parity parity = (Parity)Enum.Parse(typeof(Parity), DataConfigModel._Parity);
            StopBits stopBits = (StopBits)Enum.Parse(typeof(StopBits), DataConfigModel._StopBits);
            serialPort.SerialPortInni(DataConfigModel._Port, Convert.ToInt32(DataConfigModel._BaudRate), Convert.ToInt32(DataConfigModel._DataBits), stopBits, parity);
        }
        public void ConnectPLC()
        {
            try
            {
                serialPort.Open();
            }
            catch
            {
            }
        }
        public bool CheckConnectPLC()
        {
            return serialPort.IsOpen();
        }
        public string PortName()
        {
            return serialPort.PortName;
        }
        public void DisconnectPLC()
        {
            try
            {
                if (serialPort.IsOpen())
                {
                    serialPort.Close();
                    serialPort.Dispose();
                }
            }
            catch { }

        }
        bool convertResult;
        double convertResultDB;
        public bool ReadDataFromPLC(string bitget)
        {

            if (serialPort.IsOpen())
            {
                OperateResult<bool> result = serialPort.ReadBool(bitget);
                if (result.IsSuccess)
                {
                    convertResult = result.Content;
                }
            }
            return convertResult;
        }
        public double ReadDataFromPLC2(string bitget)
        {
            OperateResult<Int32> result = serialPort.ReadInt32(bitget);
            if (result.IsSuccess)
            {
                convertResultDB = result.Content;
            }
            return convertResultDB;
        }
        public void WriteDataToPLC2(string bitget, Int32 valueDB)
        {
            serialPort.Write(bitget, valueDB);
        }

        public void WriteDataToPLC(string bitset, bool valueset)
        {
            serialPort.Write(bitset, valueset);
        }
    }
}
