using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    // Action class is used to store an individual action in the schedule
    public class Action : JsonSerializerSettings
    {
        private String TrayName;
        private String Crop;
        private DateTime Time;
        private String Type;
        private Dictionary<String, UInt16> Data;

        public Action(String trayIn, String cropIn, DateTime timeIn, 
            String typeIn, Dictionary<String, UInt16> dataIn)
        {
            TrayName = trayIn;
            Crop = cropIn;
            Time = timeIn;
            Type = typeIn;
            Data = dataIn;  
        }

        public String getTrayName()
        {
            return TrayName;
        }

        public String getCrop()
        {
            return Crop;
        }

        public DateTime getDateTime()
        {
            return Time;
        }

        public String getType()
        {
            return Type;
        }

        public Dictionary<String, UInt16> getData()
        {
            return Data;
        }

        
    }
}
