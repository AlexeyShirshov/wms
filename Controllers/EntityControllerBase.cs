using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Wms.Data;
using Wms.Repository;

namespace Wms.Web.Controllers
{
    public class EntityControllerBase : Controller
    {
        public IWmsDataFacade DataFacade { get; private set; }

   		public EntityControllerBase (IWmsDataFacade dataFacade)
		{
			DataFacade = dataFacade ?? new WmsDataFacade();
		}

   		public EntityControllerBase() : this(null)
		{
			
		} 
    }
}
