using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wms.Tests.Fakes
{
    //public class FakeQueryProvider : Data.IQueryProvider
    //{
    //    private readonly List<Post> _postList = new List<Post>();

    //    public FakeQueryProvider()
    //    {
    //        _postList.Add(new Post { Id = 1, Title = "Post#1", Text = "This is the first post" });
    //    }

    //    public IQueryable GetEntityQuery(string entityName)
    //    {
    //        return String.Equals(entityName, "Post", StringComparison.InvariantCultureIgnoreCase) ? _postList.AsQueryable() : null;
    //    }
    //}

    public class FakeRepositoryProvider : Wms.Repository.IRepositoryProvider
    {
        public object CreateRepository()
        {
            return new FakeRepository();
        }

        public void Init(string tempPath, WXML.Model.WXMLModel model)
        {
            throw new NotImplementedException();
        }

        public Type RepositoryType
        {
            get { return typeof (FakeRepository); }
        }

        public Wms.Repository.IModificationTracker CreateTracker()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<System.CodeDom.CodeCompileUnit> CreateCompileUnits(WXML.Model.WXMLModel model)
        {
            throw new NotImplementedException();
        }

        public void SetRepositoryAssembly(System.Reflection.Assembly assembly)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeRepository
    {
        private readonly List<Post> _postList = new List<Post>();

        public FakeRepository()
		{
			_postList.Add(new Post { Id = 1, Title = "Post#1", Text = "This is the first post" });
		}

        public IQueryable<Post> Posts
        {
            get
            {
                return _postList.AsQueryable();
            }
        }
    }
}
