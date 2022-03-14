using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    // This class is used to parse the JSON recipe data gathered from the API and 
    // convert it into a usable object
    public class JsonRecipeParser
    {
        private JObject recipesJsonObj;

        // Constructor - takes the raw JSON string
        public JsonRecipeParser(String strJsonRecipe)
        {
            recipesJsonObj = JObject.Parse(strJsonRecipe);
        }

        // Gets the list of recipes from the JSON object, stored as JTOkens prior to parsing
        private List<JToken> getRecipeList()
        {
            List<JToken> recipeList = new List<JToken>();

            if (recipesJsonObj != null)
            {
                // Searches for the recipes in the JSON
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
                // Do nothing
            }

            return recipeList;
        }

        // Helper function for getting the number of recipes in the JSON
        public int getNumberOfRecipes()
        {
            return getRecipeList().Count;
        }

        // Given an individual recipe (stored in a JObject), get a list of the lighting phases
        // stored within the recipe, as easily readable objects
        private List<LightingPhase> getLightingPhases(JObject recipe)
        {
            List<LightingPhase> lightingPhases = new List<LightingPhase>();

            // Get the lighting phases for the given recipe as a unparsed JToken
            JToken lightingPhasesJson = recipe["lightingPhases"];

            // Verify that it has found the JSON for the lighting phases
            if (lightingPhasesJson != null)
            {
                // Parse through the lighting phases and add all data from JSON into object
                for (int i = 0; i < lightingPhasesJson.ToArray<JToken>().Length; i++)
                {
                    LightingPhase currentLightingPhase = new LightingPhase();

                    // Use try here in case something unexpected happens when getting JSON details
                    try
                    {
                        // Construct our lighting phase object from JSON data
                        currentLightingPhase.setName(lightingPhasesJson[i]["name"].ToString());
                        currentLightingPhase.setOrder(lightingPhasesJson[i]["order"].Value<UInt16>());
                        currentLightingPhase.setHours(lightingPhasesJson[i]["hours"].Value<UInt16>());
                        currentLightingPhase.setMinutes(lightingPhasesJson[i]["minutes"].Value<UInt16>());
                        currentLightingPhase.setRepetitions(lightingPhasesJson[i]["repetitions"].Value<UInt16>());
                        currentLightingPhase.setOperations(lightingPhasesJson[i]["operations"].Values<JObject>().ToList());
                    }
                    catch
                    {
                        Console.WriteLine("ERROR: Unexpected behaviour setting lighting phase");
                    }

                    lightingPhases.Add(currentLightingPhase);
                }
            }

            return lightingPhases;
        }

        // Given an individual recipe (stored in a JObject), get a list of the watering phases
        // stored within the recipe, as easily readable objects
        private List<WateringPhase> getWateringPhases(JObject recipe)
        {
            List<WateringPhase> wateringPhases = new List<WateringPhase>();

            // Get the watering phases for the recipe given
            JToken wateringPhasesJson = recipe["wateringPhases"];

            // Verify that it has found the JSON for the watering phases
            if (wateringPhasesJson != null)
            {
                // Parse through the watering phases and add all data from JSON into object
                for (int i = 0; i < wateringPhasesJson.ToArray<JToken>().Length; i++)
                {
                    WateringPhase currentWateringPhase = new WateringPhase();

                    // Use try here in case something unexpected happens
                    try
                    {
                        // Construct our watering phase object from JSON data
                        currentWateringPhase.setName(wateringPhasesJson[i]["name"].ToString());
                        currentWateringPhase.setAmount(wateringPhasesJson[i]["amount"].Value<UInt16>());
                        currentWateringPhase.setOrder(wateringPhasesJson[i]["order"].Value<UInt16>());
                        currentWateringPhase.setHours(wateringPhasesJson[i]["hours"].Value<UInt16>());
                        currentWateringPhase.setMinutes(wateringPhasesJson[i]["minutes"].Value<UInt16>());
                        currentWateringPhase.setRepetitions(wateringPhasesJson[i]["repetitions"].Value<UInt16>());
                    }
                    catch
                    {
                        Console.WriteLine("ERROR: Unexpected behaviour setting watering phase");
                    }

                    wateringPhases.Add(currentWateringPhase);
                }
            }

            return wateringPhases;
        }

        // Gets a complete recipe object given the recipe index in the JSON
        private Recipe getCompleteRecipe(UInt16 index)
        {
            Recipe recipe = new Recipe();

            // Grabs the list of Recipes as unparsed JTokens
            List<JToken> recipesList = getRecipeList();

            // If the index supplied is valid
            if (recipesList.Count > index)
            {
                // Get the recipe (i.e. Basil)
                JObject currentRecipe = JObject.Parse(recipesList[index].ToString());

                // Verify getting the recipe hasn't failed
                if (currentRecipe != null)
                {
                    // Attempt to grab the lighting and watering phases from JSON
                    // something could go wrong if JSON is faulty, so keep in a try
                    try
                    {
                        // Grabs the lighting and watering phases from the JSON

                        List<LightingPhase> lightingPhases = getLightingPhases(currentRecipe);

                        List<WateringPhase> wateringPhases = getWateringPhases(currentRecipe);

                        // Forms the recipe using gathered information
                        recipe.setName(currentRecipe["name"].ToString());
                        recipe.setLightingPhases(lightingPhases);
                        recipe.setWateringPhases(wateringPhases);

                     }
                    catch
                    {
                        Console.WriteLine("ERROR: Issue when creating the recipe using JSON data");
                    }

                }

            }

            return recipe;
        }

        // Public function to grab all recipes from the JSON data
        public List<Recipe> getAllRecipes()
        {
            List<Recipe> allRecipes = new List<Recipe>();

            // Iterates through all recipes and grabs the recipe object (from raw JSON) for each one
            for (UInt16 i = 0; i < getNumberOfRecipes(); i++)
            {
                allRecipes.Add(getCompleteRecipe(i));
            }

            return allRecipes;
        }

       
    }
}
