using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using WXML.Model;
using System.Web;
using System.Web.Hosting;
using WXML.Model.Descriptors;
using WXML.Model.Database.Providers;
using WXML.SourceConnector;

namespace Config
{
    public class Domain : ModelStore
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

        public SourceView GetSourceView()
        {
            ISourceProvider p = CreateDomainProvider();

            return p.GetSourceView(null, null, true, true);
        }

        public WXMLModel CreateNewModelFromSource(string names)
        {
            ISourceProvider p = CreateDomainProvider();

            WXMLModel model = new WXMLModel();

            SourceView sv = p.GetSourceView(null, names, true, true);

            SourceToModelConnector connector = new SourceToModelConnector(sv, model);

            connector.ApplySourceViewToModel();

            return model;
        }

        public WXMLModel OpenForEdit()
        {
            return ModelManager.OpenModelStoreForEdit(this);
        }

        public void Save()
        {
            ModelManager.SaveModelStore(this);
        }

        public string SynchronizeWithSource()
        {
            ISourceProvider p = CreateDomainProvider();

            SourceView sv = p.GetSourceView(null, null, true, true);

            ModelToSourceConnector connector = new ModelToSourceConnector(sv, LoadWXMLModel());

            return connector.GenerateSourceScript(p, true);
        }

        private ISourceProvider CreateDomainProvider()
        {
            return new MSSQLProvider(ConnectionString, null);
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
            return new Domain { Name = elementName };
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Domain)element).Name;
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

        public new Domain this[string name]
        {
            get 
            { 
                return (Domain)BaseGet(name); 
            }
        }
    }
}
