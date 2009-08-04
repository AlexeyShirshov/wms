using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WXML.Model;
using LinqCodeGenerator;
using System.Reflection;
using System.Configuration;
using System.Web.Hosting;
using System.Web.Compilation;

namespace Wms.Repository
{
    public class LinqRepositoryProvider : IRepositoryProvider
    {
        private Type _repositoryType;

        public LinqRepositoryProvider(Type t)
        {
            _repositoryType = t;
        }

        public LinqRepositoryProvider()
        {
        }

        public void Init(string tempPath, WXMLModel model)
        {
            LinqCodeDomGenerator gen = new LinqCodeDomGenerator(model, new WXML.CodeDom.WXMLCodeDomGeneratorSettings());

            Assembly ass = gen.Compile(/*tempPath + "\\Wms.Data.dll", */LinqToCodedom.CodeDomGenerator.Language.CSharp);

            _repositoryType = ass.GetType("Wms.Data.WmsRepository");
        }

        public object CreateRepository()
        {
            return Activator.CreateInstance(_repositoryType, (object)ConfigurationManager.ConnectionStrings["wms"].ConnectionString);
        }

        public Type RepositoryType 
        {
            get
            {
                return _repositoryType;
            }
        }
    }
}
