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
using Microsoft.Practices.Unity;
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
		private FakeDefinitionManager _definitionManager;
		private IUnityContainer _container;
		
		private IUnityContainer GetFakeContainer()
		{
			_definitionManager = new FakeDefinitionManager();
			var container = new UnityContainer();
			container.RegisterInstance<IDefinitionManager>(_definitionManager);
			return container;
		}


		[SetUp]
		public void Setup()
		{
			_container = GetFakeContainer();
			_controller = new EntitiesController(_container);
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
		public void Edit_Definition_Saves()
		{
			var form = new FormCollection
			           	{
							{ "Name", "News" },
							{ "Description", "Test description" }
			           	};
			ActionResult result = _controller.Edit("News", form);

			Assert.IsInstanceOfType<ViewResult>(result);
			Assert.AreEqual(1, _definitionManager.SaveCount);
            
			EntityDefinition d = _definitionManager.EntityModel.GetEntity("News");
            
			Assert.IsNotNull(d);
			Assert.AreEqual("Test description", d.Description);

		}

		[Test]
		public void Edit_Definition_Handles_Non_Existing_Type()
		{
			Assert.Throws<HttpException>(() => _controller.Edit("Nothing"));
		}

		[Test]
		public void Create_Definition_Returns_View()
		{
			var result = _controller.Create();

			Assert.IsInstanceOfType<ViewResult>(result);
		}

		[Test]
		public void Create_Definition_Handles_Existing_Type()
		{
			var result = _controller.Create(new FormCollection{ { "Name", "News" }, { "Description", "Duplicate"}});
			var viewResult = result as ViewResult;

			Assert.IsNotNull(viewResult);
			Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
		}

		[Test]
		public void Create_Definition_Saves()
		{
			int countBefore = _definitionManager.EntityModel.GetEntities().Count();

			var result = _controller.Create(new FormCollection {{ "Name", "NewEntity" }, { "Description", "Test entity" }});

			Assert.IsInstanceOfType<RedirectToRouteResult>(result);
			Assert.IsTrue(_definitionManager.EntityModel.GetActiveEntities().Any(ed => ed.Name == "NewEntity"));
		}


		[Test]
		public void Delete_Definition_Deletes()
		{
			var result = _controller.Delete("News");

			Assert.IsInstanceOfType<RedirectToRouteResult>(result);
            Assert.IsFalse(_definitionManager.EntityModel.GetActiveEntities().Any(e => e.Identifier == "News"), "Entity description not deleted");
		}
		

		[Test]
		public void Delete_Definition_Handles_Non_Existing_Type()
		{
			Assert.Throws<HttpException>(() => _controller.Delete("Nothing"));
		}

	}
}
