using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wms.Web
{
	public class Control : IControl
	{
		#region Implementation of IControl

		public string Name { get; set; }
		public string Contents { get; set; }

		#endregion
	}
}
