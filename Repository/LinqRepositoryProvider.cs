using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WXML.Model;
using LinqCodeGenerator;
using System.Reflection;

namespace Wms.Repository
{
    public class LinqRepositoryProvider : IRepositoryProvider
    {
        public object CreateRepository(WXMLModel model)
        {
            LinqCodeDomGenerator gen = new LinqCodeDomGenerator(model, new WXML.CodeDom.WXMLCodeDomGeneratorSettings());

            Assembly ass = gen.Compile(LinqToCodedom.CodeDomGenerator.Language.CSharp);

            Type t = ass.GetType("Wms.Data.WmsRepository");

            return Activator.CreateInstance(t);
        }
    }
}
