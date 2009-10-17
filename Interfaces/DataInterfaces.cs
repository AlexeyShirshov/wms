using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wms.Repository;
using Wms.Web;
using WXML.Model;
using WXML.Model.Descriptors;

namespace Wms.Data
{
    [Obsolete]
	public interface IRepository<T>
	{
		IQueryable<T> Items { get; }
		void Save(T instance);
		void Delete(T instance);
	}
	public interface IDefinitionManager
	{
        WXMLModel EntityModel { get;  }
		void ApplyModelChanges(WXMLModel model);
		void ApplyModelChanges(string script);
	}

    [Obsolete]
	public interface IRepositoryManager : IModificationTracker
	{
		IQueryable GetEntityQuery(string entityName);
		IQueryable<T> GetEntityQuery<T>();
	}
}
