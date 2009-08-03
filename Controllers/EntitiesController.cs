using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using WXML.Model;

namespace Wms.Web.Controllers
{
    public class EntitiesController : Controller
    {
        public ActionResult Index()
        {
            return View(MvcApplication.Entities);
        }

		public ActionResult BrowseEntity2(string id)
		{
			return View(Wms.Repository.WmsDataFacade.GetEntity(MvcApplication.Entities.ActiveEntities.Single(e => e.Identifier == id).Name));
		}

		public ActionResult BrowseEntity(string id)
		{
			return View(MvcApplication.Entities.ActiveEntities.Single(e => e.Identifier == id));
		}


    }
}
