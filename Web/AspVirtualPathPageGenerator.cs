﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Wms.Web;

namespace Wms.Web
{
	public class AspPageGenerator : IPageGenerator
	{
		private string _rootFolder;

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

		private static string GetAsp(IPage page)
		{
			return @"<%@ Page Language=""C#"" Inherits=""System.Web.Mvc.ViewPage"" %>" + page.Contents;
		}
		#endregion
	}
}
