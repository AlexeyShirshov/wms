using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using Wms.Web.Controllers;
using WXML.Model;

namespace Wms.Tests.Controllers
{
	[TestFixture]
	public class EntitiesControllerTest : ControllerTestBase<EntitiesController>
	{
		private WXMLModel _model;

		[SetUp]
		public void Setup()
		{
			_controller = GetController();
			_model = new WXMLModel();
		}

		[Test]
		public void Can_List_Instances()
		{
			var result = _controller.Browse("News") as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<IEnumerable>(result.ViewData.Model);
			Assert.IsNotNull(result.ViewData.Model);
		}

		[Test]
		public void Can_List_Definitions()
		{
			var result = _controller.Index() as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsInstanceOf<IEnumerable>(result.ViewData.Model);
			Assert.IsNotNull(result.ViewData.Model);
		}

		[Test]
		public void Edit_Definition_Returns_Model()
		{
			var result = _controller.Edit("Post") as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.ViewData.Model);
		}

		[Test]
		public void Edit_Instance_Returns_Model()
		{
			var result = _controller.Edit("News", 1) as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.ViewData.Model);
		}

		[Test]
		public void Create_Definition_Returns_Model()
		{
			var result = _controller.Create() as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.ViewData.Model);
		}

		[Test]
		public void Create_Instance_Returns_Model()
		{
			var result = _controller.Create("News") as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.ViewData.Model);
		}

		[Test]
		public void Delete_Instance_Redirects()
		{
			var result = _controller.Delete("News", 1) as RedirectResult;

			Assert.IsNotNull(result);
		}

		[Test]
		public void Delete_Definition_Redirects()
		{
			var result = _controller.Delete("News") as RedirectResult;

			Assert.IsNotNull(result);
		}
	}
}
