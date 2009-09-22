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
using Wms.Repository;

namespace Wms.Tests
{
	[TestFixture]
	public class ViewGeneratorTest
	{
		private const string AssemblyName = "Test";
		
		[Test]
		public void Can_Generate_Edit_View()
		{
			var generator = new ViewGenerator(TestUtils.FakeContainer);
			var sw = new StringWriter();

			generator.GenerateEditView(GetEntityDefinition(), sw);

			Console.WriteLine(sw.ToString());

			Assert.IsFalse(String.IsNullOrEmpty(sw.GetStringBuilder().ToString()));
		}

		[Test]
		public void Can_Generate_Create_View()
		{
			var generator = new ViewGenerator(TestUtils.FakeContainer);
			var sw = new StringWriter();

			generator.GenerateCreateView(GetEntityDefinition(), sw);

			Console.WriteLine(sw.ToString());

			Assert.IsFalse(String.IsNullOrEmpty(sw.GetStringBuilder().ToString()));
		}


		[Test]
		public void Can_Generate_Controller()
		{
			var generator = new ViewGenerator(TestUtils.FakeContainer);

			var ccu = generator.GenerateController(GetEntityDefinition());

            //var cdp = new CSharpCodeProvider(new Dictionary<string, string> {{"CompilerVersion", "3.5"}});
            //var opts = new CompilerParameters {GenerateInMemory = true, GenerateExecutable = false};
            //var resultAssembly  = cdp.CompileAssemblyFromDom(opts, ccu);

            //Assert.AreEqual(0, resultAssembly.Errors.Count);

			new DefaultClassLoader().Load(ccu, AssemblyName); 
			Type controllerType = Type.GetType("Wms.Controllers.PostController," + AssemblyName );
            Assert.IsNotNull(controllerType);
			Assert.IsTrue(typeof(Controller).IsAssignableFrom(controllerType));
		}


		
		
		[Test]
		public void Generates_Proper_Actions([Column("Index", "Create", "Edit")] string action)
		{
			var generator = new ViewGenerator(TestUtils.FakeContainer);

            var ccu = generator.GenerateController(GetEntityDefinition());

			var cl = new DefaultClassLoader();
			cl.Load(ccu, "Test");

			Type controllerType = Type.GetType("Wms.Controllers.PostController" + "," + AssemblyName );

			var browseAction = controllerType.GetMethods().FirstOrDefault(mi => mi.Name == action);

			Assert.IsNotNull(browseAction, action);
			Assert.IsTrue(browseAction.ReturnType.IsAssignableFrom(typeof(ActionResult)), action);
		}

		[Test]
		public void Browse_Action_Returns_Model()
		{
			var generator = new ViewGenerator(TestUtils.FakeContainer);
			var sw = new StringWriter();

            var ccu = generator.GenerateController(GetEntityDefinition());
			
			new DefaultClassLoader().Load(ccu, AssemblyName);
			Type controllerType = Type.GetType("Wms.Controllers.PostController" + "," + AssemblyName);
			var controller = Activator.CreateInstance(controllerType, new object[] { TestUtils.FakeContainer });


		    //var controller = Activator.CreateInstance(AssemblyName, "Wms.Controllers.PostController", new object[] { TestUtils.FakeContainer } );
			var browseAction = controller.GetType().GetMethods().FirstOrDefault(mi => mi.Name == "Index");
			var result = browseAction.Invoke(controller, new object[] { }) as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.ViewData.Model);
			Assert.IsInstanceOfType<IEnumerable>(result.ViewData.Model);
		}

		[Test]
		public void Can_Generate_TextBox()
		{
            var pd = new ScalarPropertyDefinition(null, "Name") { PropertyType = new TypeDefinition("tString", typeof(string)) };
			string result = ViewGenerator.GetEditControl(pd, false);

            StringAssert.EqualToWhiteSpace(@"<%= Html.TextBox(""Name"") %>", result);

			result = ViewGenerator.GetEditControl(pd, true);

			StringAssert.EqualToWhiteSpace(@"<%= Html.TextBox(""Name"", Model.Name) %>", result);

		}

		[Test]
		public void Can_Generate_CheckBox()
		{
            var pd = new ScalarPropertyDefinition(null, "Flag") { PropertyType = new TypeDefinition("tBool", typeof(bool)) };
			var sw = new StringWriter();
			string result = ViewGenerator.GetEditControl(pd, false);

			result = ViewGenerator.GetEditControl(pd, true);

			StringAssert.EqualToWhiteSpace(@"<%=Html.CheckBox(""Flag"",Model.Flag)%>", result);
		}

        private static EntityDefinition  GetEntityDefinition()
		{
			var ed = new FakeDataFacade().EntityModel.GetEntity("Post");
            var pd = new ScalarPropertyDefinition(null, "Flag") { PropertyType = new TypeDefinition("tBoolean", typeof(bool)) };
			ed.AddProperty(pd);
			return ed;
		}

		[Test]
		public void Supports_Complex_PK()
		{
			Assert.Fail("Write the test!");
		}
	}
}
