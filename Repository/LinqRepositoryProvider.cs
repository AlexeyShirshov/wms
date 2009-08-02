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
        public object CreateRepository(string tempPath, WXMLModel model)
        {
            LinqCodeDomGenerator gen = new LinqCodeDomGenerator(model, new WXML.CodeDom.WXMLCodeDomGeneratorSettings());

            Assembly ass = gen.Compile(/*tempPath + "\\Wms.Data.dll", */LinqToCodedom.CodeDomGenerator.Language.CSharp);

            Type t = ass.GetType("Wms.Data.WmsRepository");

            return Activator.CreateInstance(t, (object)ConfigurationManager.ConnectionStrings["wms"].ConnectionString);
        }
    }
}
