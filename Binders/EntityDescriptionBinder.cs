using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXML.Model.Descriptors;

namespace Wms.Web.Binders
{
	public class EntityDescriptionBinder : IModelBinder
	{
		public string Prefix { get; set; }

		#region Implementation of IModelBinder

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			return null;
		}

		#endregion
	}

	public class EntityDescriptionAttribute : CustomModelBinderAttribute
	{
		private string _prefix;

		public EntityDescriptionAttribute() : this("Property") {} 

		public EntityDescriptionAttribute(string prefix)
		{
			_prefix = prefix;
		}

		#region Overrides of CustomModelBinderAttribute


		public override IModelBinder GetBinder()
		{
			return new EntityDescriptionBinder() { Prefix = _prefix };
		}

		#endregion
	}

}
