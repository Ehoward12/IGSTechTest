using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    public class LightingPhaseOperation
    {
        private UInt16 OffsetHours;
        private UInt16 OffsetMinutes;
        private UInt16 LightIntensity;

        public LightingPhaseOperation(UInt16 offsetHoursIn, UInt16 offsetMinutesIn, UInt16 lightIntensityIn)
        {
            OffsetHours = offsetHoursIn;
            OffsetMinutes = offsetMinutesIn;
            LightIntensity = lightIntensityIn;
        }

        public UInt16 getOffsetHours()
        {
            return OffsetHours;
        }

        public UInt16 getOffsetMinutes()
        {
            return OffsetMinutes;
        }

        public UInt16 getLightIntensity()
        {
            return LightIntensity;
        }
    }
}
