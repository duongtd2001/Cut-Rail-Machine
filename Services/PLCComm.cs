using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S7.Net;
using S7.Net.Types;
using CUT_RAIL_MACHINE.Models;
using System.Windows;

namespace CUT_RAIL_MACHINE.Services
{
    public class PLCComm
    {
        private Plc S7Comm;
        private CpuType cpuTypes;
        public PLCComm()
        {
            cpuTypes = (CpuType)Enum.Parse(typeof(CpuType), DataConfigModel.CPUTypes);
            S7Comm = new Plc(cpuTypes, DataConfigModel.IP_PLC, Convert.ToInt16(DataConfigModel._Rack), Convert.ToInt16(DataConfigModel._Slot));
        }

        public void ConnectPLC()
        {
            try
            {
                S7Comm.Open();
            }
            catch
            { }
        }

        public void DisconnectPLC()
        {
            if (S7Comm.IsConnected)
            {
                S7Comm.Close();
            }
        }

        public bool IsConnect()
        {
            return S7Comm.IsConnected;
        }

        public bool ReadBoolFromPLC(string address)
        {
            try
            {
                bool result = (bool)S7Comm.Read(address);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public double ReadRealFromPLC(string address)
        {
            return ((uint)S7Comm.Read(address)).ConvertToDouble();
        }

        public void WriteBoolToPLC(string address, int value)
        {
            S7Comm.Write(address, value);
        }

        public void WriteDoubleToPLC(string address, double value)
        {
            S7Comm.Write(address, value);
        }
    }
}
