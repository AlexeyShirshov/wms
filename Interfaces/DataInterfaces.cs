using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wms.Web;
using WXML.Model;
using WXML.Model.Descriptors;

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

	public interface IEntityService
	{
		EntityDescription GetDefinitionByIdentifier(string identifier);
		void Delete(string identifier);
		void Save(EntityDescription entityDescription);
		IEnumerable<Type> GetAllowedPropertyTypes();
		IEnumerable<EntityDescription> GetEntityDefinitions(int start, int count);
	}
}
