using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    public class VerticalFarm
    {
        private String FarmName;
        private List<CropTray> CropTrays;
        private List<Recipe> FarmRecipes;

        public VerticalFarm(String farmNameIn, List<CropTray> cropTraysIn, List<Recipe> farmRecipesIn)
        {
            this.FarmName = farmNameIn;
            this.CropTrays = cropTraysIn; 
            this.FarmRecipes = farmRecipesIn;
        }

        public String getFarmName()
        {
            return FarmName;
        }

        public List<CropTray> getCropTrays()
        {
            return CropTrays;
        }

        public List<Recipe> getFarmRecipes()
        {
            return FarmRecipes;
        }

    }
}
