using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WXML.Model;
using WXML.Model.Descriptors;

namespace Wms.Extensions
{
	[Obsolete]
	public static class PropertyDefinitionExtensions
	{
        public static bool IsPrimaryKey(this PropertyDefinition propertyDefinition)
        {
			return (propertyDefinition.Attributes & Field2DbRelations.PrimaryKey) > 0;
        }
	}
}
