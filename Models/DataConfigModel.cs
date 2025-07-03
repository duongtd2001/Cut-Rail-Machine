using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUT_RAIL_MACHINE.Models
{
    public class DataConfigModel
    {
        // Data config plc siemen
        public static string CPUTypes { get; set; }
        public static string IP_PLC { get; set; }
        public static string _Rack { get; set; }
        public static string _Slot { get; set; }

        // fx serial
        public static string _Port { get; set; }
        public static string _BaudRate { get; set; }
        public static string _Parity { get; set; }
        public static string _DataBits { get; set; }
        public static string _StopBits { get; set; }

        // Data Employees
        public static string PathEmployees { get; set; }
        public static string FileNameEmp { get; set; }

        // Data SQL Server
        public static string DataSource { get; set; }
        public static string InitialCatalog { get; set; }
        public static string PersistSecurityInfo { get; set; }
        public static string UserID { get; set; }
        public static string Password { get; set; }
        public static string SaveSQL { get; set; }

        // Save Data
        public static string PathSaveData { get; set; }
        public static string FileSaveData { get; set; }
        public static string SaveExcel { get; set; }

        // Data Modbus
        public static string Modbus_IP { get; set; }
        public static string Modbus_Port { get; set; }
    }
}
