using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUT_RAIL_MACHINE.Models
{
    public class UserModel
    {
        public string Machine { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string ProducName { get; set; }
        public string LotNo { get; set; }
        public string Time { get; set; }
        //
        public string STT { get; set; }
        public string Access { get; set; }
        public string PO { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Lenght { get; set; }  
        public string _Type { get; set; }
        public string Quantity { get; set; }
    }
}
