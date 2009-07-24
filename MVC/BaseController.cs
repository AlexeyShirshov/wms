using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Wms.Proto.Data;
using Wms.Proto.Web;

namespace Wms.Proto.MVC
{
	public abstract class CmsController : Controller
	{
		public IPage Page { get; set; }

	}
}
