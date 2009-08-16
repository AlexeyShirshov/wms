using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wms.Web;

namespace Wms.Tests
{
	[TestFixture]
	public class AspPageGeneratorTest
	{
		[Test]
		public void Can_Generate_Page()
		{
			IPageGenerator generator = new AspPageGenerator();
			var page = new Page { Name = "Test", Url = "test", Contents = "Test page" };
			const string expected = @"<%@ Page Language=""C#"" Inherits=""Wms.Mvc.WmsPage""%>Test page";

		    using (var sw = new StringWriter())
			{
                generator.Generate(page, sw);
				string s = sw.ToString();
                Assert.AreEqual(expected, s);
			}
		}

		[Test]
		public void Can_Generate_Control()
		{
			IPageGenerator generator = new AspPageGenerator();
			var control = new Control { Name = "TestControl", Contents = "Test control" };
			const string expected = @"<%@ Control Language=""C#"" Inherits=""System.Web.Mvc.ViewUserControl<Wms.Web.IControlModel>"" %>Test control";

		    using (var sw = new StringWriter())
			{
				generator.Generate(control, sw);
				string s = sw.ToString();
                Assert.AreEqual(expected, s);
			}
		}


	}
}
