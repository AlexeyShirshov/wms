using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Wms.Web;

namespace Wms.Web.Extensions
{
	public static class HtmlExtension
	{
		public static string PageEditLink(this HtmlHelper html, IPage page)
		{
			return html.ActionLink(page.Name,  "Edit", "Admin", new { id = page.Name }, null);
		}
	}
}
