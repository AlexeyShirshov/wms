using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WXML.Model;
using WXML.Model.Descriptors;

namespace Wms.Web.Models.Entities
{
	public class EntityDescriptionViewModel
	{
		public EntityDescription EntityDescription { get; set; }
		public IEnumerable<string> AllowedTypes { get; set; }
	}
}
