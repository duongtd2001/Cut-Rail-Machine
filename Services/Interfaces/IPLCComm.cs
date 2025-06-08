using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUT_RAIL_MACHINE.Services.Interfaces
{
    public interface IPLCComm
    {
        void ConnectPLC();
        void DisconnectPLC();
        bool IsConnected();
        bool ReadBoolFromPLC(string address);
        void WriteBoolToPLC(string address, bool value);
        double ReadDoubleFromPLC(string address);
        void WriteDoubleToPLC(string address, double value);
    }
}
