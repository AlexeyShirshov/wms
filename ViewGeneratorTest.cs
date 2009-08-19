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
			var sw = new StringWriter();

			generator.GenerateController(GetEntityDefinition(), sw);
            Type controllerType = GetControllerType2(sw.ToString(), "Wms.Controllers.PostController");

            Assert.IsNotNull(controllerType);
			Assert.IsTrue(typeof(Controller).IsAssignableFrom(controllerType));
		}
		
		[Test]
		public void Can_Generate_Browse_Action()
		{
			var generator = new ViewGenerator();
			var sw = new StringWriter();

			generator.GenerateController(GetEntityDefinition(), sw);

			string sourceCode = sw.ToString();
			var cdp = CodeDomProvider.CreateProvider("cs");
			
			Console.WriteLine(sourceCode);

			var resultAssembly  = cdp.CompileAssemblyFromSource(new CompilerParameters(), sourceCode.Split('\n'));
			var controllerType = resultAssembly.CompiledAssembly.GetType("PostController");

			var browseAction = controllerType.GetMethods().FirstOrDefault(mi => mi.Name == "Browse");

			Assert.IsNotNull(browseAction);
			Assert.IsTrue(browseAction.ReturnType.IsAssignableFrom(typeof(ActionResult)));
		}

		[Test]
		public void Browse_Action_Returns_Model()
		{
			var generator = new ViewGenerator();
			var sw = new StringWriter();

			generator.GenerateController(GetEntityDefinition(), sw);

			string sourceCode = sw.ToString();
			var cdp = CodeDomProvider.CreateProvider("cs");
			
			Console.WriteLine(sourceCode);

			var resultAssembly  = cdp.CompileAssemblyFromSource(new CompilerParameters(), sourceCode.Split('\n'));
			var controllerType = resultAssembly.CompiledAssembly.GetType("PostController");
			var controller = Activator.CreateInstance(controllerType);
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


		private static Type GetControllerType(string sourceCode, string controllerType)
		{
            var d = new Dictionary<string, string>();
            d["CompilerVersion"] = "v3.5";
            var cdp = new CSharpCodeProvider(d);

            Console.WriteLine(sourceCode);
            
            var opts = new CompilerParameters();
            //opts.ReferencedAssemblies.Add(typeof(System.Web.Mvc.Controller).Assembly.CodeBase);
            opts.ReferencedAssemblies.Add(@"C:\WINDOWS\assembly\GAC_MSIL\System.Web.Mvc\1.0.0.0__31bf3856ad364e35\System.Web.Mvc.dll");
            opts.ReferencedAssemblies.Add(@"System.dll");
            opts.ReferencedAssemblies.Add(@"System.Core.dll");
            opts.GenerateInMemory = true;
            opts.GenerateExecutable = false;
			var resultAssembly  = cdp.CompileAssemblyFromSource(opts, sourceCode);
			return resultAssembly.CompiledAssembly.GetType(controllerType);
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
