using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wms.Tests.Fakes;
using Wms.Web;
using WXML.Model.Descriptors;

namespace Wms.Tests
{
	[TestFixture]
	public class ViewGeneratorTest
	{
		[Test]
		public void Can_Generate_Edit_View()
		{
			var generator = new ViewGenerator();
			var sw = new StringWriter();

			generator.GenerateEditView(GetEntityDefinition(), sw);

			Assert.IsFalse(String.IsNullOrEmpty(sw.GetStringBuilder().ToString()));
		}

		[Test]
		public void Can_Generate_Create_View()
		{
			
		}

		[Test]
		public void Can_Generate_Browse_View()
		{

		}

		[Test]
		public void Can_Generate_Controller()
		{
			var generator = new ViewGenerator();
			var sw = new StringWriter();

			generator.GenerateController(GetEntityDefinition(), sw);
			
			var cdp = CodeDomProvider.CreateProvider("cs");
			
			var stringResult = sw.GetStringBuilder().ToString();
			Console.WriteLine(stringResult);

			var resultAssembly  = cdp.CompileAssemblyFromSource(new CompilerParameters(), stringResult.Split('\n'));
			var resultType = resultAssembly.CompiledAssembly.GetType("PostController");

			Assert.IsNotNull(resultType);
		}
		
		private static EntityDefinition  GetEntityDefinition()
		{
			return new FakeDataFacade().EntityModel.GetEntity("Post");
		}


	}
}
