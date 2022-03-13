using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    public class Schedule : JsonSerializerSettings
    {
        List<Action> actions;

        public Schedule(List<Action> actionsIn)
        {
            actions = actionsIn;
        }

        public List<Action> getActions()
        {
            return actions;
        }
    }
}
