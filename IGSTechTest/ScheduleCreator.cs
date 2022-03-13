using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    public class ScheduleCreator
    {

        public ScheduleCreator()
        {

        }

        // Find the recipe for a given tray
        private Recipe findRecipeForTray(CropTray tray, List<Recipe> recipes)
        {
            Recipe recipe = null;

            for (int i = 0; i < recipes.Count; i++)
            {
                if (recipes[i].getName() == tray.getCropName())
                {
                    recipe = recipes[i];
                }
            }

            return recipe;
        }

        public List<Action> createScheduleForFarm(VerticalFarm ourFarm)
        {
            List<Action> ourSchedule = new List<Action>();

            // Loop through all crop trays in our farm
            for (int i = 0; i < ourFarm.getCropTrays().Count; i++)
            {
                CropTray currentTray = ourFarm.getCropTrays()[i];

                // Find recipe for current crop tray
                Recipe trayRecipe = findRecipeForTray(currentTray, ourFarm.getFarmRecipes());

                List<Action> lightingActions = getLightingActions(trayRecipe, currentTray.getTrayName());
                List<Action> wateringActions = getWateringActions(trayRecipe, currentTray.getTrayName());

                ourSchedule.AddRange(lightingActions);
                ourSchedule.AddRange(wateringActions);
            }

            // Now need to order the list of actions by time
            ourSchedule = ourSchedule.OrderBy(o => o.getDateTime()).ToList();

            return ourSchedule;
        }

        public String getScheduleJsonFromActionList(List<Action> actions)
        {
            // Create json string prefix
            String json = @"{
                            ""schedule"":
                            [
                                {
                                ""actions"":[";



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

            json += @"]}
                ]
            }";

            return json;
        }

        private List<Action> getLightingActions(Recipe recipe, String trayName)
        {
            List<Action> actions = new List<Action>();

            // Create date / time stamp for keeping track of actions
            DateTime timeTrack = new DateTime();

            // Create an empty list of a fixed size
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
            for (int phaseNo = 0; phaseNo < orderedLightingPhases.Count; phaseNo++)
            {
                LightingPhase lightingPhase = orderedLightingPhases[phaseNo];

                for (int repetitionNo = 0; repetitionNo < lightingPhase.getRepetitions(); repetitionNo++)
                {
                    List<LightingPhaseOperation> operations = lightingPhase.getLightingPhaseOperations();

                    DateTime phaseStamp = timeTrack;

                    for (int operationNo = 0; operationNo < operations.Count; operationNo++)
                    {
                        LightingPhaseOperation currentOperation = operations[operationNo];

                        timeTrack = timeTrack.AddHours( (double) currentOperation.getOffsetHours());
                        timeTrack = timeTrack.AddMinutes( (double) currentOperation.getOffsetMinutes());

                        Dictionary<String, UInt16> actionData = new Dictionary<String, UInt16>();
                        actionData.Add("Intensity", currentOperation.getLightIntensity());

                        Action currentAction = new Action(trayName, recipe.getName(), timeTrack,
                                                "lighting", actionData);

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

        private List<Action> getWateringActions(Recipe recipe, String trayName)
        {
            List<Action> actions = new List<Action>();

            // Create date / time stamp for keeping track of actions
            DateTime timeTrack = new DateTime();

            // Create an empty list of a fixed size
            List<WateringPhase> orderedWateringPhases = 
                new List<WateringPhase>( new WateringPhase[recipe.getWateringPhases().Count] );

            // First put watering phases in order, using the order attribute
            for (int i = 0; i < recipe.getWateringPhases().Count; i++)
            {
                WateringPhase wateringPhase = recipe.getWateringPhases()[i];

                // Insert the lighting phase using its provided order number             
                orderedWateringPhases[wateringPhase.getOrder()] = wateringPhase;
            }

            // Start constructing the actions from the lighting phases
            for (int phaseNo = 0; phaseNo < orderedWateringPhases.Count; phaseNo++)
            {
                WateringPhase wateringPhase = orderedWateringPhases[phaseNo];

                for (int repetitionNo = 0; repetitionNo < wateringPhase.getRepetitions(); repetitionNo++)
                { 
                    Dictionary<String, UInt16> actionData = new Dictionary<String, UInt16>();
                    actionData.Add("Amount", wateringPhase.getAmount());

                    Action currentAction = new Action(trayName, recipe.getName(), timeTrack,
                                            "Watering", actionData);

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
