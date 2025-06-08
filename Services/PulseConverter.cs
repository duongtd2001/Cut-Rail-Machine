using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUT_RAIL_MACHINE.Services
{
    public class PulseConverter
    {
        private readonly double _pulsePerRev;
        private readonly double _mmPerRev;

        public double PulsePerMM => _pulsePerRev / _mmPerRev;
        public double MMPerPluse => _mmPerRev / _pulsePerRev;

        public PulseConverter(double pulsePerRev, double mmPerRev)
        {
            _pulsePerRev = pulsePerRev;
            _mmPerRev = mmPerRev;
        }

        public double ConvertMMtoPulse(double mm)
        {
            return mm * PulsePerMM;
        }

        public double ConvertPulseToMM(double pulse)
        {
            return pulse * MMPerPluse;
        }

        public double ConvertSpeedMMperSecToPulse(double mmPerSec)
        {
            return mmPerSec * PulsePerMM;
        }

        public double ConvertSpeedPulseToMMperSec(double pulsePerSec)
        {
            return pulsePerSec * MMPerPluse;
        }
    }
}
