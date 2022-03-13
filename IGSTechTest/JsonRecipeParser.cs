using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    public class JsonRecipeParser
    {
        private JObject recipesJsonObj;

        public JsonRecipeParser(String strJsonRecipe)
        {
            recipesJsonObj = JObject.Parse(strJsonRecipe);
        }

        private List<JToken> getRecipeList()
        {
            List<JToken> recipeList = new List<JToken>();

            if (recipesJsonObj != null)
            {
                JToken recipes = recipesJsonObj.GetValue("recipes");

                if (recipes != null)
                {
                    recipeList = recipes.Values<JToken>().ToList();
                }
                else
                {
                    // Do nothing
                }
            }
            else
            {
                // Don nothing
            }

            return recipeList;
        }

        public int getNumberOfRecipes()
        {
            return getRecipeList().Count;
        }

        private List<LightingPhase> getLightingPhases(JObject recipe)
        {
            List<LightingPhase> lightingPhases = new List<LightingPhase>();

            // Get the lighting phases for the recipe given
            JToken lightingPhasesJson = recipe["lightingPhases"];

            if (lightingPhasesJson != null)
            {
                // Parse through the lighting phases and add all data from JSON into object
                for (int i = 0; i < lightingPhasesJson.ToArray<JToken>().Length; i++)
                {
                    LightingPhase currentLightingPhase = new LightingPhase();

                    // Use try here in case something unexpected happens
                    try
                    {

                        currentLightingPhase.Name = lightingPhasesJson[i]["name"].ToString();
                        currentLightingPhase.Hours = lightingPhasesJson[i]["hours"].Value<UInt16>();
                        currentLightingPhase.Minutes = lightingPhasesJson[i]["minutes"].Value<UInt16>();
                        currentLightingPhase.Repetitions = lightingPhasesJson[i]["repetitions"].Value<UInt16>();
                        currentLightingPhase.setOperations(lightingPhasesJson[i]["operations"].Values<JObject>().ToList());
                    }
                    catch
                    {

                    }

                    lightingPhases.Add(currentLightingPhase);
                }
            }

            return lightingPhases;
        }

        private List<WateringPhase> getWateringPhases(JObject recipe)
        {
            List<WateringPhase> wateringPhases = new List<WateringPhase>();

            // Get the watering phases for the recipe given
            JToken wateringPhasesJson = recipe["wateringPhases"];

            if (wateringPhasesJson != null)
            {
                // Parse through the watering phases and add all data from JSON into object
                for (int i = 0; i < wateringPhasesJson.ToArray<JToken>().Length; i++)
                {
                    WateringPhase currentWateringPhase = new WateringPhase();

                    // Use try here in case something unexpected happens
                    try
                    {
                        currentWateringPhase.Name = wateringPhasesJson[i]["name"].ToString();
                        currentWateringPhase.Amount = wateringPhasesJson[i]["amount"].Value<UInt16>();
                        currentWateringPhase.Order = wateringPhasesJson[i]["order"].Value<UInt16>();
                        currentWateringPhase.Hours = wateringPhasesJson[i]["hours"].Value<UInt16>();
                        currentWateringPhase.Minutes = wateringPhasesJson[i]["minutes"].Value<UInt16>();
                        currentWateringPhase.Repetitions = wateringPhasesJson[i]["repetitions"].Value<UInt16>();
                    }
                    catch
                    {

                    }

                    wateringPhases.Add(currentWateringPhase);
                }
            }

            return wateringPhases;
        }

        public Recipe getCompleteRecipe(UInt16 index)
        {
            Recipe recipe = new Recipe();

            List<JToken> recipesList = getRecipeList();

            // If the index supplied is valid
            if (recipesList.Count > index)
            {
                // Get the recipe (i.e. Basil)
                JObject currentRecipe = JObject.Parse(recipesList[index].ToString());

                if (currentRecipe != null)
                {
                    try
                    {


                        List<LightingPhase> lightingPhases = getLightingPhases(currentRecipe);

                        List<WateringPhase> wateringPhases = getWateringPhases(currentRecipe);

                        recipe.Name = currentRecipe["name"].ToString();
                        recipe.LightingPhases = lightingPhases;
                        recipe.WateringPhases = wateringPhases;

                     }
                    catch
                    {

                    }

                }

            }

            return recipe;
        }

       
    }
}
