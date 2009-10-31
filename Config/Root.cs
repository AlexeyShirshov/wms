using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Config
{

    public class WmsRoot : ConfigurationSection
    {
        [
        ConfigurationProperty("domains", IsDefaultCollection = false),
        ConfigurationCollection(typeof(Domains))
        ]
        public Domains Domains
        {
            get { return this["domains"] as Domains; }
        }

        [
        ConfigurationProperty("viewModels", IsDefaultCollection = false),
        ConfigurationCollection(typeof(ViewModels))
        ]
        public ViewModels ViewModels
        {
            get { return this["viewModels"] as ViewModels; }
        }

        public static WmsRoot Config
        {
            get
            {
                return (WmsRoot)ConfigurationManager.GetSection("wms");
            }
        }
    }

    
}
