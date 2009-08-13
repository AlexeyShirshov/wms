using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Collections.Specialized;

namespace Wms.Web.Extensions
{
	public static class DebugExtensions
	{
		public static void WriteCollection (NameValueCollection collection)
		{
			for (int i = 0; i < collection.AllKeys.Length; i++)
			{
				Debug.WriteLine(String.Format("{0} : {1}", collection.AllKeys[i], collection[collection.AllKeys[i]]));
			}
		}
	}
}
