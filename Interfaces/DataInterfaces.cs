using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wms.Proto.Web;

namespace Wms.Proto.Data
{
	public interface IRepository<T>
	{
		IQueryable<T> Items { get; }
		void Save(T entity);
		void Delete(T entity);
	}
}
