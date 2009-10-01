﻿using System;
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
		void Save(T instance);
		void Delete(T instance);
	}
	public interface IDefinitionManager
	{
        WXMLModel EntityModel { get;  }
		void ApplyModelChanges(WXMLModel model);
		void ApplyModelChanges(string script);
	}

	public interface IRepositoryManager
	{
		IQueryable GetEntityQuery(string entityName);
		IQueryable<T> GetEntityQuery<T>();
	}
}
