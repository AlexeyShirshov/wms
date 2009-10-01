using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Wms.Data;

namespace Wms.Web.Controllers
{
	public class WmsController : Controller
	{
		protected IUnityContainer Container;

		public WmsController() : this(null) { }
		
		public WmsController(IUnityContainer container)
		{
			Container = container ?? MvcApplication.Container;
		}
	}
}
