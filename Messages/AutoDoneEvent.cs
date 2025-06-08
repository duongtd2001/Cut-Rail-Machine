using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUT_RAIL_MACHINE.Messages
{
    public class AutoDoneEvent
    {
        public int Message { get; set; }
        public AutoDoneEvent(int message)
        {
            Message = message;
        }
    }
}
