using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MbUnit.Framework;

namespace Wms.Tests.Misc
{
	public class Item
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
	
	[TestFixture]
	public class Research
	{
        [Test]
		public void Lambda([Column(1)] int id)
        {
			Expression<Func<Item, bool>> l = (Item i) => i.Id == id;

			int x = 10;
			
			Expression<Func<Item, bool>> l1 = (Item i) => i.Id == x;
        }
	}
}
