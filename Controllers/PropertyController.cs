using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Wms.Data;
using Wms.Web.Models;

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
            model.AllowedTypes = DataFacade.EntityModel.Types.Select(t => t.Identifier);
            model.PropertyDefinition =
                DataFacade.EntityModel.GetEntity(type).GetProperties().First(p => p.Identifier == propertyName);
            return model;
        }

    }
}
