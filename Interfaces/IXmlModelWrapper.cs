using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wms.Interfaces
{
	interface ITypeDefinition
	{
		string TypeId { get; }
		string DisplayName { get; }
	}

	interface IEntityDefinition
	{
		string EntityId { get; }
		string Name { get; set; }
		IList<IPropertyDefinition> Properties { get; }
	}

	interface IPropertyDefinition
	{
		string PropertyId { get; }
		string Name { get; set; }
		ITypeDefinition Type { get; set; }
		bool IsPrimaryKey { get; set; }

	}

	interface IXmlModelWrapper
	{
		IEnumerable<ITypeDefinition> GetTypes();
		IList<IEntityDefinition> Entities { get; }
	}
}
