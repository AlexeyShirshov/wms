using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Wms.Data;
using Wms.Exceptions;

namespace Wms.Web.Controllers
{
    public class InstanceController : EntityControllerBase
    {
    	public InstanceController(IWmsDataFacade dataFacade) : base(dataFacade)
    	{
    	}

		public InstanceController() : this(null) { }

        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Edit(string entityId, string instanceId )
		{
			var query = DataFacade.GetEntityQuery(entityId);
			if (query == null)
				throw new HttpNotFoundException();

			return View();
		}



    	public ActionResult Create(string entityId)
    	{
    		throw new NotImplementedException();
    	}

    	[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create(string entityId, FormCollection form)
    	{
    		throw new NotImplementedException();
    	}

    	public ActionResult Edit(string entityId, string instanceId, FormCollection form)
    	{
    		throw new NotImplementedException();
    	}
    }
}
