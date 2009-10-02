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
		private readonly List<PostToTag> _postToTagList = new List<PostToTag>();

		public FakeRepositoryManager()
		{
			_postList.Add(new Post { ID = 1, Title = "Post#1", Text = "This is the first post" });
			_postToTagList.Add(new PostToTag() { PostId = 1, TagId = 2 });
		}
		#region Implementation of IRepositoryManager

		public IQueryable GetEntityQuery(string entityName)
		{
			switch (entityName.ToLower())
			{
				case "post":
					return _postList.AsQueryable();
				case "posttotag":
					return _postToTagList.AsQueryable();
				default:
					return null;

			}
		}

		public IQueryable<T>  GetEntityQuery<T>()
		{
			return GetEntityQuery(typeof(T).Name) as IQueryable<T>;
		}

		#endregion

		#region Implementation of IDisposable

		public void Dispose()
		{
		}

		#endregion

		#region Implementation of IModificationTracker

		public void Add(params object[] entities)
		{
			foreach (var o in entities)
			{
				if(o is Post)
					_postList.Add(o as Post);

				if(o is PostToTag)
					_postToTagList.Add(o as PostToTag);
			}
		}

		public void Delete(params object[] entities)
		{
			foreach (var o in entities)
			{
				if (o is Post)
					_postList.Remove(o as Post);

				if (o is PostToTag)
					_postToTagList.Remove(o as PostToTag);
			}
		}

		public void AcceptModifications()
		{
		}

		#endregion
	}
}
