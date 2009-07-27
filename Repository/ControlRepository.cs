using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wms.Data;
using Wms.Web;

namespace Wms.Repository
{
	public class ListControlRepository : IRepository<IControl>
	{
		private readonly IList<Control> _controlList = new List<Control>();

		public IQueryable<IControl> Items
		{
			get { return _controlList.Cast<IControl>().AsQueryable(); }
		}

		public void Save(IControl entity)
		{
			_controlList.Add(new Control { Name = entity.Name, Contents = entity.Contents });
		}

		public void Delete(IControl entity)
		{
			var Control = _controlList.First(p => p.Name == entity.Name);
			_controlList.Remove(Control);
		}

	}
}
