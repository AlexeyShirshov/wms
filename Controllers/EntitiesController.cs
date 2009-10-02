using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Microsoft.Practices.Unity;
using Wms.Data;
using Wms.Exceptions;
using Wms.MVC;
using Wms.Web.Extensions;
using Wms.Web.Models.Entities;
using WXML.CodeDom;
using WXML.Model;
using WXML.Model.Descriptors;
using Wms.Repository;

namespace Wms.Web.Controllers
{
    public class EntitiesController : WmsController
    {
		private const string Namespace = "Wms.Entities.User";
		private readonly IDefinitionManager _definitionManager;

		private IEnumerable<TypeDefinition> AllowedTypes
		{
			get
			{
                return _definitionManager.EntityModel.GetTypes();
			}
		}

        public EntitiesController(IUnityContainer container) : base(container)
        {
			_definitionManager = Container.Resolve<IDefinitionManager>();
        }
        
        public EntitiesController() : this(null)
        {
        }

        public ActionResult Index()
        {
            return View(_definitionManager.EntityModel.GetActiveEntities());
        }
		
    	public ActionResult Edit(string entityId)
    	{
			var entityDescription = _definitionManager.EntityModel.GetEntity(entityId);
			if (entityDescription == null)
				throw new HttpException(404, "Entity description not found");
			return View(new EntityDefinitionViewModel { AllowedTypes = AllowedTypes.Select(t => t.ClrType.ToString()) , EntityDefinition = entityDescription } );
    	}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Edit(string entityId, FormCollection form)
		{
			EntityDefinition entityDefinition = _definitionManager.EntityModel.GetEntity(entityId);
			if(entityDefinition == null)
			{
				throw new HttpNotFoundException();
			}
			try
			{
				UpdateModel(entityDefinition, form.ToValueProvider());
				_definitionManager.ApplyModelChanges(_definitionManager.EntityModel);
				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				ModelState.AddModelError("_FORM", e);
				return View(new EntityDefinitionViewModel { AllowedTypes = AllowedTypes.Select(t => t.ClrType.ToString()), EntityDefinition = entityDefinition });
			}
		}

//        private EntityDefinition GetEntityDefinition(string entityId, NameValueCollection form)
//        {
//#if DEBUG
//            DebugExtensions.WriteCollection(form);
//#endif

//            var entityDefinition = new EntityDefinition(entityId, entityId, "Wms.Data.Internal", "", _definitionManager.EntityModel);
//            for (int i = 0; form.AllKeys.Any(k => k.StartsWith(i + ".")); i++ )
//            {
//                var curProp = _definitionManager.EntityModel.GetEntity(entityId).GetProperty(form["propID." + i]);

//                var propertyDefinition = new ScalarPropertyDefinition(entityDefinition, form[i + ".Name"])
//                {
//                    PropertyAlias = curProp.PropertyAlias
//                };

//                if (form[i + ".IsPrimaryKey"].StartsWith("true"))
//                    propertyDefinition.Attributes = Field2DbRelations.PrimaryKey;
//                string typeName = form[i + ".ClrTypeName"];

//                Debug.WriteLine("Type Name =" + typeName);

//                propertyDefinition.PropertyType = AllowedTypes.First(t => t.ClrType.ToString() == typeName);
//                entityDefinition.AddProperty(propertyDefinition);
//            }
//            return entityDefinition;
//        }

    	public ActionResult Create()
    	{
			return View();
    	}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create(FormCollection form)
		{
			try
			{
				var entityName = form["Name"];
				var sourceFragment = new SourceFragmentDefinition("tbl" + entityName, entityName, "Internal" );
				_definitionManager.EntityModel.AddSourceFragment(sourceFragment);
				var ed = new EntityDefinition(entityName, entityName, Namespace);
				ed.AddSourceFragment(new SourceFragmentRefDefinition(sourceFragment));
				_definitionManager.EntityModel.AddEntity(ed);
				_definitionManager.ApplyModelChanges(_definitionManager.EntityModel);
				return RedirectToAction("Index");
			}
			catch(Exception e)
			{
				ModelState.AddModelError("_FORM", e);
				return View();
			}
		}


    	public ActionResult Delete(string entityId)
    	{
			var entityDescription = _definitionManager.EntityModel.GetEntity(entityId);
			if (entityDescription == null)
				throw new HttpNotFoundException("Entity description");
            _definitionManager.EntityModel.RemoveEntity(_definitionManager.EntityModel.GetActiveEntities().First(d => d.Identifier == entityId));
			return RedirectToAction("Index");
    	}



    }
}
