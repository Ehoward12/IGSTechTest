using System.Net;
using DotNet.Docker;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

MainApp application = new MainApp();

public class MainApp
{
    APIAccessLayer api = new APIAccessLayer();
    const String recipeUrl = "http://localhost:8080/recipe";

    public MainApp()
    {
        String recipesJson = api.getJsonFromUrl(recipeUrl);

        JsonRecipeParser parser = new JsonRecipeParser(recipesJson);

        Recipe recipe = parser.getCompleteRecipe(0);

        Recipe recipe2 = parser.getCompleteRecipe(1);

        int i = 0;
    }

}





