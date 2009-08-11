using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wms.Repository;
using System.IO;
using WXML.Model;

namespace Wms.Tests
{
    [TestFixture]
    public class WmsFacadeTest
    {
        [Test]
        public void СhangeEntities()
        {
            string root = Path.GetFullPath(Environment.CurrentDirectory + @"..\..\..\Repository\Meta");

            WmsDataFacade f = new WmsDataFacade(root);

            WXMLModel model = new WXMLModel();

            f.ApplyModelChanges(model);
        }
    }
}
