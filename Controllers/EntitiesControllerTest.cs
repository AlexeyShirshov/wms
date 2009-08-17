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
using Wms.Data;
using Wms.Tests.Fakes;
using Wms.Web.Controllers;
using Wms.Web.Models.Entities;
using WXML.Model;
using WXML.Model.Descriptors;

namespace Wms.Tests.Controllers
{
	// ReSharper disable InconsistentNaming

	[TestFixture]
	public class EntitiesControllerTest : ControllerTestBase<EntitiesController>
	{
		private FakeDataFacade _dataFacade;

		private ControllerContext GetFakeControllerContext(ControllerBase controller)
		{
			var httpContextMock = new Mock<HttpContextBase>();
			return new ControllerContext(httpContextMock.Object, new RouteData(), controller);
		}

		[SetUp]
		public void Setup()
		{
			_dataFacade = new FakeDataFacade();
			_controller = new EntitiesController(_dataFacade);
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

            Assert.IsInstanceOfType<IEnumerable<EntityDefinition>>(result.ViewData.Model);
			Assert.IsNotNull(result.ViewData.Model);

            var edList = model as IEnumerable<EntityDefinition>;

			Assert.GreaterThan(edList.Count(), 0);
		}

		[Test]
		public void Edit_Definition_Returns_Model()
		{
			var result = _controller.Edit("Post") as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.ViewData.Model);

			var ed = result.ViewData.Model as EntityDefinitionViewModel;

			Assert.IsNotNull(ed);
			Assert.AreEqual("Post", ed.EntityDefinition.Identifier);

			Assert.GreaterThan(ed.AllowedTypes.Count(), 0);
		}

		[Test]
		public void Edit_Definition_Saves([Column("Post", "News")] string entityType,
		                                  [Column("Id", "Ident")] string propertyId)
		{
			var form = new FormCollection
			           	{
			           		{"PropID.0", propertyId},
                            {"0.Name", propertyId},
			           		{"0.ClrTypeName", "Int32"},
			           		{"0.IsPrimaryKey", "true,false"},
                            {"PropID.1", propertyId},
			           		{"1.Name", "Title"},
			           		{"1.ClrTypeName", "String"},
			           		{"1.IsPrimaryKey", ""}
			           	};
			ActionResult result = _controller.Edit(entityType, form);

			Assert.IsInstanceOfType<ViewResult>(result);

			Assert.AreEqual(1, _dataFacade.SaveCount);

            EntityDefinition d = _dataFacade.EntityModel.GetEntity(entityType);

			Assert.IsNotNull(d);
			Assert.AreEqual(2, d.GetActiveProperties().Count());
			Assert.AreEqual(propertyId, d.GetPkProperties().Single().Name);
			Assert.AreEqual("Title", d.GetActiveProperties().Skip(1).First().Name);
			Assert.AreEqual(typeof(Int32), d.GetActiveProperties().First().PropertyType.ClrType);

		}

		[Test]
		public void Edit_Definition_Saves_With_No_PK([Column("Post", "News")] string entityType, [Column("Id", "Ident")] string propertyName)
		{
			var form = new FormCollection { { "0.Name", propertyName }, { "0.Type", "Int32" }, { "0.IsPrimaryKey", "false" }, { "0.ClrTypeName", "Int32" } };

			var result = _controller.Edit(entityType, form);

			Assert.IsInstanceOfType<ViewResult>(result);

			Assert.AreEqual(1, _dataFacade.SaveCount);

			var d = _dataFacade.EntityModel.GetEntity(entityType);

			Assert.IsNotNull(d);
			Assert.AreEqual(1, d.GetActiveProperties().Count());
			Assert.IsNull(d.GetPkProperties().SingleOrDefault());
		}

		[Test]
		public void Edit_Definition_Handles_Non_Existing_Type()
		{
			Assert.Throws<HttpException>(() => _controller.Edit("Nothing"));
		}

		[Test]
		public void Edit_Instance_Returns_Model()
		{
			var result = _controller.EditInstance("Post", 1) as ViewResult;

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
			var result = _controller.CreateInstance("Post") as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.ViewData.Model);
		}

		[Test]
		public void Delete_Instance_Redirects()
		{
			var result = _controller.DeleteInstance("Post", 2) as RedirectToRouteResult;

			Assert.IsNotNull(result);

			result.ExecuteResult(GetFakeControllerContext(_controller));

			Assert.AreEqual("Entities", result.RouteValues["controller"]);
			Assert.AreEqual("Browse", result.RouteValues["action"]);
			Assert.AreEqual("Post", result.RouteValues["type"]);
		}

		[Test]
		public void Delete_Definition_Deletes()
		{
			var result = _controller.Delete("News");

			Assert.IsInstanceOfType<RedirectToRouteResult>(result);
            Assert.IsFalse(_dataFacade.EntityModel.GetActiveEntities().Any(e => e.Identifier == "News"), "Entity description not deleted");

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
