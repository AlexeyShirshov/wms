using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Microsoft.Practices.Unity;
using Wms.Data;
using Wms.Tests.Fakes;
using Wms.Web.Models;
using Wms.Web.Controllers;
using Moq;
using WXML.Model.Descriptors;

namespace Wms.Tests.Controllers
{
    [TestFixture]
    public class PropertyControllerTest : ControllerTestBase<PropertyController>
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
			_controller = new PropertyController(_container);
		}

        [Test]
        public void Edit_Existing_Returns_Model()
        {
            var result = _controller.Edit("Post", "ID");

            Assert.IsInstanceOfType<ViewResult>(result);

            var viewResult = result as ViewResult;

            Assert.IsInstanceOfType<PropertyDefinitionViewModel>(viewResult.ViewData.Model);
            Assert.IsNotNull(viewResult.ViewData.Model);
        }

        [Test]
        public void Edit_Saves_Property()
        {
            var form = new FormCollection {{"Name", "Identifier"}, {"Type", "Int64"}, {"IsPrimaryKey", "false"}};
            var result = _controller.Edit("Post", "Url", form);

            Assert.IsInstanceOfType<ViewResult>(result);
            Assert.AreEqual(1, _definitionManager.SaveCount);
			Assert.AreEqual("Identifier", _definitionManager.EntityModel.GetEntity("Post").GetProperty("Url").Name);
		}

        [Test]
        public void Create_Returns_View()
        {
            var result = _controller.Create("Post");

            Assert.IsInstanceOfType<ViewResult>(result);
            Assert.IsNotNull(result);

            var viewResult = result as ViewResult;
            var model = viewResult.ViewData.Model;

            Assert.IsInstanceOfType<PropertyDefinitionViewModel>(model);
            Assert.IsNotNull(model);

            var pdvModel = model as PropertyDefinitionViewModel;

            Assert.IsNotNull(pdvModel.EntityDefinition);
            Assert.GreaterThan(pdvModel.AllowedTypes.Count(), 0);
        }

        [Test]
        public void Create_Saves_Property_And_Redirects()
        {
            var form = new FormCollection {{"Name", "Area"}, {"Type", "tString"}, {"IsPrimaryKey", "false"}};
            var result = _controller.Create("Post", form);

            Assert.IsInstanceOfType<RedirectToRouteResult>(result);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, _definitionManager.SaveCount);
        }


    }
}
