using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WXML.Model;

namespace Wms.Repository
{
    public interface IRepositoryProvider
    {
        object CreateRepository(WXMLModel model);
    }
}
