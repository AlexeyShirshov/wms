using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.Web.Models.Admin
{
	public class PageEditorModel
	{
		public IPage Page { get; set; }
		public IEnumerable<IControl> Controls { get; set; }
		public string ControlTemplate { get; set; }
	}
}
