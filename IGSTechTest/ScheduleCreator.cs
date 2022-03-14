using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    public class ScheduleCreator
    {

        // Constructor
        public ScheduleCreator()
        {
            // Do nothing
        }

        // Pulls the recipe for a given tray
        private Recipe findRecipeForTray(CropTray tray, List<Recipe> recipes)
        {
            Recipe recipe = null;

            // Check through the recipes and find which recipe our tray needs to use
            for (int i = 0; i < recipes.Count; i++)
            {
                if (recipes[i].getName() == tray.getCropName())
                {
                    recipe = recipes[i];
                }
            }

            return recipe;
        }

        // Public operation used to create our farm schedule, given the farm object
        // containing the recipes and the trays
        public List<Action> createScheduleForFarm(VerticalFarm ourFarm)
        {
            List<Action> ourSchedule = new List<Action>();

            // Loop through all crop trays in our farm
            for (int i = 0; i < ourFarm.getCropTrays().Count; i++)
            {
                CropTray currentTray = ourFarm.getCropTrays()[i];

                // Find recipe for current crop tray
                Recipe trayRecipe = findRecipeForTray(currentTray, ourFarm.getFarmRecipes());

                // Creates a list of Schedule Actions for the lighting and then watering phases of tray
                List<Action> lightingActions = getLightingActions(trayRecipe, currentTray.getTrayName());
                List<Action> wateringActions = getWateringActions(trayRecipe, currentTray.getTrayName());

                // Adds actions to main list
                ourSchedule.AddRange(lightingActions);
                ourSchedule.AddRange(wateringActions);
            }

            // Now need to order the list of actions by time, so schedule is in proper order
            ourSchedule = ourSchedule.OrderBy(o => o.getDateTime()).ToList();

            return ourSchedule;
        }

        // Convert our list of actions into a JSON string
        public String getScheduleJsonFromActionList(List<Action> actions)
        {
            // Create json string prefix
            String json = @"{
                            ""schedule"":
                            [
                                {
                                ""actions"":[";



            // Loops through all actions and creates a JSON string for each, adding to main JSON string
            for (int i = 0; i < actions.Count; i++)
            {
                Action currentAction = actions[i];

                json += "{";
                json += @" ""trayName"": "" " + currentAction.getTrayName() + @""",";
                json += @" ""crop"": "" " + currentAction.getCrop() + @""",";
                json += @" ""datetime"": "" " + currentAction.getDateTime().ToString() + @""",";
                json += @" ""type"": "" " + currentAction.getType() + @""",";
                json += @" """ + currentAction.getData().Keys.ToList()[0] + @""": "" " + currentAction.getData().Values.ToList()[0] + @"""";
                json += "}";

                // If we're not on our final iteration of the loop, add a comma for the next action
                if (actions.Count != i + 1)
                {
                    json += ",";
                }
            }

            // Suffix for JSON contents
            json += @"]}
                ]
            }";

            return json;
        }

        // Creates a list of actions for our lighting phases of a tray
        private List<Action> getLightingActions(Recipe recipe, String trayName)
        {
            List<Action> actions = new List<Action>();

            // Create date / time stamp for keeping track of actions
            DateTime timeTrack = new DateTime();

            // Create an empty list of a fixed size - number of lighting phases in recipe
            List<LightingPhase> orderedLightingPhases = 
                new List<LightingPhase>(new LightingPhase[recipe.getLightingPhases().Count]);

            // First put lighting phases in order, using the order attribute
            for (int i = 0; i < recipe.getLightingPhases().Count; i++)
            {
                LightingPhase lightingPhase = recipe.getLightingPhases()[i];

                // Insert the lighting phase using its provided order number
                orderedLightingPhases[lightingPhase.getOrder()] = lightingPhase;
            }

            // Start constructing the actions from the lighting phases
            // Loop through all lighting phases
            for (int phaseNo = 0; phaseNo < orderedLightingPhases.Count; phaseNo++)
            {
                LightingPhase lightingPhase = orderedLightingPhases[phaseNo];

                // Loop through number of repetitions each phase requires
                for (int repetitionNo = 0; repetitionNo < lightingPhase.getRepetitions(); repetitionNo++)
                {
                    List<LightingPhaseOperation> operations = lightingPhase.getLightingPhaseOperations();

                    // Take a timestamp here, as this will be used to reset our tracker to prior to incrementing time
                    DateTime phaseStamp = timeTrack;

                    // Loop through each lighting phase operation and create an action
                    for (int operationNo = 0; operationNo < operations.Count; operationNo++)
                    {
                        LightingPhaseOperation currentOperation = operations[operationNo];

                        // Increment time tracker with hours and minutes from current operation
                        timeTrack = timeTrack.AddHours( (double) currentOperation.getOffsetHours());
                        timeTrack = timeTrack.AddMinutes( (double) currentOperation.getOffsetMinutes());

                        // Add the intensity as a dict, to the action
                        Dictionary<String, UInt16> actionData = new Dictionary<String, UInt16>();
                        actionData.Add("Intensity", currentOperation.getLightIntensity());

                        // Form the action using the gathered data
                        Action currentAction = new Action(trayName, recipe.getName(), timeTrack,
                                                "lighting", actionData);

                        // Add the action to our list
                        actions.Add(currentAction);

                        // Reset time track to beginning of phase
                        timeTrack = phaseStamp;
                    }

                    // Add phase hours
                    // This will keep the time track up to date on the current repetition
                    timeTrack = timeTrack.AddHours(lightingPhase.getHours());

                }

                
            }

            return actions;
        }

        // Creates a list of actions for our waterings phases of a tray
        private List<Action> getWateringActions(Recipe recipe, String trayName)
        {
            List<Action> actions = new List<Action>();

            // Create date / time stamp for keeping track of actions
            DateTime timeTrack = new DateTime();

            // Create an empty list of a fixed size - number of lighting phases in recipe
            List<WateringPhase> orderedWateringPhases = 
                new List<WateringPhase>( new WateringPhase[recipe.getWateringPhases().Count] );

            // First put watering phases in order, using the order attribute
            for (int i = 0; i < recipe.getWateringPhases().Count; i++)
            {
                WateringPhase wateringPhase = recipe.getWateringPhases()[i];

                // Insert the watering phase using its provided order number             
                orderedWateringPhases[wateringPhase.getOrder()] = wateringPhase;
            }

            // Start constructing the actions from the watering phases
            // Loop through all the watering phases contained within the recipe
            for (int phaseNo = 0; phaseNo < orderedWateringPhases.Count; phaseNo++)
            {
                WateringPhase wateringPhase = orderedWateringPhases[phaseNo];

                // Loop through number of repetitions each phase requires
                for (int repetitionNo = 0; repetitionNo < wateringPhase.getRepetitions(); repetitionNo++)
                { 
                    // Add water amount to dict, and then into action
                    Dictionary<String, UInt16> actionData = new Dictionary<String, UInt16>();
                    actionData.Add("Amount", wateringPhase.getAmount());

                    // Form action using gathered information of the watering phase
                    Action currentAction = new Action(trayName, recipe.getName(), timeTrack,
                                            "Watering", actionData);

                    // Add watering phase to list
                    actions.Add(currentAction);

                    // Update time tracker following addition of action
                    timeTrack = timeTrack.AddHours((double)wateringPhase.getHours());
                    timeTrack = timeTrack.AddMinutes((double)wateringPhase.getMinutes());
                }
            }

            return actions;
        }
    }
}
