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

        public static WmsRoot Config
        {
            get
            {
                return (WmsRoot)ConfigurationManager.GetSection("WmsRoot");
            }
        }
    }

    
}
