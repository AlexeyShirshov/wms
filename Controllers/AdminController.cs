using System.IO;
using System.Net.Mime;
using System.Web.Routing;
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
		public IPageGenerator PageGenerator { get; set; }
    	//Datasources
		public IRepository<IPage> PageRepository { get; set; }
		public IRepository<IControl> ControlRepository { get; set; }
    	public RouteCollection RouteCollection
    	{
    		get { return RouteTable.Routes; }
    	}

		public AdminController() : this(null, null, null) { }

		public AdminController(IRepository<IPage> pageRepository, IRepository<IControl> controlRepository, IPageGenerator pageGenerator)
		{
			PageRepository = pageRepository ?? Container.GetPageRepository();
			PageGenerator = pageGenerator ?? Container.GetPageGenerator();
			ControlRepository = controlRepository ?? Container.GetControlRepository();
		}

        public ActionResult BrowseEntity2(string id)
        {
            return View(Wms.Repository.WmsDataFacade.GetEntity(MvcApplication.Entities.ActiveEntities.Single(e => e.Identifier == id).Name));
        }

        public ActionResult BrowseEntity(string id)
        {
            return View(MvcApplication.Entities.ActiveEntities.Single(e=>e.Identifier==id));
        }

        public ActionResult Entities()
        {
            return View(MvcApplication.Entities);
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
				using(var sw = new StreamWriter(Request.MapPath("/Views/Pages/" + page.Name + ".aspx")))
				{
    				PageRepository.Save(page);
					PageGenerator.Generate(page, sw );
					RouteCollection.Insert(0, new Route(page.Url, new RouteValueDictionary(new {controller = "Page", action = "Index", page = page.Name}),
						new MvcRouteHandler()));
					return RedirectToAction("Pages");
				}
			}
			return View();
    	}

    	public ActionResult EditPage(string name)
    	{
			var page = PageRepository.Items.FirstOrDefault(p => p.Name == name);
			if(page != null)
			{
				return View(new PageEditorModel { Page = page, Controls = ControlRepository.Items });
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
				using (var sw = new StreamWriter(ControllerContext.HttpContext.Request.MapPath("/Views/Pages/" + control.Name + ".ascx")))
				{
					ControlRepository.Save(control);
					PageGenerator.Generate(control, sw);
					return RedirectToAction("Controls");
				}
			}
			return View();
		}

		public ActionResult EditControl(string name)
		{
			var control = ControlRepository.Items.FirstOrDefault(c => c.Name == name);
			if (control != null)
				return View(control);
			throw new HttpException(404, "");
		}

		private bool ValidatePage(IPage page)
		{
            if (string.IsNullOrEmpty(page.Name))
			{
				ModelState.AddModelError("Name", "*");
			}

			if(string.IsNullOrEmpty(page.Url))
			{
				ModelState.AddModelError("Url", "*");
			}

			return ModelState.IsValid;
		}

		private bool ValidateControl(IControl control)
		{
            if(string.IsNullOrEmpty(control.Name))
            {
				ModelState.AddModelError("Name", "*");
            }

			if(string.IsNullOrEmpty(control.Contents))
			{
				ModelState.AddModelError("Contents", "*");
			}

			return ModelState.IsValid;
		}

		
    }
}
