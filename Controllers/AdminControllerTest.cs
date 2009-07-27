using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MbUnit.Framework;
using Wms.Repository;
using Wms.Tests.Controllers;
using Wms.Web;
using Wms.Web.Controllers;

namespace Wms.Tests
{
	[TestFixture]
	[TestsOn("AdminController")]
	public class AdminControllerTest : ControllerTestBase<AdminController>
	{
        private ListPageRepository _pageRepository;
		
		[SetUp]
		public void Setup()
		{
			_pageRepository = new ListPageRepository();
			_pageRepository.Save(new Page { Contents = "<html></html>", Name = "News", Url = "/news" });
			_controller = GetController();
			_controller.PageRepository = _pageRepository;
		}

		[Test]
		public void Index_Returns_Pages()
		{
			_controller.PageRepository = _pageRepository;
			var result = _controller.Pages() as ViewResult;

			Assert.IsInstanceOfType(typeof(IEnumerable<IPage>), result.ViewData.Model);
		}

		[Test]
		public void Create_Returns_Page()
		{
			var result = _controller.CreatePage() as ViewResult;

			Assert.IsInstanceOfType(typeof(IPage), result.ViewData.Model);
		}

		[Test]
		public void Create_Saves_Changes()
		{
			var page = new Page() { Name = "Unit", Contents = "", Url = "/unit" };

			_controller.CreatePage(page);

			Assert.IsTrue(_pageRepository.Items.Any(p => p.Name == "Unit"));
		}

		[Test]
		public void Edit_Existing_Returns_Page()
		{
			var result = _controller.EditPage("News") as ViewResult;

			var page = result.ViewData.Model as IPage;

			Assert.IsNotNull(page);
			Assert.IsTrue(page.Name == "News");
		}

		[Test]
		[ExpectedException(typeof(HttpException))]
		public void Edit_NonExisting_Throws_404()
		{
			_controller.EditPage("Zhopa");
		}


	}
}
