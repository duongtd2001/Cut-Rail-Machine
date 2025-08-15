using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUT_RAIL_MACHINE.Services
{
    public class MasterPosition : FileRW
    {
        
        public static Dictionary<string, double> Positions = new Dictionary<string, double>();

        public MasterPosition() : base(Utils.DATAPATH, Utils.MASTER_POSITION_FILENAME)
        {
        }

        public void ReadData(string _model = "")
        {
            string sVal;
            double dVal;

            var section = "LX15";
            sVal = ReadValue(_model, section, "DISTANCE", "0");
            if(double.TryParse(sVal, out dVal)) Positions["LX15"] = dVal;

            section = "LX20";
            sVal = ReadValue(_model, section, "DISTANCE", "0");
            if (double.TryParse(sVal, out dVal)) Positions["LX20"] = dVal;

            section = "LX26";
            sVal = ReadValue(_model, section, "DISTANCE", "0");
            if (double.TryParse(sVal, out dVal)) Positions["LX26"] = dVal;

            section = "LX30";
            sVal = ReadValue(_model, section, "DISTANCE", "0");
            if (double.TryParse(sVal, out dVal)) Positions["LX30"] = dVal;

            section = "LX45";
            sVal = ReadValue(_model, section, "DISTANCE", "0");
            if (double.TryParse(sVal, out dVal)) Positions["LX45"] = dVal;
        }
    }
}
