using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Wms.MVC;
using Wms.Web.Controllers;

namespace Wms.Tests.Controllers
{
	public class ControllerTestBase<T> where T: WmsController, new()
	{
		protected T _controller;

		protected T GetController()
		{
			return Activator.CreateInstance(typeof(T), new[] {TestUtils.FakeContainer }) as T;  
		}

	}
}
