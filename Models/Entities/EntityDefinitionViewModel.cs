using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WXML.Model;
using WXML.Model.Descriptors;

namespace Wms.Web.Models.Entities
{
	public class EntityDefinitionViewModel
	{
        public EntityDefinition EntityDefinition { get; set; }
		public IEnumerable<string> AllowedTypes { get; set; }
	}
}
