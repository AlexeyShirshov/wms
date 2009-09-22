using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wms.Data;
using WXML.Model;

namespace Wms.Repository
{
	public class WebDataFacade : IWmsDataFacade
	{
		private static readonly WmsDataFacade _dataFacade = new WmsDataFacade();
		#region Implementation of IWmsDataFacade

		public IQueryable GetEntityQuery(string entityName)
		{
			return WmsDataFacade.GetEntityQuery(entityName);
		}

		public WXMLModel EntityModel
		{
			get { return _dataFacade.EntityModel; }
		}

		public void ApplyModelChanges(WXMLModel model)
		{
			_dataFacade.ApplyModelChanges(model);
		}

		#endregion
	}
}
