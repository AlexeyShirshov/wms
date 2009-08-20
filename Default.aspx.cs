﻿using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Text;
using System.IO;
using WXML.Model.Descriptors;
using WXML.Model;

namespace Wms.Web
{
	public partial class _Default : System.Web.UI.Page
	{
		public void Page_Load(object sender, System.EventArgs e)
		{
            // Change the current path so that the Routing handler can correctly interpret
			// the request, then restore the original path so that the OutputCache module
			// can correctly process the response (if caching is enabled).

			string originalPath = Request.Path;
			HttpContext.Current.RewritePath(Request.ApplicationPath, false);
			IHttpHandler httpHandler = new MvcHttpHandler();
			httpHandler.ProcessRequest(HttpContext.Current);
			HttpContext.Current.RewritePath(originalPath, false);
		}
	}
}
