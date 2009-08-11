using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MbUnit.Framework;
using Wms.Repository;
using Wms.Tests.Controllers;
using Wms.Web;
using Wms.Web.Controllers;
using Moq;
using Wms.Web.Models.Admin;

namespace Wms.Tests.Controllers
{
	[TestFixture]
	public class AdminControllerTest
	{
        private ListPageRepository _pageRepository;
		private ListControlRepository _controlRepository;
		private AdminController _controller;
		private Mock<IPageGenerator> _pageGenerator;
		
		[SetUp]
		public void Setup()
		{
			_pageRepository = new ListPageRepository();
			_pageRepository.Save(new Page { Contents = "<html></html>", Name = "News", Url = "/news" });

			_controlRepository = new ListControlRepository();
			_controlRepository.Save(new Control { Name = "Label", Contents = "Unit testing" });

			_pageGenerator = new Mock<IPageGenerator>(MockBehavior.Loose);
			_pageGenerator.Setup(pg => pg.Generate(It.Is<IPage>(p => true), It.Is<TextWriter>(tw => true)));

			_controller = new AdminController(_pageRepository, _controlRepository, _pageGenerator.Object);
			var contextMock = new Mock<ControllerContext>();
			contextMock.Setup(c => c.HttpContext.Request.MapPath(It.Is<String>(s => true))).Returns("test.aspx");
			_controller.ControllerContext = contextMock.Object;
		}

		[Test]
		public void Index_Returns_Pages()
		{
			_controller.PageRepository = _pageRepository;
			var result = _controller.Pages() as ViewResult;

			Assert.IsInstanceOfType(typeof(IEnumerable<IPage>), result.ViewData.Model);
		}

		[Test]
		public void Create_Returns_View_And_Correct_Model()
		{
			var result = _controller.CreatePage() as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(typeof(PageEditorModel), result.ViewData.Model);
		}

		[Test]
		public void Create_Simple_Url_Saves_Changes()
		{
			var page = new Page { Name = "Unit", Contents = "", Url = "unit" };
			var httpContextMock = new Mock<HttpContextBase>();
			httpContextMock.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/unit");
			_controller.CreatePage(page);
			var routeData = _controller.RouteCollection.GetRouteData(httpContextMock.Object);

			Assert.IsTrue(_pageRepository.Items.Any(p => p.Name == "Unit"));
			_pageGenerator.Verify(pg => pg.Generate(It.Is<IPage>(p => true), It.Is<TextWriter>(tw => true)));
			Assert.IsNotNull(routeData);
			Assert.AreEqual("Page", routeData.Values["controller"]);
			Assert.AreEqual("Index", routeData.Values["action"]);
			Assert.AreEqual("Unit", routeData.Values["page"]);
		}

		[Test]
		public void Create_Complex_Url_Saves_Changes()
		{
			var page = new Page { Name = "Unit", Contents = "", Url = "unit/test" };
			var httpContextMock = new Mock<HttpContextBase>();
			httpContextMock.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns("~/unit/test");
	
			_controller.CreatePage(page);
			var routeData = _controller.RouteCollection.GetRouteData(httpContextMock.Object);

			Assert.IsTrue(_pageRepository.Items.Any(p => p.Name == "Unit"));
			_pageGenerator.Verify(pg => pg.Generate(It.Is<IPage>(p => true), It.Is<TextWriter>(tw => true)));
			
			
			Assert.IsNotNull(routeData);
			Assert.AreEqual("Page", routeData.Values["controller"]);
			Assert.AreEqual("Index", routeData.Values["action"]);
			Assert.AreEqual("Unit", routeData.Values["page"]);
		}

		[Test]
		public void Create_Validates_Input()
		{
			var result = _controller.CreatePage(new Page { Name = null, Contents = null, Url = null }) as ViewResult;

			Assert.AreEqual(2, result.ViewData.ModelState.Count);
		}


		[Test]
		public void Edit_Existing_Returns_View_And_Correct_Model()
		{
			var result = _controller.EditPage("News") as ViewResult;
			Assert.IsNotNull(result);

			var model = result.ViewData.Model as PageEditorModel;

			Assert.IsNotNull(model);
			Assert.IsTrue(model.Page.Name == "News");
		}

		[Test]
		public void EditPage_NonExisting_Throws_404()
		{
			Assert.Throws<HttpException>(() => _controller.EditPage("NoPage"));
		}

		[Test]
		public void EditControl_NonExisting_Throws_404()
		{
			Assert.Throws<HttpException>(() => _controller.EditControl("NoControl"));
		}
		


	}
}
