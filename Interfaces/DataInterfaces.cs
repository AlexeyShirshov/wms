using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wms.Web;

namespace Wms.Data
{
	public interface IRepository<T>
	{
		IQueryable<T> Items { get; }
		void Save(T entity);
		void Delete(T entity);
	}

	public interface IQueryProvider
	{
		IQueryable GetEntityQuery(string entityName);
	}
}
