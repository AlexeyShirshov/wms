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
		private readonly string _fileName;
		
		public WmsBuildProvider(string fileName)
		{
			_fileName = String.IsNullOrEmpty(fileName) ? HostingEnvironment.MapPath(@"~/App_Data/Meta/entities.xml") : fileName;
		}

		public WmsBuildProvider() : this(null)
		{
			
		}

        public override void GenerateCode(AssemblyBuilder assemblyBuilder)
        {
            base.GenerateCode(assemblyBuilder);
            WXMLModel model = null;

            using (XmlReader xr = new XmlTextReader(_fileName))
            {
                model = WXMLModel.LoadFromXml(xr);
            }

            IRepositoryProvider prov = WmsDataFacade.GetRepositoryProvider();

            foreach (CodeCompileUnit unit in prov.CreateCompileUnits(model))
            {
                assemblyBuilder.AddCodeCompileUnit(this, unit);
            }

            string fn = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(assemblyBuilder.GetTempFilePhysicalPath("dll")));

            AssemblyLoadEventHandler d = null;
            d = delegate(object sender, AssemblyLoadEventArgs args)
            {
                if (args.LoadedAssembly.ManifestModule.Name.StartsWith(fn))
                {
                    prov.SetRepositoryAssembly(args.LoadedAssembly);
                    AppDomain.CurrentDomain.AssemblyLoad -= d;
                }
            };

            AppDomain.CurrentDomain.AssemblyLoad += d;
        }
    }
}
