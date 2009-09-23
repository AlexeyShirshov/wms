using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Wms.Interfaces
{
	public interface IClassLoader
	{
		void Load(CodeCompileUnit compileUnit, string assemblyName, params Assembly[] additionalAssemblies);
	}
}
