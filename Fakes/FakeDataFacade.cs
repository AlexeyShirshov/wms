using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Wms.Data;
using WXML.Model;

namespace Wms.Tests.Fakes
{
	class FakeDataFacade : IWmsDataFacade
	{
		private WXMLModel _model;
		private readonly List<Post> _postList = new List<Post>();
		
		public int SaveCount { get; private set; }

		public FakeDataFacade()
		{
			using (var reader = XmlReader.Create(Path.Combine(TestUtils.TestDataDir, "TestEntities.xml")))
			{
				_model = WXMLModel.LoadFromXml(reader);
			}
			_postList.Add(new Post { Id = 1, Title = "Post#1", Text = "This is the first post" });
			SaveCount = 0;
		}
		#region Implementation of IWmsDataFacade

		public IQueryable GetEntityQuery(string entityName)
		{
			return String.Equals("post", entityName, StringComparison.InvariantCultureIgnoreCase) ? _postList.AsQueryable() : null;
		}

		public WXMLModel GetEntityModel()
		{
			return _model;
		}

		public void ApplyModelChanges(WXMLModel model)
		{
			_model = model;
			SaveCount++;
		}

		#endregion
	}
}
