using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.UI;
using Wms.Proto.Data;
using Page = Wms.Proto.Web.Page;

namespace Wms.Proto.Web.Controllers
{
    public class AdminController : Controller
    {
		//Datasources
		public IRepository<IPage> PageRepository { get; set; }

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View(PageRepository.Items);
        }

    	

    	public ActionResult Create()
    	{
			return View(new Page());
    	}

    	public ActionResult Create(IPage page)
    	{
    		PageRepository.Save(page);
			return RedirectToAction("Index");
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
    }
}
