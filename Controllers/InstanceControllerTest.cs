using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Wms.Data;
using Wms.Tests.Fakes;
using Wms.Web.Controllers;

namespace Wms.Tests.Controllers
{
	[TestFixture]
	public class InstanceControllerTest : ControllerTestBase<InstanceController>
	{
		private IWmsDataFacade _dataFacade;
		[SetUp]
		public void Setup()
		{
			_dataFacade = new FakeDataFacade();
			_controller = new InstanceController(_dataFacade);
		}

		[Test]
		public void Edit_Existing_Returns_Model()
		{
			var result = _controller.Edit("Post", 1.ToString());

			Assert.IsInstanceOfType<ViewResult>(result);

			var model = (result as ViewResult).ViewData.Model;
			Assert.IsInstanceOfType<Post>(model);
			Assert.IsNotNull(model);
		}

		[Test]
		public void Edit_Non_Existing_Throws_404()
		{
			Assert.Throws<HttpException>( () => _controller.Edit("Post", 111.ToString()));
			Assert.Throws<HttpException>( () => _controller.Edit("NonEntity", 111.ToString()));
		}

		[Test]
		public void Edit_Existing_Saves()
		{
			var form = new FormCollection {{"Title", "Unit test"}, {"Text", "Unit test"}};
			var result = _controller.Edit("Post", 1.ToString(), form);

			Assert.IsInstanceOfType<ViewResult>(result);
			Assert.IsNotNull(result);

			var post = (_dataFacade.GetEntityQuery("Post") as IQueryable<Post>).First(p => p.ID == 1);
			Assert.AreEqual("Unit test", post.Text);
			Assert.AreEqual("Unit test", post.Title);
		}


		[Test]
		public void Create_Returns_View()
		{
			var result = _controller.Create("Post");

			Assert.IsInstanceOfType<ViewResult>(result);
			Assert.IsNotNull(result);
		}

		[Test]
		public void Create_Non_Existing_Throws_404()
		{
			Assert.Throws<HttpException>(() => _controller.Create("NonEntity"));
			Assert.Throws<HttpException>(() => _controller.Create("NonEntity", new FormCollection()));
		}
	}
}
