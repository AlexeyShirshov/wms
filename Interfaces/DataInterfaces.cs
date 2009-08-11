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

	public interface IWmsDataFacade
	{
		IQueryable GetEntityQuery(string entityName);
		WXMLModel GetEntityModel();
		void ApplyModelChanges(WXMLModel model);
	}

    //public interface IQueryProvider
    //{
    //    IQueryable GetEntityQuery(string entityName);
    //}

    //public interface IEntityService
    //{
    //    EntityDefinition GetDefinitionByIdentifier(string identifier);
    //    void Delete(string identifier);
    //    void Save(EntityDefinition entityDescription);
    //    IEnumerable<Type> GetAllowedPropertyTypes();
    //    IEnumerable<EntityDefinition> GetEntityDefinitions(int start, int count);
    //}
}
