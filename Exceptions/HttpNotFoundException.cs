using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Wms.Exceptions
{
	public class HttpNotFoundException : HttpException
	{
		public HttpNotFoundException() : base(404, "") { }

		public HttpNotFoundException(string objectName) : base(404, objectName + " not found") { }
	}
}
