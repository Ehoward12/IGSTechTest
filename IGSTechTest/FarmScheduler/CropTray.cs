using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Docker
{
    // Class is used to store information for a crop tray
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
