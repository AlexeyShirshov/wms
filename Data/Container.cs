using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Hosting;
using Wms.Data;
using Wms.Repository;

namespace Wms.Web.Data
{
	public class Container
	{
		private static IRepository<IPage> _pageRepository = new ListPageRepository();
		private static IRepository<IControl> _controlRepository = new ListControlRepository();

		public static IRepository<IPage> GetPageRepository()
		{
			return _pageRepository;
		}

		public static IPageGenerator GetPageGenerator()
		{
			return new AspPageGenerator();
		}

		public static IRepository<IControl> GetControlRepository()
		{
			return _controlRepository;	
		}
	}
}
