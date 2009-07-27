using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wms.Data;
using Wms.Web;

namespace Wms.Repository
{
	public class ListPageRepository : IRepository<IPage>
	{
		private readonly IList<Page> _pageList = new List<Page>();

		public IQueryable<IPage> Items
		{
			get { return _pageList.Cast<IPage>().AsQueryable(); }
		}

		public void Save(IPage entity)
		{
			_pageList.Add( new Page{Name = entity.Name, Url = entity.Url, Contents = entity.Contents} );
		}

		public void Delete(IPage entity)
		{
			var page = _pageList.First(p => p.Name == entity.Name);
			_pageList.Remove(page);
		}
	}
}
