using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wms.Helpers
{
	public static class StringHelper
	{
		public static bool IsNullOrEmpty(this string s)
		{
			return String.IsNullOrEmpty(s);
		}
	}
}
