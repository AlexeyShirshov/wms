using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Wms.Proto.MVC;

namespace Wms.Proto.Web.Controllers
{
    public class PageController : CmsController
    {
        //
        // GET: /Default/

        public ActionResult Index()
        {
            return Content(Page.Contents);
        }

    }
}
