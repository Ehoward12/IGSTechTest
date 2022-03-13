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
        public List<Action> actions { get; set; }

        public Schedule()
        {
            actions = new List<Action>();
        }
    }
}
