using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Wms.Interfaces;
using WXML.Model.Descriptors;
using LinqToCodedom;
using LinqToCodedom.Extensions;
using System.CodeDom;

namespace Wms.Web
{
	public class ViewGenerator : IViewGenerator
	{
		#region Implementation of IViewGenerator

		public void GenerateCreateView(EntityDefinition ed, TextWriter tw)
		{
			throw new NotImplementedException();
		}

		public void GenerateBrowseView(EntityDefinition ed, TextWriter tw)
		{
			throw new NotImplementedException();
		}

		public void GenerateEditView(EntityDefinition ed, TextWriter tw)
		{
			throw new NotImplementedException();
		}

		public void GenerateController(EntityDefinition ed, TextWriter tw)
		{
			var generator = new CodeDomGenerator();
			var controller  = generator.AddReference("System.Web.Mvc.dll")
				.AddNamespace("Wms.Controllers").AddClass(ed.Identifier + "Controller")
				.Implements(typeof(System.Web.Mvc.Controller));

			Console.WriteLine(generator.GenerateCode(CodeDomGenerator.Language.CSharp));

			var ass = generator.Compile();

			Console.WriteLine(ass.FullName);

			tw.WriteLine(generator.GenerateCode(CodeDomGenerator.Language.CSharp));
		}


		#endregion
	}
}
