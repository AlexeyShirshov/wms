using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wms.Web;

namespace Wms.Tests
{
	[TestFixture]
	public class HtmlBuilderTest
	{
		[Test]
		public void Handles_No_TextWriter()
		{
			Assert.Throws<ArgumentException>(() => new HtmlBuilder(null));
		}
	}
}
