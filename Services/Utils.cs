using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUT_RAIL_MACHINE.Services
{
    public class Utils
    {
        public static string DATAPATH = Path.Combine("C:\\Users\\Public\\CUT_RAIL_DATA", "System");
        public static string MACHINEDATA_FILENAME = "MachineData.ini";
        public static string SYSTEMDATA_FILENAME = "System.ini";
        public static string MASTER_POSITION_FILENAME = "MasterPosition.ini";
        public static string MASTER_STONE = "MasterStone.ini";
    }
}
