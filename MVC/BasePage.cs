using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc.Html;

namespace Wms.Mvc
{
	public class BasePage : System.Web.Mvc.ViewPage
	{
		public void Control(string name)
		{
			if(String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			Html.RenderPartial(name, new ControlModel());
		}

		public void Control(string name, object data)
		{
			Html.RenderPartial(name, new ControlModel { Data = data });
		}
	}
}
