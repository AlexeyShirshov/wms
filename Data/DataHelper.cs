using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Proto.Data;
using Wms.Proto.Repository;

namespace Wms.Proto.Web.Data
{
	public class DataHelper
	{
		private static IRepository<IPage> _pageRepository = new ListPageRepository();

		public static IRepository<IPage> GetPageRepository()
		{
			return _pageRepository;
		}
	}
}
