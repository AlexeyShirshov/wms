using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.IO;
using WXML.Model;

namespace Config
{
    public class ModelManager
    {
        class ModelEdit
        {
            public string User { get; internal set; }
            public DateTime Date { get; internal set; }
            public WXMLModel Model { get; internal set; }

            //public override bool Equals(object obj)
            //{
            //    return Equals(obj as ModelEdit);
            //}

            //public bool Equals(ModelEdit me)
            //{
            //    if (me == null)
            //        return false;

            //    return Model.Name == me.Model.Name;
            //}

            //public override int GetHashCode()
            //{
            //    return Model.Name.GetHashCode();
            //}
        }

        private static object _editLock = new object();

        public static bool CanEditModelStore(ModelStore ms)
        {
            ModelEdit e = (ModelEdit)HttpContext.Current.Application[ms.Name];
            if (e != null)
            {
                return e.User == HttpContext.Current.User.Identity.Name;
            }
            return true;
        }

        public static WXMLModel OpenModelStoreForEdit(ModelStore ms)
        {
            ModelEdit e = (ModelEdit)HttpContext.Current.Application[ms.Name];
            if (e == null)
            {
                lock (_editLock)
                {
                    e = (ModelEdit)HttpContext.Current.Application[ms.Name];
                    if (e == null)
                    {
                        e = new ModelEdit
                        {
                            User = HttpContext.Current.User.Identity.Name,
                            Date = DateTime.Now,
                            Model = ms.LoadWXMLModel()
                        };
                        HttpContext.Current.Application[ms.Name] = e;
                    }
                }
            }

            if (e.User != HttpContext.Current.User.Identity.Name)
                throw new ApplicationException();

            return e.Model;
        }

        public static void SaveModelStore(ModelStore ms)
        {
            ModelEdit e = (ModelEdit)HttpContext.Current.Application[ms.Name];
            if (e != null)
            {
                lock (_editLock)
                {
                    e = (ModelEdit)HttpContext.Current.Application[ms.Name];
                    if (e != null)
                    {
                        FileAttributes attr = File.GetAttributes(ms.Location);
                        if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            File.SetAttributes(ms.Location, attr & ~FileAttributes.ReadOnly);

                        XmlDocument xdoc = e.Model.GetXmlDocument();
                        xdoc.Save(ms.Location);

                        HttpContext.Current.Application.Remove(ms.Name);
                    }
                }
            }
        }
    }
}
