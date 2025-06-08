using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUT_RAIL_MACHINE.Models
{
    public static class UserSession
    {
        public static string CurrentUser { get; set; }
        public static string CurrentID { get; set; }
        public static string CurrentAccess { get; set; }
        public static string CurrentPO { get; set; }
        public static int NumberOfLoginTimes { get; set; }
        public static int SavePos { get; set; } = 0;
    }
}
