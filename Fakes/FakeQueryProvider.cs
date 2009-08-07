using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wms.Tests.Fakes
{
	public class FakeQueryProvider : Data.IQueryProvider
	{
		private readonly List<Post> _postList = new List<Post>();

		public FakeQueryProvider()
		{
			_postList.Add(new Post { Id = 1, Title = "Post#1", Text = "This is the first post" });
		}

		public IQueryable GetEntityQuery(string entityName)
		{
			if(String.Equals(entityName, "Post", StringComparison.InvariantCultureIgnoreCase))
				return _postList.AsQueryable();
			throw new InvalidOperationException();
		}

	}
}
