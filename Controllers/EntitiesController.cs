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
        //
        // GET: /Entities/

        public ActionResult Index()
        {
            return View(MvcApplication.Entities);
        }

    }
}
