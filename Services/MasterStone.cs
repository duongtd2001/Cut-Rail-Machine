using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUT_RAIL_MACHINE.Services
{
    public class MasterStone : FileRW
    {
        public Dictionary<string, double> msStone = new Dictionary<string, double>();

        public MasterStone() : base(Utils.DATAPATH, Utils.MASTER_STONE)
        {
        }

        public void ReadData(string _model = "")
        {
            string sVal;
            double dVal;

            var section = "FULL_SIZE";
            sVal = ReadValue(_model, section, "Value", "0");
            if(double.TryParse(sVal, out dVal)) msStone["FULLSIZE"] = dVal;

            section = "LX45";
            sVal = ReadValue(_model, section, "Value", "0");
            if (double.TryParse(sVal, out dVal)) msStone["LX45"] = dVal;

            section = "LX30";
            sVal = ReadValue(_model, section, "Value", "0");
            if (double.TryParse(sVal, out dVal)) msStone["LX30"] = dVal;

            section = "LX26";
            sVal = ReadValue(_model, section, "Value", "0");
            if (double.TryParse(sVal, out dVal)) msStone["LX26"] = dVal;

            section = "LX20";
            sVal = ReadValue(_model, section, "Value", "0");
            if (double.TryParse(sVal, out dVal)) msStone["LX20"] = dVal;

            section = "LX15";
            sVal = ReadValue(_model, section, "Value", "0");
            if (double.TryParse(sVal, out dVal)) msStone["LX15"] = dVal;

            section = "REPLACE";
            sVal = ReadValue(_model, section, "Value", "0");
            if (double.TryParse(sVal, out dVal)) msStone["REPLACE"] = dVal;

            section = "CURRENT";
            sVal = ReadValue(_model, section, "Value", "2000000");
            if (double.TryParse(sVal, out dVal)) msStone["CURRENT"] = dVal;
        }

        public void WriteData(string _model, string _section, string _key, object _iValue)
        {
            WriteValue(_model, _section, _key, _iValue);
        }
    }
}
