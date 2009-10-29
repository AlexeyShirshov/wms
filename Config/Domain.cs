using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using WXML.Model;
using System.Web;
using System.Web.Hosting;

namespace Config
{
    public class Domain : ConfigurationElement
    {
        [ConfigurationProperty("connectionString", DefaultValue = "", IsRequired = false)]
        public string ConnectionString
        {
            get
            {
                return (string)this["connectionString"];
            }
            set
            {
                this["connectionString"] = value;
            }
        }

        [ConfigurationProperty("modelFilename", DefaultValue = "", IsRequired = true)]
        public string ModelFilename
        {
            get
            {
                return (string)this["modelFilename"];
            }
            set
            {
                this["modelFilename"] = value;
            }
        }

        public WXMLModel Model
        {
            get
            {
                return WXMLModel.LoadFromXml(HostingEnvironment.MapPath(ModelFilename));
            }
        }
    }

    public class Domains : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Domain();
        }

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new Domain { ModelFilename = elementName };
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Domain)element).ModelFilename;
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
                return "domain";
            }
        }

        public Domain this[int index]
        {
            get { return (Domain)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        public new Domain this[string filename]
        {
            get 
            { 
                return (Domain)BaseGet(filename); 
            }
        }
    }
}
