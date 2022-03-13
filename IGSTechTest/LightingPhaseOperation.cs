using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    public class LightingPhaseOperation
    {
        private UInt16 OffsetHours { get; set; }
        private UInt16 OffsetMinutes { get; set; }
        private UInt16 LightIntensity { get; set; } 

        public LightingPhaseOperation(UInt16 offsetHoursIn, UInt16 offsetMinutesIn, UInt16 lightIntensityIn)
        {
            OffsetHours = offsetHoursIn;
            OffsetMinutes = offsetMinutesIn;
            LightIntensity = lightIntensityIn;
        }
    }
}
