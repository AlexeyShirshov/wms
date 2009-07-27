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
using Wms.Web.Models.Admin;
using Page = Wms.Web.Page;

namespace Wms.Web.Controllers
{
    public class AdminController : Controller
    {
		private IPageGenerator PageGenerator { get; set; }
    	//Datasources
		public IRepository<IPage> PageRepository { get; set; }
		public IRepository<IControl> ControlRepository { get; set; }

		public AdminController() : this(null, null, null) { }

		public AdminController(IRepository<IPage> pageRepository, IRepository<IControl> controlRepository, IPageGenerator pageGenerator)
		{
			PageRepository = pageRepository ?? Container.GetPageRepository();
			PageGenerator = pageGenerator ?? Container.GetPageGenerator();
			ControlRepository = controlRepository ?? Container.GetControlRepository();
		}

		public ActionResult Index()
		{
			return View();
		}
        
        public ActionResult Pages()
        {
            return View(PageRepository.Items);
        }

		public ActionResult Controls()
		{
			return View(ControlRepository.Items);
		}

    	public ActionResult CreatePage()
    	{
			return View(new PageEditorModel { Page = new Page(), Controls = ControlRepository.Items });
    	}

		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(false)]
    	public ActionResult CreatePage(Page page)
    	{
			if(ValidatePage(page))
			{
    			PageRepository.Save(page);
				PageGenerator.Generate(page);
				RouteTable.Routes.Insert(0, new Route(page.Url, new RouteValueDictionary(new {controller = "Page", action = "Index", page = page.Name}),
					new MvcRouteHandler()));
				return RedirectToAction("Pages");
			}
			return View();
    	}

    	public ActionResult EditPage(string name)
    	{
			var page = PageRepository.Items.FirstOrDefault(p => p.Name == name);
			if(page != null)
			{
				return View(page);
			}
			throw new HttpException(404, "");
    	}

		public ActionResult CreateControl()
		{
			return View(new Control());
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[ValidateInput(false)]
		public ActionResult CreateControl(Control control)
		{
			if(ValidateControl(control))
			{
				ControlRepository.Save(control);
				PageGenerator.Generate(control);
				return RedirectToAction("Controls");
			}
			return View();
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

		private bool ValidateControl(IControl control)
		{
            if(control.Name.IsNullOrEmpty())
            {
				ModelState.AddModelError("Name", "*");
            }

			if(control.Contents.IsNullOrEmpty())
			{
				ModelState.AddModelError("Contents", "*");
			}

			return ModelState.IsValid;
		}

		
    }
}
