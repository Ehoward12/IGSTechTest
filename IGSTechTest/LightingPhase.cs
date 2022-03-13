using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{

    public class LightingPhase
    {
        private List<LightingPhaseOperation> Operations;
        public String Name { get; set; }
        public UInt16 Order { get; set; }
        public UInt16 Hours { get; set; }
        public UInt16 Minutes { get; set; }
        public UInt16 Repetitions { get; set; }

        public LightingPhase()
        {
            Operations = new List<LightingPhaseOperation>();
            Name = "";
            Order = 0;
            Hours = 0;
            Minutes = 0;
            Repetitions = 0;
        }

        public void setOperations(List<JObject> operationList)
        {
            // Add all operations as objects to the operations list
            for (int i = 0; i < operationList.Count; i++)
            {
                LightingPhaseOperation phase;

                try
                {
                    phase = new LightingPhaseOperation(
                        operationList[i]["offsetHours"].Value<UInt16>(),
                        operationList[i]["offsetMinutes"].Value<UInt16>(),
                        operationList[i]["lightIntensity"].Value<UInt16>()
                        );
                }
                catch
                {
                    phase = new LightingPhaseOperation(0, 0, 0);
                }
                Operations.Add(phase);
            }
        }
    }
}
