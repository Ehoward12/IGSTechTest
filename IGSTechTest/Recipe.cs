using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    public class Recipe
    {
        public string Name { get; set; }

        public List<LightingPhase> LightingPhases { get; set; }

        public List<WateringPhase> WateringPhases { get; set; }

        public Recipe()
        {
            Name = "";
            LightingPhases = new List<LightingPhase>();
            WateringPhases = new List<WateringPhase>();
        }
    }
}
