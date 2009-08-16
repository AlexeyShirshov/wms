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
    public class EntitiesController : EntityControllerBase
    {
		
		private IEnumerable<TypeDefinition> AllowedTypes
		{
			get
			{
                return DataFacade.EntityModel.GetTypes();
			}
		}

        public EntitiesController(IWmsDataFacade dataFacade) : base(dataFacade) { }
        
        public EntitiesController() : this(null)
        {
        }

        public ActionResult Index()
        {
            return View(DataFacade.EntityModel.GetActiveEntities());
        }


    	public ActionResult Browse(string type)
    	{
    		var query = DataFacade.GetEntityQuery(type);
			if (query == null)
				throw new HttpException(404, "Entity type not found");
    		return View(query);
    	}

		[ActionName("EditDefinition")]
    	public ActionResult Edit(string entityId)
    	{
			var entityDescription = DataFacade.EntityModel.GetEntity(entityId);
			if (entityDescription == null)
				throw new HttpException(404, "Entity description not found");
			return View("EditDefinition", new EntityDefinitionViewModel { AllowedTypes = AllowedTypes.Select(t => t.ClrType.ToString()) , EntityDefinition = entityDescription } );
    	}

		[ActionName("EditDefinition")]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Edit(string entityId, FormCollection form)
		{
			EntityDefinition entityDefinition = GetEntityDefinition(entityId, form);

			DataFacade.EntityModel.RemoveEntity(DataFacade.EntityModel.GetEntity(entityId));
			DataFacade.EntityModel.AddEntity(entityDefinition);
			DataFacade.ApplyModelChanges(DataFacade.EntityModel);

			return View("EditDefinition", new EntityDefinitionViewModel { AllowedTypes = AllowedTypes.Select(t => t.ClrType.ToString()), EntityDefinition = entityDefinition });
		}

    	private EntityDefinition GetEntityDefinition(string entityId, NameValueCollection form)
    	{
			DebugExtensions.WriteCollection(form);

    		var entityDefinition = new EntityDefinition(entityId, entityId, "Wms.Data.Internal", "", DataFacade.EntityModel);
    		for (int i = 0; form.AllKeys.Any(k => k.StartsWith(i + ".")); i++ )
    		{
                var curProp = DataFacade.EntityModel.GetEntity(entityId).GetProperty(form["propID." + i]);

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

    	public ActionResult Delete(string entityId)
    	{
			var entityDescription = DataFacade.EntityModel.GetEntity(entityId);
			if (entityDescription == null)
				throw new HttpNotFoundException("Entity description");
            DataFacade.EntityModel.RemoveEntity(DataFacade.EntityModel.GetActiveEntities().First(d => d.Identifier == entityId));
			return RedirectToAction("Index");
    	}
    }
}
