using System.Net;
using DotNet.Docker;


MainApp application = new MainApp();

public class MainApp
{
    APIAccessLayer api = new APIAccessLayer();
    const String recipeUrl = "http://localhost:8080/recipe";

    public MainApp()
    {
        String result = api.getJsonFromUrl(recipeUrl);
        Console.WriteLine(result);

    }

}





