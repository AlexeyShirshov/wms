using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WXML.Model;

namespace Wms.Repository
{
    public interface IRepositoryProvider
    {
        object CreateRepository();
        void Init(string tempPath, WXMLModel model);
        Type RepositoryType { get; }
    }
}
