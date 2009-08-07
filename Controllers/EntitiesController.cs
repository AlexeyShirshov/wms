using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using WXML.Model;
using IQueryProvider = Wms.Data.IQueryProvider; 

namespace Wms.Web.Controllers
{
    public class EntitiesController : Controller
    {
		public  WXMLModel EntitiesModel { get; set; }
		public IQueryProvider QueryProvider { get; set; }
		
		public EntitiesController() : this(null, null)
		{
			
		}

		public EntitiesController (WXMLModel entitiesModel, IQueryProvider queryProvider)
		{
			EntitiesModel = entitiesModel ?? MvcApplication.Entities;
			QueryProvider = queryProvider ?? new WebQueryProvider();
		}
        
		public ActionResult Index()
        {
            return View(EntitiesModel.ActiveEntities);
        }


    	public ActionResult Browse(string type)
    	{
			return View(QueryProvider.GetEntityQuery(type));
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
