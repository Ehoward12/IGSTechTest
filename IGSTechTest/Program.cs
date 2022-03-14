using System.Net;
using DotNet.Docker;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Create our main application
MainApp application = new MainApp();

// The main application for our Recipe to JSON schedule software
public class MainApp
{
    // Globals

    // Our access to the API layer
    APIAccessLayer Api = new APIAccessLayer();
    // Our farm object
    VerticalFarm OurFarm;

    // Constants

    // Recipe URL for the API
    const String recipeUrl = "http://localhost:8080/recipe";
    // List containing the trays our farm will grow
    private readonly String[] trayArray = {"Basil", "Basil", "Basil", "Basil", "Basil",
                                         "Basil", "Strawberries", "Strawberries",
                                         "Strawberries", "Strawberries"};

    // Constructor
    public MainApp()
    {
        // Get the JSON from the API
        String recipesJson = Api.getJsonFromUrl(recipeUrl);

        // Parse the JSON data and store in easily readable objects
        JsonRecipeParser parser = new JsonRecipeParser(recipesJson);

        // Grab our recipes from the JSON returned from the API
        List<Recipe> ourRecipes = parser.getAllRecipes();

        // Create our farm object, containing the trays and the recipes
        createFarm(ourRecipes);

        // Schedule creator object is used to handler the creation of actions
        // for the schedule, using the information obtained from the API
        ScheduleCreator scheduleCreator = new ScheduleCreator();
        List<DotNet.Docker.Action> scheduleList = scheduleCreator.createScheduleForFarm(OurFarm);

        // Convert list of Actions to JSON string
        String scheduleJson = scheduleCreator.getScheduleJsonFromActionList(scheduleList);

        // Write JSON string to file
        System.IO.File.WriteAllText("schedule.JSON", scheduleJson);
    }

    // Creates our farm object with its crop trays
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





