using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    // Class is used to store information for a recipe
    public class Recipe
    {
        private string Name;

        private List<LightingPhase> LightingPhases;

        private List<WateringPhase> WateringPhases;
        
        public Recipe()
        {
            Name = "";
            LightingPhases = new List<LightingPhase>();
            WateringPhases = new List<WateringPhase>();
        }

        public void setName(String name)
        {
            this.Name = name;
        }

        public String getName()
        {
            return Name;
        }

        public void setLightingPhases(List<LightingPhase> lightingPhases)
        {
            this.LightingPhases = lightingPhases;
        }

        public List<LightingPhase> getLightingPhases()
        {
            return LightingPhases;
        }

        public void setWateringPhases(List<WateringPhase> wateringPhases)
        {
            this.WateringPhases = wateringPhases;
        }

        public List<WateringPhase> getWateringPhases()
        {
            return WateringPhases;
        }
    }
}
