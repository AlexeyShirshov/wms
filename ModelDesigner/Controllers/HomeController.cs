using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXML.Model;

namespace ModelDesigner.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            ViewData["TypesCount"] = Config.WmsRoot.Config.Domains[0].LoadWXMLModel().GetTypes().Count();
            ViewData["ViewModelsCount"] = Config.WmsRoot.Config.ViewModels[0].LoadWXMLModel().GetEntities().Count();


            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
