using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wms.Exceptions
{
	public class EntityNotFoundException : Exception
	{
		public String EntityName { get; private set; }

		public EntityNotFoundException(string entityName)
		{
			EntityName = entityName;
		}

		public override string Message
		{
			get
			{
				return string.Format("Entity type {0} not found", EntityName);
			}
		}
	}
}
