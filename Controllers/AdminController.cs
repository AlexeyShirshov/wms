using System.Web.Routing;
using Wms.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.UI;
using Wms.Data;
using Wms.Web.Data;
using Page = Wms.Web.Page;

namespace Wms.Web.Controllers
{
    public class AdminController : Controller
    {
		private IPageGenerator PageGenerator { get; set; }
    	//Datasources
		public IRepository<IPage> PageRepository { get; set; }

		public AdminController() : this(null, null) { }

		public AdminController(IRepository<IPage> db, IPageGenerator pageGenerator)
		{
			PageRepository = db ?? DataHelper.GetPageRepository();
			PageGenerator = pageGenerator ?? DataHelper.GetPageGenerator();
		}
        
        public ActionResult Index()
        {
            string s = null;
            bool f = s.IsNullOrEmpty();
            return View(PageRepository.Items);
        }

    	

    	public ActionResult Create()
    	{
			return View(new Page());
    	}

		[AcceptVerbs(HttpVerbs.Post)]
    	public ActionResult Create(Page page)
    	{
			if(ValidatePage(page))
			{
    			PageRepository.Save(page);
				PageGenerator.Generate(page);
				RouteTable.Routes.Insert(0, new Route(page.Url, new RouteValueDictionary(new {controller = "Page", action = "Index", page = page.Name}),
					new MvcRouteHandler()));
				return RedirectToAction("Index");
			}
			return View();
    	}

    	public ActionResult Edit(string name)
    	{
			var page = PageRepository.Items.FirstOrDefault(p => p.Name == name);
			if(page != null)
			{
				return View(page);
			}
			throw new HttpException(404, "");
    	}

		private bool ValidatePage(IPage page)
		{
			if(page.Name.IsNullOrEmpty())
			{
				ModelState.AddModelError("Name", "*");
			}

			if(page.Url.IsNullOrEmpty())
			{
				ModelState.AddModelError("Url", "*");
			}

			return ModelState.IsValid;
		}
    }
}
