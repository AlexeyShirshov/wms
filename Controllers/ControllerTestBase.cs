using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Wms.Tests.Controllers
{
	public class ControllerTestBase<T> where T: Controller, new()
	{
		protected T _controller;

		protected T GetController()
		{
			return new T();
		}
	}
}
