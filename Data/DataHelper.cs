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
	public class DataHelper
	{
		private static IRepository<IPage> _pageRepository = new ListPageRepository();

		public static IRepository<IPage> GetPageRepository()
		{
			return _pageRepository;
		}

		public static IPageGenerator GetPageGenerator()
		{
			return new AspPageGenerator(HostingEnvironment.MapPath("/Views/Shared"));
		}

	}
}
