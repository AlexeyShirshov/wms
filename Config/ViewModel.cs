using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using WXML.Model;
using System.Web;
using System.Web.Hosting;
using System.Xml;
using System.IO;

namespace Config
{
    public class ModelStore : ConfigurationElement
    {
        [ConfigurationProperty("location", DefaultValue = "", IsRequired = true)]
        public string Location
        {
            get
            {
                return (string)this["location"];
            }
            set
            {
                this["location"] = value;
            }
        }

        [ConfigurationProperty("name", DefaultValue = "", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("description", DefaultValue = "", IsRequired = false)]
        public string Description
        {
            get
            {
                return (string)this["description"];
            }
            set
            {
                this["description"] = value;
            }
        }

        public WXMLModel LoadWXMLModel()
        {
            return WXMLModel.LoadFromXml(HostingEnvironment.MapPath(Location));
        }
    }

    public class ViewModels : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModelStore();
        }

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new ModelStore { Name = elementName };
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModelStore)element).Name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "model";
            }
        }

        public ModelStore this[int index]
        {
            get { return (ModelStore)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        public new ModelStore this[string name]
        {
            get 
            {
                return (ModelStore)BaseGet(name); 
            }
        }
    }
}
