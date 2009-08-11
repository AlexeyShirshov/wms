using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using Wms.Data;
using WXML.Model;
using WXML.CodeDom;
using System.Reflection;
using System.Configuration;

namespace Wms.Repository
{
    public class WmsDataFacade
    {
        private string _path;
        private static IRepositoryProvider _provider;

        public WmsDataFacade(string path)
        {
            _path = path;
        }

        public WmsDataFacade()
        {
            _path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/Meta/");
        }

        public static IQueryable GetEntityQuery(string name)
        {
            string propName = WXMLCodeDomGeneratorNameHelper.GetMultipleForm(name);

            if (_provider == null)
                _provider = GetRepositoryProvider();

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

        public void ApplyModelChanges(WXMLModel changes)
        {
            string fileName = Path.Combine(_path, "entities.xml");
            WXMLModel model = null;
            using (XmlReader xr = new XmlTextReader(fileName))
            {
                model = WXMLModel.LoadFromXml(xr);
            }
            model.Merge(changes);
            string user = "admin";
            if (System.Web.HttpContext.Current != null && !string.IsNullOrEmpty(System.Web.HttpContext.Current.User.Identity.Name))
                user = System.Web.HttpContext.Current.User.Identity.Name;

            File.Copy(fileName, Path.Combine(_path, string.Format(@"/EntityArchive/{0}~{1}.xml", user, DateTime.UtcNow.ToString("d", System.Globalization.CultureInfo.InvariantCulture))));

            File.SetAttributes(fileName, File.GetAttributes(fileName) & ~FileAttributes.ReadOnly);

            XmlDocument xdoc = model.GetXmlDocument();
            xdoc.Save(fileName);
        }

        public void ApplyModelChanges(string script)
        {
            using (StringReader sr = new StringReader(script))
            {
                ApplyModelChanges(WXMLModel.LoadFromXml(new XmlTextReader(sr)));
            }
        }
    }

}
