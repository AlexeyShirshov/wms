using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wms.Proto.Web
{
	public class Page : IPage
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public string Contents { get; set; }
		public IEnumerable<IControl> Controls
		{
			get { throw new NotImplementedException(); }
		}
	}
}
