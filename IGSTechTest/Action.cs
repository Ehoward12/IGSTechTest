using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    public class Action : JsonSerializerSettings
    {
        private String crop;
        private DateTime time;
        private String type;
        private Dictionary<String, UInt16> data;

        public Action(String cropIn, DateTime timeIn, 
            String typeIn, Dictionary<String, UInt16> dataIn)
        {
            crop = cropIn;
            time = timeIn;
            type = typeIn;
            data = dataIn;  
        }

        public String getCrop()
        {
            return crop;
        }

        public DateTime getTime()
        {
            return time;    
        }

        public String getType()
        {
            return type;
        }

        public Dictionary<String, UInt16> getData()
        {
            return data;
        }
    }
}
