using System.Net;
using DotNet.Docker;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

MainApp application = new MainApp();

public class MainApp
{
    APIAccessLayer Api = new APIAccessLayer();
    VerticalFarm OurFarm;
    // Constants
    const String recipeUrl = "http://localhost:8080/recipe";
    private readonly String[] trayArray = {"Basil", "Basil", "Basil", "Basil", "Basil",
                                         "Basil", "Strawberries", "Strawberries",
                                         "Strawberries", "Strawberries"};

    public MainApp()
    {
        String recipesJson = Api.getJsonFromUrl(recipeUrl);

        JsonRecipeParser parser = new JsonRecipeParser(recipesJson);

        // Grab our recipes from the JSON returned from the API
        List<Recipe> ourRecipes = parser.getAllRecipes();

        // Create our farm object, containing the trays and the recipes
        createFarm(ourRecipes);

        ScheduleCreator scheduleCreator = new ScheduleCreator();
        List<DotNet.Docker.Action> scheduleList = scheduleCreator.createScheduleForFarm(OurFarm);

        String scheduleJson = scheduleCreator.getScheduleJsonFromActionList(scheduleList);

        System.IO.File.WriteAllText("schedule.JSON", scheduleJson);
    }

    private void createFarm(List<Recipe> ourRecipes)
    {
        List<CropTray> cropTrays = new List<CropTray>();

        // Grab all crops from constant array defined at the top and create crop
        // tray objects. Add these to our farm object
        for (int i = 0; i < trayArray.Length; i++)
        {
            String trayName = ( "Tray" + i.ToString() );

            CropTray tray = new CropTray(trayName, trayArray[i]);

            cropTrays.Add(tray);
        }

        OurFarm = new VerticalFarm("OurFarm", cropTrays, ourRecipes);
    }

}





