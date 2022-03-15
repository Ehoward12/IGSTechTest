using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    // Class is used to store information for a watering phase
    public class WateringPhase
    {
        private UInt16 Amount;
        private String Name;
        private UInt16 Order;
        private UInt16 Hours;
        private UInt16 Minutes;
        private UInt16 Repetitions;

        public WateringPhase()
        {
            Amount = 0;
            Name = "";
            Order = 0;
            Hours = 0;
            Minutes = 0;
            Repetitions = 0;
        }

        public void setAmount(UInt16 amountIn)
        {
            this.Amount = amountIn; 
        }

        public UInt16 getAmount()
        {
            return this.Amount;
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

    }
}
