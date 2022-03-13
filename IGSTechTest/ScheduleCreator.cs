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
                if (recipes[i].getName() == tray.getTrayName())
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


            return ourSchedule;
        }

        private List<Action> getLightingActions(Recipe recipe, String trayName)
        {
            List<Action> actions = new List<Action>();

            // Create date / time stamp for keeping track of actions
            DateTime timeTrack = new DateTime();

            List<LightingPhase> orderedLightingPhases = new List<LightingPhase>();

            // First put lighting phases in order, using the order attribute
            for (int i = 0; i < recipe.getLightingPhases().Count; i++)
            {
                LightingPhase lightingPhase = recipe.getLightingPhases()[i];

                // Insert the lighting phase using its provided order number
                orderedLightingPhases.Insert(lightingPhase.getOrder(), lightingPhase);
            }

            // Start constructing the actions from the lighting phases
            for (int phaseNo = 0; phaseNo < orderedLightingPhases.Count; phaseNo++)
            {
                LightingPhase lightingPhase = orderedLightingPhases[phaseNo];

                DateTime phaseStamp = timeTrack;

                for (int repetitionNo = 0; repetitionNo < lightingPhase.getRepetitions(); repetitionNo++)
                {
                    List<LightingPhaseOperation> operations = lightingPhase.getLightingPhaseOperations();

                    for (int operationNo = 0; operationNo < operations.Count; operationNo++)
                    {
                        LightingPhaseOperation currentOperation = operations[operationNo];

                        timeTrack.AddHours(currentOperation.getOffsetHours());
                        timeTrack.AddMinutes(currentOperation.getOffsetMinutes());

                        Dictionary<String, UInt16> actionData = new Dictionary<String, UInt16>();
                        actionData.Add("Intensity", currentOperation.getLightIntensity());

                        Action currentAction = new Action(trayName, recipe.getName(), timeTrack,
                                                "lighting", actionData);

                        actions.Add(currentAction);
                    }

                    // Reset time track to beginning of phase
                    timeTrack = phaseStamp;
                    // Add phase hours * number of current repetitions
                    // This will keep the time track up to date on the current repetition
                    timeTrack.AddHours(lightingPhase.getHours() * (repetitionNo + 1));

                }

                
            }

            return actions;
        }

        private List<Action> getWateringActions(Recipe recipe, String trayName)
        {
            List<Action> actions = new List<Action>();

            // Create date / time stamp for keeping track of actions
            DateTime dateTime = new DateTime();

            return actions;
        }
    }
}
