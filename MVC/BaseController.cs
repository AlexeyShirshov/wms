﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Wms.Data;
using Wms.Web;

namespace Wms.Mvc
{
	public abstract class CmsController : Controller
	{
		public IPage Page { get; set; }

	}
}
