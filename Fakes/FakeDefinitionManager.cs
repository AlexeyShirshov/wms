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
	internal class FakeDefinitionManager : IDefinitionManager
	{
		private WXMLModel _model;
		
		public int SaveCount { get; private set; }

		public FakeDefinitionManager()
		{
			using (var reader = XmlReader.Create(@"..\..\Test_Data\TestEntities.xml"))
			{
				_model = WXMLModel.LoadFromXml(reader);
			}
			SaveCount = 0;
		}

		#region Implementation of IWmsDataFacade

		public WXMLModel EntityModel
		{
            get { return _model; }
		}

		public void ApplyModelChanges(WXMLModel model)
		{
			_model = model;
			SaveCount++;
		}

		public void ApplyModelChanges(string script)
		{
			throw new NotImplementedException();
		}

		#endregion
	}

	internal class FakeRepositoryManager : IRepositoryManager
	{
		private readonly List<Post> _postList = new List<Post>();
		
		public FakeRepositoryManager()
		{
			_postList.Add(new Post { ID = 1, Title = "Post#1", Text = "This is the first post" });
		}
		#region Implementation of IRepositoryManager

		public IQueryable GetEntityQuery(string entityName)
		{
			return String.Equals("post", entityName, StringComparison.InvariantCultureIgnoreCase) ? _postList.AsQueryable() : null;
		}

		public IQueryable<T>  GetEntityQuery<T>()
		{
			return GetEntityQuery(typeof(T).Name) as IQueryable<T>;
		}

		#endregion
	}
}
