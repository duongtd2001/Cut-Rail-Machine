using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Device;
using System.Net.Sockets;
using CUT_RAIL_MACHINE.Models;
using Modbus.Extensions.Enron;
using Caliburn.Micro;
using S7.Net;
using System.Runtime.InteropServices;
using System.Net;
using Modbus.Message;


namespace CUT_RAIL_MACHINE.Services
{
    public class ModbusTCP
    {
        TcpClient client;
        ModbusIpMaster master;

        private MachineData machineData;

        public ModbusTCP(MachineData machine)
        {
            machineData = machine;
            Connect();
        }

        public void Connect()
        {
            try
            {
                //client = new TcpClient(machineData.PLC_IP, machineData.PLC_PORT);
                //client.SendTimeout = 3000;
                //client.ReceiveTimeout = 3000;
                //master = ModbusIpMaster.CreateIp(client);
            }
            catch
            {
            }
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public bool IsModbus()
        {
            if(client != null)
            {
                return client.Connected;
            }
            else
                return false;
            
        }

        public List<double> ReadMutilHoldingReal(ushort startAddress, ushort numberOfPoint)
        {
            try
            {
                ushort[] realRegs = master.ReadHoldingRegisters(1, startAddress, numberOfPoint);
                List<double> result = new List<double>();
                for (int i = 0; i < realRegs.Length; i += 2)
                {
                    byte[] bytes = new byte[4];
                    bytes[0] = (byte)(realRegs[i] >> 8);
                    bytes[1] = (byte)(realRegs[i] & 0xFF);
                    bytes[2] = (byte)(realRegs[i + 1] >> 8);
                    bytes[3] = (byte)(realRegs[i + 1] & 0xFF);

                    float floatValue = BitConverter.ToSingle(bytes.Reverse().ToArray(), 0);
                    string stringValue = floatValue.ToString();
                    result.Add(Convert.ToDouble(stringValue));
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public List<int> ReadMutilHoldingInt(ushort startAddress, ushort numberOfPoint)
        {
            try
            {
                ushort[] realRegs = master.ReadHoldingRegisters(1, startAddress, numberOfPoint);
                List<int> result = new List<int>();
                {
                    for (int i = 0; i < realRegs.Length; i++)
                    {
                        result.Add(realRegs[i]);
                    }
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public void WriteMutilRegisters(ushort coilAddress, ushort[] value)
        {
            try
            {
                master.WriteMultipleRegisters(coilAddress, value);
                
            }
            catch
            {
            }
        }

        public void WriteSingleRegis(ushort coilAddress, ushort value)
        {
            try
            {
                master.WriteSingleRegister(coilAddress, value);
            }
            catch
            {
            }
            
        }

        public void WriteSingleRegsReal(ushort regsAddress, double value)
        {
            try
            {
                ushort[] regs = ConvertDoubleToUshortWrite(value);
                master.WriteMultipleRegisters(regsAddress, regs);
            }
            catch
            {
            }
        }

        private static ushort[] ConvertDoubleToUshortWrite(double value)
        {
            try
            {
                float f = (float)value;
                byte[] bytes = BitConverter.GetBytes(f);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);

                ushort highWord = (ushort)((bytes[0] << 8) | (bytes[1]));
                ushort lowWord = (ushort)((bytes[2] << 8) | (bytes[3]));
                return new ushort[] { highWord, lowWord };
            }
            catch
            {
                return null;
            }
        }
    }
}
