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

		public AspPageGenerator(string rootFolder)
		{
			if(String.IsNullOrEmpty(rootFolder))
			{
				throw new ArgumentException("rootFolder can't be null or empty", "rootFolder");
			}
			_rootFolder = rootFolder;
		}


		#region Implementation of IPageGenerator
		public void Generate(IPage page)
		{
            using(var sw = new StreamWriter(Path.Combine(_rootFolder, page.Name) + ".aspx", false))
            {
				sw.Write(GetAsp(page));
            }
		}

		public void Generate(IControl control)
		{
			using (var sw = new StreamWriter(Path.Combine(_rootFolder, control.Name) + ".ascx", false))
			{
				sw.Write(GetAsp(control));
			}
		}

		private static string GetAsp(IPage page)
		{
			return @"<%@ Page Language=""C#"" Inherits=""Wms.Mvc.BasePage""%>" + page.Contents;
		}

		private static string GetAsp(IControl control)
		{
			return @"<%@ Control Language=""C#"" Inherits=""System.Web.Mvc.ViewUserControl<IControlModel>"" %>" + control.Contents;
		}
		#endregion
	}
}
