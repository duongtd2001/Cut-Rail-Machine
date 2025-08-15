using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUT_RAIL_MACHINE.Services
{
    public class MachineData : FileRW
    {

        public string PLC_IP;
        public int PLC_PORT;

        public string EMPLOYEES_PATH;
        public string EMPLOYEES_FILENAME;

        public string MASTER_TYPE_PATH;
        public string MASTER_TYPE_FILENAME;

        public static List<string> connecttionString = new List<string>();

        string DataSource, InitialCatalog, PersistSecurityInfo, UserID, Password;
        string resultConnectString;
        public bool SaveSQL_OST;
        public bool SaveSQL_ERP;

        public MachineData() : base(Utils.DATAPATH, Utils.MACHINEDATA_FILENAME)
        {
        }

        public void ReadData(string _model = "")
        {
            string sVal;
            int iVal;
            bool bVal;

            var section = "PLC_INFO";
            sVal = ReadValue(_model, section, "PLC_IP", "192.168.0.1");
            if(!string.IsNullOrEmpty(sVal) ) PLC_IP = sVal;

            sVal = ReadValue(_model, section, "PLC_PORT", "502");
            if(int.TryParse(sVal, out iVal) ) PLC_PORT = iVal;

            section = "EMPLOYEES_INFO";
            sVal = ReadValue(_model, section, "PATH", "\\192.168.100.100\\05_OST_Program\\EMPLOYEES_OST");
            if(!string.IsNullOrEmpty(sVal)) EMPLOYEES_PATH = sVal;

            sVal = ReadValue(_model, section, "FILENAME", "Employees.xls");
            if (!string.IsNullOrEmpty(sVal)) EMPLOYEES_FILENAME = sVal;

            section = "MASTER_TYPE";
            sVal = ReadValue(_model, section, "PATH", "C:\\Users\\Public\\CUT_RAIL_DATA\\Data\\Type");
            if(!string.IsNullOrEmpty(sVal)) MASTER_TYPE_PATH = sVal;

            sVal = ReadValue(_model, section, "FILENAME", "Type.xlsx");
            if (!string.IsNullOrEmpty(sVal)) MASTER_TYPE_FILENAME = sVal;

            section = "SERVER_OST";
            sVal = ReadValue(_model, section, "DataSource", "192.168.100.100");
            if (!string.IsNullOrEmpty(sVal)) DataSource = $"Data Source={sVal}";

            sVal = ReadValue(_model, section, "InitialCatalog", "OST");
            if (!string.IsNullOrEmpty(sVal)) InitialCatalog = $"Initial Catalog={sVal}";

            sVal = ReadValue(_model, section, "PersistSecurityInfo", "True");
            if (!string.IsNullOrEmpty(sVal)) PersistSecurityInfo = $"Persist Security Info={sVal}";

            sVal = ReadValue(_model, section, "UserID", "ost_pe");
            if (!string.IsNullOrEmpty(sVal)) UserID = $"User ID={sVal}";

            sVal = ReadValue(_model, section, "Password", "ost_pe@spclt");
            if (!string.IsNullOrEmpty(sVal)) Password = $"Password={sVal}";

            sVal = ReadValue(_model, section, "SaveSQL", "false");
            if (bool.TryParse(sVal, out bVal)) SaveSQL_OST = bVal;

            resultConnectString = string.Format("{0};{1};{2};{3};{4}", DataSource, InitialCatalog, PersistSecurityInfo, UserID, Password);
            connecttionString.Add(resultConnectString);

            section = "SERVER_ERP";
            sVal = ReadValue(_model, section, "DataSource", "192.168.100.100");
            if (!string.IsNullOrEmpty(sVal)) DataSource = $"Data Source={sVal}";

            sVal = ReadValue(_model, section, "InitialCatalog", "ERP");
            if (!string.IsNullOrEmpty(sVal)) InitialCatalog = $"Initial Catalog={sVal}";

            sVal = ReadValue(_model, section, "PersistSecurityInfo", "True");
            if (!string.IsNullOrEmpty(sVal)) PersistSecurityInfo = $"Persist Security Info={sVal}";

            sVal = ReadValue(_model, section, "UserID", "fspu");
            if (!string.IsNullOrEmpty(sVal)) UserID = $"User ID={sVal}";

            sVal = ReadValue(_model, section, "Password", "@fspU");
            if (!string.IsNullOrEmpty(sVal)) Password = $"Password={sVal}";

            sVal = ReadValue(_model, section, "SaveSQL", "false");
            if (bool.TryParse(sVal, out bVal)) SaveSQL_ERP = bVal;

            resultConnectString = string.Format("{0};{1};{2};{3};{4}", DataSource, InitialCatalog, PersistSecurityInfo, UserID, Password);
            connecttionString.Add(resultConnectString);
        }
    }
}
