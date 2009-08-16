using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Wms.Data;
using Wms.Mvc;

namespace Wms.Web.Controllers
{
	public class WmsControllerFactory : DefaultControllerFactory
	{
		public override IController CreateController(RequestContext requestContext, string controllerName)
		{
			if (String.Equals(controllerName, "admin", StringComparison.InvariantCultureIgnoreCase)
				|| String.Equals(controllerName, "entities", StringComparison.InvariantCultureIgnoreCase)
                || String.Equals(controllerName, "property", StringComparison.InvariantCultureIgnoreCase))
				return base.CreateController(requestContext, controllerName);

			var controller = base.CreateController(requestContext, "Page") as CmsController;
			controller.Page = Data.Container.GetPageRepository().Items
				.Single(p => p.Name == requestContext.RouteData.Values["page"].ToString());
			
			return controller;
		}
	}
}
