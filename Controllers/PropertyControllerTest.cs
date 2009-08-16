using System;
using System.Collections;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
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
			_controller = new PropertyController(_dataFacade);
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
            var form = new FormCollection() {{"Name", "Identifier"}, {"Type", "Int64"}, {"IsPrimaryKey", "false"}};
            var result = _controller.Edit("Post", "Name", form);

            Assert.IsInstanceOfType<ViewResult>(result);
        }

    }
}
