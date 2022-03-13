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
        private String Name;
        private UInt16 Order;
        private UInt16 Hours;
        private UInt16 Minutes;
        private UInt16 Repetitions;
        private List<LightingPhaseOperation> Operations;

        public LightingPhase()
        {
            Operations = new List<LightingPhaseOperation>();
            Name = "";
            Order = 0;
            Hours = 0;
            Minutes = 0;
            Repetitions = 0;
        }

        public void setName(String nameIn)
        {
            this.Name = nameIn;  
        }

        public String getName()
        {
            return this.Name;
        }

        public void setOrder(UInt16 orderIn)
        {
            this.Order = orderIn;
        }

        public UInt16 getOrder()
        {
            return this.Order;
        }

        public void setHours(UInt16 hoursIn)
        {
            this.Hours = hoursIn;
        }

        public UInt16 getHours()
        {
            return this.Hours;
        }

        public void setMinutes(UInt16 minutesIn)
        {
            this.Minutes = minutesIn;
        }

        public UInt16 getMinutes()
        {
            return this.Minutes;
        }

        public void setRepetitions(UInt16 repetitionsIn)
        {
            this.Repetitions = repetitionsIn;
        }

        public UInt16 getRepetitions()
        {
            return this.Repetitions;
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

        public List<LightingPhaseOperation> getLightingPhaseOperations()
        {
            return this.Operations;
        }
    }
}
