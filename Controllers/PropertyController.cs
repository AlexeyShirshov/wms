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

        public ActionResult Edit(string type, string propertyName)
        {
            return View(GetPropertyDefinitionViewModel(type, propertyName));
        }

        private PropertyDefinitionViewModel GetPropertyDefinitionViewModel(string type, string propertyName)
        {
            var model = new PropertyDefinitionViewModel();
            model.AllowedTypes = DataFacade.EntityModel.GetTypes().Select(t => t.Identifier);
            model.PropertyDefinition =
                DataFacade.EntityModel.GetEntity(type).GetProperties().First(p => p.Identifier == propertyName);
            return model;
        }

        [AcceptVerbs(HttpVerbs.Put)]
        public ActionResult Edit(string type, string propertyName, FormCollection form)
        {
            var ed = DataFacade.EntityModel.GetEntity(type);
            
            if(ed == null)
                throw new HttpNotFoundException("entity");

            var property = ed.GetProperties().FirstOrDefault(p => p.Identifier == propertyName);

            if (property == null)
                throw new HttpNotFoundException("property");

            UpdateModel(property, GetValueProvider(form));

            DataFacade.ApplyModelChanges(DataFacade.EntityModel);

            return View(GetPropertyDefinitionViewModel(type, propertyName));
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
            
            return result;
        }

    }
}
