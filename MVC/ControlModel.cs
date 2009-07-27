using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wms.Web;

namespace Wms.Mvc
{
	public class ControlModel : IControlModel
	{
		#region Implementation of IControlModel

		public object Data { get; set; }
		#endregion
	}
}
