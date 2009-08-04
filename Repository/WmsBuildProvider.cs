using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;
using Wms.Repository;
using LinqCodeGenerator;
using WXML.Model;
using System.Xml;
using System.Web.Hosting;
using System.CodeDom;
using System.IO;

namespace Wms
{
    [BuildProviderAppliesTo(BuildProviderAppliesTo.Code)]
    public class WmsBuildProvider : BuildProvider
    {
        public override void GenerateCode(AssemblyBuilder assemblyBuilder)
        {
            base.GenerateCode(assemblyBuilder);

            WXMLModel model = WXMLModel.LoadFromXml(new XmlTextReader(HostingEnvironment.MapPath(@"~/App_Data/Meta/entities.xml")));

            //IRepositoryProvider prov = WmsDataFacade.GetRepositoryProvider(assemblyBuilder.GetTempFilePhysicalPath("dll"), model);

            //assemblyBuilder.AddAssemblyReference(prov.RepositoryType.Assembly);

            LinqCodeDomGenerator gen = new LinqCodeDomGenerator(model, new WXML.CodeDom.WXMLCodeDomGeneratorSettings());

            LinqToCodedom.CodeDomGenerator.Language lang = LinqToCodedom.CodeDomGenerator.Language.CSharp;

            CodeCompileUnit unit = gen.GetCompileUnit(lang);

            //CodeSnippetCompileUnit unit = new CodeSnippetCompileUnit("namespace Wms.Data.Internal { public static class Page2 { public static string Hello { get { return \"hi!\";}}}}");

            assemblyBuilder.AddCodeCompileUnit(this, unit);

            string fn = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(assemblyBuilder.GetTempFilePhysicalPath("dll")));

            AssemblyLoadEventHandler d = null;
            d = delegate(object sender, AssemblyLoadEventArgs args)
            {
                if (args.LoadedAssembly.ManifestModule.Name.StartsWith(fn))
                {
                    WmsDataFacade.SetRepositoryProvider(args.LoadedAssembly.GetType("Wms.Data.WmsRepository"));
                    AppDomain.CurrentDomain.AssemblyLoad -= d;
                }
            };

            AppDomain.CurrentDomain.AssemblyLoad += d;
        }
    }
}
