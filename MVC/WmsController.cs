using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace Wms.MVC
{
	public class WmsController : Controller
	{
		protected readonly IUnityContainer Container;

		public WmsController(IUnityContainer container)
		{
			if (container == null)
				throw new ArgumentNullException("container");

			Container = container;
		}
	}
}
