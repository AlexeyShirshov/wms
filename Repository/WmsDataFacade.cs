using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Wms.Data;
using WXML.Model;
using WXML.CodeDom;
using System.Reflection;
using System.Configuration;

namespace Wms.Repository
{
    public class WmsDataFacade
    {
        private static IRepositoryProvider _provider;

        public static IQueryable GetEntityQuery(string name)
        {
            string propName = WXMLCodeDomGeneratorNameHelper.GetMultipleForm(name);

            PropertyInfo pi = _provider.RepositoryType.GetProperty(propName);

            return (IQueryable)pi.GetValue(GetRepository(), null);
        }

        public static object GetRepository()
        {
            return _provider.CreateRepository();
        }

        public static IRepositoryProvider GetRepositoryProvider(string tempPath, WXMLModel model)
        {
            if (_provider == null)
            {
                Type pt = Type.GetType(ConfigurationManager.AppSettings["repositoryProvider"]);

                _provider = (IRepositoryProvider)Activator.CreateInstance(pt);

                _provider.Init(tempPath, model);
            }

            return _provider;
        }

        public static IRepositoryProvider GetRepositoryProvider()
        {
            if (_provider == null)
            {
                Type pt = Type.GetType(ConfigurationManager.AppSettings["repositoryProvider"]);

                _provider = (IRepositoryProvider)Activator.CreateInstance(pt);
            }

            return _provider;
        }

        internal static void SetRepositoryProvider(Type t)
        {
            Type pt = Type.GetType(ConfigurationManager.AppSettings["repositoryProvider"]);

            _provider = (IRepositoryProvider)Activator.CreateInstance(pt, (object)t);
        }
    }

}
