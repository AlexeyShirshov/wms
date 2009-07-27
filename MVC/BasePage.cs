using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc.Html;

namespace Wms.MVC
{
	public class BasePage : System.Web.Mvc.ViewPage
	{
		public void Block(string name)
		{
			if(String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			Html.RenderPartial(name);
		}
	}
}
