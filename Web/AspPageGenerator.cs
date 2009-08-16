using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Wms.Web;

namespace Wms.Web
{
	public class AspPageGenerator : IPageGenerator
	{
		private readonly string _rootFolder;



		#region Implementation of IPageGenerator
		public void Generate(IPage page, TextWriter writer)
		{
			if (page == null)
				throw new ArgumentNullException("page");
			if (writer == null)
				throw new ArgumentNullException("writer");
			
			writer.Write(GetAsp(page));
		}

		public void Generate(IControl control, TextWriter writer)
		{
			if (control == null)
				throw new ArgumentNullException("control");
			if (writer == null)
				throw new ArgumentNullException("writer");

			writer.Write(GetAsp(control));
		}

		private static string GetAsp(IPage page)
		{
			return @"<%@ Page Language=""C#"" Inherits=""Wms.Mvc.WmsPage""%>" + page.Contents;
		}

		private static string GetAsp(IControl control)
		{
			return @"<%@ Control Language=""C#"" Inherits=""System.Web.Mvc.ViewUserControl<Wms.Web.IControlModel>"" %>" + control.Contents;
		}
		#endregion
	}
}
