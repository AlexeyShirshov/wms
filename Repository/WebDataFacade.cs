using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Xml;
using Wms.Data;
using WXML.Model;

namespace Wms.Repository
{
	public class WebDataFacade : IWmsDataFacade
	{
		private readonly WmsDataFacade _dataFacade = new WmsDataFacade();
		private WXMLModel _entityModel;
		public WebDataFacade()
		{
            _entityModel = WXMLModel.LoadFromXml(new XmlTextReader(HostingEnvironment.MapPath(@"~/App_Data/Meta/entities.xml")));
		}
		#region Implementation of IWmsDataFacade

		public IQueryable GetEntityQuery(string entityName)
		{
			return WmsDataFacade.GetEntityQuery(entityName);
		}

		public WXMLModel GetEntityModel()
		{
			return _entityModel;
		}
        
		public void ApplyModelChanges(WXMLModel model)
		{
			_dataFacade.ApplyModelChanges(model);
		}

		#endregion
	}
}
