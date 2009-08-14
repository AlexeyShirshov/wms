using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Wms.Data;
using Wms.Exceptions;
using Wms.Web.Extensions;
using Wms.Web.Models.Entities;
using WXML.Model;
using WXML.Model.Descriptors;
using Wms.Repository;

namespace Wms.Web.Controllers
{
    public class EntitiesController : Controller
    {
		public IWmsDataFacade DataFacade { get; set; }

        private WXMLModel EntitiesModel
        {
            get
            {
                return DataFacade.GetEntityModel();
            }
        }

		public EntitiesController() : this(null)
		{
			
		}
		
		private IEnumerable<TypeDefinition> AllowedTypes
		{
			get
			{
				return DataFacade.GetEntityModel().Types;
			}
		}

		public EntitiesController (IWmsDataFacade dataFacade)
		{
			DataFacade = dataFacade ?? new WebDataFacade();
		}
        
		public ActionResult Index()
        {
            return View(DataFacade.GetEntityModel().ActiveEntities);
        }


    	public ActionResult Browse(string type)
    	{
    		var query = DataFacade.GetEntityQuery(type);
			if (query == null)
				throw new HttpException(404, "Entity type not found");
    		return View(query);
    	}

		[ActionName("EditDefinition")]
    	public ActionResult Edit(string type)
    	{
			var entityDescription = DataFacade.GetEntityModel().GetEntity(type);
			if (entityDescription == null)
				throw new HttpException(404, "Entity description not found");
			return View("EditDefinition", new EntityDescriptionViewModel { AllowedTypes = AllowedTypes.Select(t => t.ClrType.ToString()) , EntityDescription = entityDescription } );
    	}

		[ActionName("EditDefinition")]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Edit(string type, FormCollection form)
		{
			EntityDefinition entityDefinition = GetEntityDefinition(type, form);

			DataFacade.GetEntityModel().RemoveEntity(DataFacade.GetEntityModel().GetEntity(type));
			DataFacade.GetEntityModel().AddEntity(entityDefinition);
			DataFacade.ApplyModelChanges(DataFacade.GetEntityModel());

			return View("EditDefinition", new EntityDescriptionViewModel { AllowedTypes = AllowedTypes.Select(t => t.ClrType.ToString()), EntityDescription = entityDefinition });
		}

    	private EntityDefinition GetEntityDefinition(string type, NameValueCollection form)
    	{
			DebugExtensions.WriteCollection(form);

    		var entityDefinition = new EntityDefinition(type, type, "Wms.Data.Internal", "", DataFacade.GetEntityModel());
    		for (int i = 0; form.AllKeys.Any(k => k.StartsWith(i + ".")); i++ )
    		{
                var curProp = DataFacade.GetEntityModel().GetEntity(type).GetProperty(form["propID." + i]);
                var propertyDefinition = new PropertyDefinition(form[i + ".Name"])
                {
                    PropertyAlias = curProp.PropertyAlias
                };

    			if (form[i + ".IsPrimaryKey"].StartsWith("true"))
    				propertyDefinition.Attributes = Field2DbRelations.PrimaryKey;
    			string typeName = form[i + ".ClrTypeName"];

    			Debug.WriteLine("Type Name =" + typeName);

				propertyDefinition.PropertyType = AllowedTypes.First(t => t.ClrType.ToString() == typeName);
    			entityDefinition.AddProperty(propertyDefinition);
    		}
    		return entityDefinition;
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
			var entityDescription = DataFacade.GetEntityModel().GetEntity(type);
			if (entityDescription == null)
				throw new HttpNotFoundException("Entity description");
			DataFacade.GetEntityModel().RemoveEntity(DataFacade.GetEntityModel().ActiveEntities.First(d => d.Identifier == type));
			return RedirectToAction("Index");
    	}
    }
}
