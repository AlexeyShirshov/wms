using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Wms.Exceptions;
using Wms.Web.Models.Entities;
using WXML.Model;
using WXML.Model.Descriptors;
using Wms.Repository;

namespace Wms.Web.Controllers
{
    public class EntitiesController : Controller
    {
		public WXMLModel EntitiesModel { get; set; }
		//public IQueryProvider QueryProvider { get; set; }
		private static readonly IEnumerable<Type> AllowedTypes = new List<Type> { typeof(Int32), typeof(Int64), typeof(String), typeof(DateTime) };
		
		public EntitiesController() : this(null/*, null*/)
		{
			
		}

		public EntitiesController (WXMLModel entitiesModel/*, IQueryProvider queryProvider*/)
		{
			EntitiesModel = entitiesModel ?? MvcApplication.Entities;
			//QueryProvider = queryProvider ?? new WebQueryProvider();
		}
        
		public ActionResult Index()
        {
            return View(EntitiesModel.ActiveEntities);
        }


    	public ActionResult Browse(string type)
    	{
    		var query = WmsDataFacade.GetEntityQuery(type);
			if (query == null)
				throw new HttpException(404, "Entity type not found");
    		return View(query);
    	}

		[ActionName("EditDefinition")]
    	public ActionResult Edit(string type)
    	{
			var entityDescription = EntitiesModel.GetEntity(type);
			if (entityDescription == null)
				throw new HttpException(404, "Entity description not found");
			return View("EditDescription", new EntityDescriptionViewModel { AllowedTypes = AllowedTypes.Select(t => t.Name) , EntityDescription = entityDescription } );
    	}

		[ActionName("EditDefinition")]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Edit(string type, FormCollection form)
		{
			var entityDefinition = new EntityDefinition(type, type, "Wms.Data.Internal", "", EntitiesModel);
			for (int i = 0; form.AllKeys.Any(k => k.StartsWith(i + ".")); i++ )
			{
				var propertyDefinition = new PropertyDefinition(form[i + ".Name"]);
				if (!String.IsNullOrEmpty(form[i + ".IsPrimaryKey"]))
					propertyDefinition.Attributes = Field2DbRelations.PrimaryKey;
				entityDefinition.AddProperty(propertyDefinition);
			}
			
			EntitiesModel.RemoveEntity(EntitiesModel.GetEntity(type));
			EntitiesModel.AddEntity(entityDefinition);

			return View("EditDescription", new EntityDescriptionViewModel { AllowedTypes = AllowedTypes.Select(t => t.Name), EntityDescription = entityDefinition });
		}


		public ActionResult Edit(string type, int id)
    	{
			return View("EditInstance");
    	}

    	public ActionResult Create()
    	{
			return View("CreateDescription");
    	}

    	public ActionResult Create(string type)
    	{
			return View("CreateInstance");
    	}

    	public ActionResult Delete(string type, int id)
    	{
			return RedirectToAction("Index");
    	}

    	public ActionResult Delete(string type)
    	{
			var entityDescription = EntitiesModel.GetEntity(type);
			if (entityDescription == null)
				throw new HttpNotFoundException("Entity description");
			EntitiesModel.RemoveEntity(EntitiesModel.ActiveEntities.First(d => d.Identifier == type));
			return RedirectToAction("Index");
    	}
    }
}
