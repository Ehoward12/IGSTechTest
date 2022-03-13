using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    public class WateringPhase
    {
        public UInt16 Amount { get; set; }

        public String Name { get; set; }

        public UInt16 Order { get; set; }

        public UInt16 Hours { get; set; }

        public UInt16 Minutes { get; set; }

        public UInt16 Repetitions { get; set; }

        public WateringPhase()
        {
            Amount = 0;
            Name = "";
            Order = 0;
            Hours = 0;
            Minutes = 0;
            Repetitions = 0;
        }

    }
}
