using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    public class CropTray
    {
        private String TrayName;

        private String CropName;

        public CropTray(String trayNameIn, String cropNameIn)
        {
            this.TrayName = trayNameIn;
            this.CropName = cropNameIn;
        }

        public String getTrayName()
        {
            return TrayName;
        }

        public String getCropName()
        {
            return CropName;
        }
    }
}
