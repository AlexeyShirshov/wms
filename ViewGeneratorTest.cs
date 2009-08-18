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
			
			Type controllerType = GetControllerType(sw.ToString(), "PostController");

			Assert.IsNotNull(controllerType);
			Assert.IsTrue(typeof(Controller).IsAssignableFrom(controllerType));
		}
		
		[Test]
		public void Can_Generate_Browse_Action()
		{
			var generator = new ViewGenerator();
			var sw = new StringWriter();

			generator.GenerateController(GetEntityDefinition(), sw);

			var controllerType = GetControllerType(sw.ToString(), "PostController");

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

			var controllerType = GetControllerType(sw.ToString(), "PostController");
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

            Assert.AreEqual(@"<%= Html.TextBox(""Name"") %>", result);

		}

		[Test]
		public void Can_Generate_CheckBox()
		{
			var pd = new PropertyDefinition("Flag") { PropertyType = new TypeDefinition("tBool", typeof(bool)) };
			var sw = new StringWriter();
			string result = ViewGenerator.GetEditControl(pd, false);

			Assert.AreEqual(@"<%= Html.CheckBox(""Flag"") %>", result);
		}

		[Test]
		public void Can_Generate_Checkbox()
		{
			
		}

		private static Type GetControllerType(string sourceCode, string controllerType)
		{
			var cdp = CodeDomProvider.CreateProvider("cs");
			
			Console.WriteLine(sourceCode);

			var resultAssembly  = cdp.CompileAssemblyFromSource(new CompilerParameters(), sourceCode.Split('\n'));
			return resultAssembly.CompiledAssembly.GetType(controllerType);
		}

		private static EntityDefinition  GetEntityDefinition()
		{
			var entityDefinition = new FakeDataFacade().EntityModel.GetEntity("Post");
			var pd = new PropertyDefinition("Flag") { PropertyType = new TypeDefinition("tBoolean", typeof(bool)) };
			entityDefinition.AddProperty(pd);
			return entityDefinition;
		}
	}
}
