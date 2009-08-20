using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wms.Tests.Fakes;
using Wms.Web;
using WXML.Model.Descriptors;
using Microsoft.CSharp;
using LinqToCodedom;

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

			Console.WriteLine(sw.ToString());

			Assert.IsFalse(String.IsNullOrEmpty(sw.GetStringBuilder().ToString()));
		}

		[Test]
		public void Can_Generate_Create_View()
		{
			var generator = new ViewGenerator();
			var sw = new StringWriter();

			generator.GenerateCreateView(GetEntityDefinition(), sw);

			Console.WriteLine(sw.ToString());

			Assert.IsFalse(String.IsNullOrEmpty(sw.GetStringBuilder().ToString()));
		}


		[Test]
		public void Can_Generate_Controller()
		{
			var generator = new ViewGenerator();

			var ccu = generator.GenerateController(GetEntityDefinition());

			var cdp = new CSharpCodeProvider(new Dictionary<string, string> {{"CompilerVersion", "3.5"}});
			var opts = new CompilerParameters {GenerateInMemory = true, GenerateExecutable = false};
    	    var resultAssembly  = cdp.CompileAssemblyFromDom(opts, ccu);

            Assert.AreEqual(0, resultAssembly.Errors.Count);

			Type controllerType = resultAssembly.CompiledAssembly.GetType("Wms.Controllers.PostController");

            Assert.IsNotNull(controllerType);
			Assert.IsTrue(typeof(Controller).IsAssignableFrom(controllerType));
		}
		
		[Test]
		public void Can_Generate_Browse_Action()
		{
			var generator = new ViewGenerator();

            var ccu = generator.GenerateController(GetEntityDefinition());

            var cdp = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "3.5" } });
            var opts = new CompilerParameters { GenerateInMemory = true, GenerateExecutable = false };
            var resultAssembly = cdp.CompileAssemblyFromDom(opts, ccu);

            Type controllerType = resultAssembly.CompiledAssembly.GetType("Wms.Controllers.PostController");
			var browseAction = controllerType.GetMethods().FirstOrDefault(mi => mi.Name == "Browse");

			Assert.IsNotNull(browseAction);
			Assert.IsTrue(browseAction.ReturnType.IsAssignableFrom(typeof(ActionResult)));
		}

		[Test]
		public void Browse_Action_Returns_Model()
		{
			var generator = new ViewGenerator();
			var sw = new StringWriter();

            var ccu = generator.GenerateController(GetEntityDefinition());

            var cdp = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "3.5" } });
            var opts = new CompilerParameters { GenerateInMemory = true, GenerateExecutable = false };
            var resultAssembly = cdp.CompileAssemblyFromDom(opts, ccu);

            Type controllerType = resultAssembly.CompiledAssembly.GetType("Wms.Controllers.PostController");
		    var controller = Activator.CreateInstance(controllerType, new object[] {});
			var browseAction = controllerType.GetMethods().FirstOrDefault(mi => mi.Name == "Browse");
			var result = browseAction.Invoke(controller, new object[] { }) as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.ViewData.Model);
			Assert.IsInstanceOfType<IEnumerable>(result.ViewData.Model);
		}

		[Test]
		public void Can_Generate_TextBox()
		{
			var pd = new PropertyDefinition("Name") { PropertyType = new TypeDefinition("tString", typeof(string)) };
			string result = ViewGenerator.GetEditControl(pd, false);

            StringAssert.EqualToWhiteSpace(@"<%= Html.TextBox(""Name"") %>", result);

			result = ViewGenerator.GetEditControl(pd, true);

			StringAssert.EqualToWhiteSpace(@"<%= Html.TextBox(""Name"", Model.Name) %>", result);

		}

		[Test]
		public void Can_Generate_CheckBox()
		{
			var pd = new PropertyDefinition("Flag") { PropertyType = new TypeDefinition("tBool", typeof(bool)) };
			var sw = new StringWriter();
			string result = ViewGenerator.GetEditControl(pd, false);

			result = ViewGenerator.GetEditControl(pd, true);

			StringAssert.EqualToWhiteSpace(@"<%=Html.CheckBox(""Flag"",Model.Flag)%>", result);
		}


		private static Type GetControllerType2(string sourceCode, string controllerType)
        {
            return CodeDomGenerator.Compile(null, CodeDomGenerator.Language.CSharp, 
                new string[]{
                    @"C:\WINDOWS\assembly\GAC_MSIL\System.Web.Mvc\1.0.0.0__31bf3856ad364e35\System.Web.Mvc.dll"
                },
                new CodeSnippetCompileUnit(sourceCode)
            ).GetType(controllerType);
        }

        private static EntityDefinition  GetEntityDefinition()
		{
			var ed = new FakeDataFacade().EntityModel.GetEntity("Post");
			var pd = new PropertyDefinition("Flag") { PropertyType = new TypeDefinition("tBoolean", typeof(bool)) };
			ed.AddProperty(pd);
			return ed;
		}
	}
}
