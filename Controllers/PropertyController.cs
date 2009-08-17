using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Wms.Data;
using Wms.Exceptions;
using Wms.Web.Models;
using WXML.Model;
using WXML.Model.Descriptors;

namespace Wms.Web.Controllers
{
    public class PropertyController : EntityControllerBase
    {
        public PropertyController(IWmsDataFacade dataFacade) : base(dataFacade)
        {
        }

        public PropertyController() : this(null)
        {
        }

        //
        // GET: /Property/

        public ActionResult Index(string type)
        {
            return View();
        }

        public ActionResult Edit(string entityId, string propertyId)
        {
            return View(GetPropertyDefinitionViewModel(entityId, propertyId));
        }

        private PropertyDefinitionViewModel GetPropertyDefinitionViewModel(string entityId, string propertyId)
        {
            var model = new PropertyDefinitionViewModel();
            model.AllowedTypes = DataFacade.EntityModel.GetTypes().Select(t => t.Identifier);
            model.PropertyDefinition =
                DataFacade.EntityModel.GetEntity(entityId).GetProperties().First(p => p.Identifier == propertyId);
            return model;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string entityId, string propertyId, FormCollection form)
        {
            var ed = DataFacade.EntityModel.GetEntity(entityId);
            
            if(ed == null)
                throw new HttpNotFoundException("entity");

            var property = ed.GetProperties().FirstOrDefault(p => p.Identifier == propertyId);

            if (property == null)
                throw new HttpNotFoundException("property");

            UpdateModel(property, GetValueProvider(form));
            
            try
            {
                DataFacade.ApplyModelChanges(DataFacade.EntityModel);
            }
            catch(Exception e)
            {
                ModelState.AddModelError("_FORM", e.Message);
            }
            return View(GetPropertyDefinitionViewModel(entityId, propertyId));
        }

        private IDictionary<string, ValueProviderResult> GetValueProvider(NameValueCollection form)
        {
            var result = new Dictionary<string, ValueProviderResult>();
            
            result.Add("Attributes", new ValueProviderResult(form["IsPrimaryKey"].StartsWith("true") ? Field2DbRelations.PrimaryKey : Field2DbRelations.None,
                form["IsPrimaryKey"], CultureInfo.CurrentCulture));

            var td = DataFacade.EntityModel.GetTypes().FirstOrDefault(t => t.Identifier == form["Type"]);
            if(td != null)
            {
                result.Add("PropertyType", new ValueProviderResult(td, form["Type"], CultureInfo.CurrentCulture));
            }

			result.Add("Name", new ValueProviderResult(form["Name"], form["Name"], CultureInfo.CurrentCulture));
            
            return result;
        }

        public ActionResult Create(string entityId)
        {
            var ed = DataFacade.EntityModel.GetEntity(entityId);
            if (ed == null)
                throw new HttpNotFoundException("entity");

            return View(new PropertyDefinitionViewModel { EntityDefinition = ed, AllowedTypes = DataFacade.EntityModel.GetTypes().Select(t => t.Identifier)});
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string entityId, FormCollection form)
        {
            var entity = DataFacade.EntityModel.GetEntity(entityId);
            if(entity == null)
                throw new HttpNotFoundException("entity");

            var property = new PropertyDefinition(entity, form["Name"]);

            UpdateModel(property, GetValueProvider(form));

            entity.AddProperty(property);

            try
            {
                DataFacade.ApplyModelChanges(DataFacade.EntityModel);
            }
            catch(Exception e)
            {
                ModelState.AddModelError("_FORM", e.Message);
                return View();
            }
            return RedirectToAction("EditDefinition", "Entities", new {entityId = entity.Identifier});
        }

        public ActionResult Delete(string entityId, string propertyId)
        {
            var entity = DataFacade.EntityModel.GetEntity(entityId);
            
            if (entity == null)
                throw new HttpNotFoundException("entity");

            var property = entity.GetProperties().FirstOrDefault(p => p.Identifier == propertyId);

            if (property == null)
                throw new HttpNotFoundException("property");

            entity.RemoveProperty(property);
            DataFacade.ApplyModelChanges(DataFacade.EntityModel);

            return RedirectToAction("EditDefinition", "Entities", new {entityId = entityId});
        }
    }
}
