using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Wms.Proto.Data;
using Wms.Proto.MVC;

namespace Wms.Proto.Web.Controllers
{
	public class CmsControllerFactory : DefaultControllerFactory
	{
		public override IController CreateController(RequestContext requestContext, string controllerName)
		{
			if (controllerName == "admin")
			{
				return base.CreateController(requestContext, controllerName);
			}
			var controller = base.CreateController(requestContext, "Page") as CmsController;
			controller.Page = Data.DataHelper.GetPageRepository().Items.FirstOrDefault(p => p.Name == controllerName);
			
			return controller;
		}
	}
}
