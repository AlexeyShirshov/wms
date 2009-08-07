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
		private WXMLModel _entittiesModel;
		private 
		public EntitiesController() : this(null)
		{
			
		}

		public EntitiesController (WXMLModel entitiesModel)
		{
			_entittiesModel = entitiesModel ?? MvcApplication.Entities;
		}
        
		public ActionResult Index()
        {
            return View();
        }

		public ActionResult BrowseEntity2(string id)
		{
			return View(Wms.Repository.WmsDataFacade.GetEntity(MvcApplication.Entities.ActiveEntities.Single(e => e.Identifier == id).Name));
            //return View(new Wms.Data.WmsRepository().Page);
		}

		public ActionResult BrowseEntity(string id)
		{
			return View(MvcApplication.Entities.ActiveEntities.Single(e => e.Identifier == id));
		}


    	public ActionResult Browse(string type)
    	{
    		throw new NotImplementedException();
    	}

    	public ActionResult Edit(string type)
    	{
    		throw new NotImplementedException();
    	}

    	public ActionResult Edit(string type, int id)
    	{
    		throw new NotImplementedException();
    	}

    	public ActionResult Create()
    	{
    		throw new NotImplementedException();
    	}

    	public ActionResult Create(string type)
    	{
    		throw new NotImplementedException();
    	}

    	public ActionResult Delete(string type, int id)
    	{
    		throw new NotImplementedException();
    	}

    	public ActionResult Delete(string type)
    	{
    		throw new NotImplementedException();
    	}
    }
}
