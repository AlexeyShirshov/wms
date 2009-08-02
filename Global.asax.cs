using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Wms.Web.Controllers;
using WXML.Model;
using System.Xml;
using System.Web.Hosting;

namespace Wms.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
        private static WXMLModel _model;

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
				"Default",                                              // Route name
				"{controller}/{action}/{id}",                           // URL with parameters
				new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
			);

            //routes.MapRoute(
            //    "Entities",                                              // Route name
            //    "/Admin/{controller}/{action}/{id}",                           // URL with parameters
            //    new { controller = "Entities", action = "Index", id = "" }  // Parameter defaults
            //);
        }

		protected void Application_Start()
		{
			RegisterRoutes(RouteTable.Routes);
			ControllerBuilder.Current.SetControllerFactory(typeof(WmsControllerFactory));

            _model = WXMLModel.LoadFromXml(new XmlTextReader(HostingEnvironment.MapPath(@"~/App_Data/Meta/entities.xml")));

            Wms.Repository.WmsDataFacade.GetRepository(_model);
		}

        public static WXMLModel Entities
        {
            get
            {
                return _model;
            }
        }
	}
}
