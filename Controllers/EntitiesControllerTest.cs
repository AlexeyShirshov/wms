using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using MbUnit.Framework;
using Moq;
using Wms.Tests.Fakes;
using Wms.Web.Controllers;
using WXML.Model;
using WXML.Model.Descriptors;

namespace Wms.Tests.Controllers
{
	[TestFixture]
	public class EntitiesControllerTest : ControllerTestBase<EntitiesController>
	{
		private WXMLModel _model;

		[SetUp]
		public void Setup()
		{
			using (var reader = XmlReader.Create(Path.Combine(TestUtils.TestDataDir, "TestEntities.xml")))
			{
				_model = WXMLModel.LoadFromXml(reader);
			}
			_controller = new EntitiesController(_model, new FakeQueryProvider());
		}

		[Test]
		public void Can_List_Instances()
		{

			var result = _controller.Browse("Post") as ViewResult;

			Assert.IsNotNull(result);

			var model = result.ViewData.Model;

			Assert.IsNotNull(model);
			Assert.IsInstanceOfType<IEnumerable>(model);

			int count = 0;
			foreach(var o in model as IEnumerable)
				count++;

			Assert.IsTrue(count > 0);
		}

		[Test]
		public void Instance_List_Handles_Non_Existing_Type()
		{
			Assert.Throws<HttpException>(() => _controller.Browse("Nothing"));
		}

		[Test]
		public void Can_List_Definitions()
		{
			var result = _controller.Index() as ViewResult;

			Assert.IsNotNull(result);

			var model = result.ViewData.Model;

			Assert.IsInstanceOfType<IEnumerable<EntityDescription>>(result.ViewData.Model);
			Assert.IsNotNull(result.ViewData.Model);

			var edList = model as IEnumerable<EntityDescription>;

			Assert.GreaterThan(edList.Count(), 0);
		}

		[Test]
		public void Edit_Definition_Returns_Model()
		{
			var result = _controller.Edit("Post") as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.ViewData.Model);

			var ed = result.ViewData.Model as EntityDescription;

			Assert.IsNotNull(ed);
			Assert.AreEqual("Post", ed.Identifier);
		}

		[Test]
		public void Edit_Definition_Handles_Non_Existing_Type()
		{
			Assert.Throws<HttpException>(() => _controller.Edit("Nothing"));
		}

		[Test]
		public void Edit_Instance_Returns_Model()
		{
			var result = _controller.Edit("Post", 1) as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.ViewData.Model);
		}

		[Test]
		public void Create_Definition_Returns_View()
		{
			var result = _controller.Create();

			Assert.IsInstanceOfType<ViewResult>(result);
		}

		[Test]
		public void Create_Instance_Returns_Model()
		{
			var result = _controller.Create("Post") as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.ViewData.Model);
		}

		[Test]
		public void Delete_Instance_Redirects()
		{
			var result = _controller.Delete("Post", 2) as RedirectToRouteResult;

			Assert.IsNotNull(result);

			result.ExecuteResult(GetFakeControllerContext(_controller));

			Assert.AreEqual("Entities", result.RouteValues["controller"]);
			Assert.AreEqual("Browse", result.RouteValues["action"]);
			Assert.AreEqual("Post", result.RouteValues["type"]);
		}

		private ControllerContext GetFakeControllerContext(ControllerBase controller)
		{
			var httpContextMock = new Mock<HttpContextBase>();
			return new ControllerContext(httpContextMock.Object, new RouteData(), controller);
		}

		[Test]
		public void Delete_Definition_Deletes()
		{
			var result = _controller.Delete("News") as RedirectToRouteResult;

			Assert.IsFalse(_model.ActiveEntities.Any(e => e.Identifier == "News"), "Entity description not deleted");

			//result.ExecuteResult(GetFakeControllerContext(_controller));
			//Assert.IsNotNull(result);
			//Assert.AreEqual("Entities", result.RouteValues["controller"]);
			//Assert.AreEqual("Index", result.RouteValues["action"]);
		}
		

		[Test]
		public void Delete_Definition_Handles_Non_Existing_Type()
		{
			Assert.Throws<HttpException>(() => _controller.Delete("Nothing"));
		}
	}
}
