using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using WXML.Model;
using WXML.CodeDom;
using System.Reflection;
using System.Configuration;

namespace Wms.Repository
{
    public class WmsDataFacade
    {
        private static object _repository;

        public static IQueryable GetEntity(string name)
        {
            string propName = WXMLCodeDomGeneratorNameHelper.GetMultipleForm(name);

            PropertyInfo pi = _repository.GetType().GetProperty(propName);

            return (IQueryable)pi.GetValue(_repository, null);
        }

        public static object GetRepository(string tempPath, WXMLModel model)
        {
            if (_repository == null)
                _repository = CreateRepository(tempPath, model);

            return _repository;
        }

        private static object CreateRepository(string tempPath, WXMLModel model)
        {
            Type repositoryType = Type.GetType(ConfigurationManager.AppSettings["repositoryProvider"]);

            IRepositoryProvider provider = (IRepositoryProvider)Activator.CreateInstance(repositoryType);

            return provider.CreateRepository(tempPath, model);
        }
    }
}
