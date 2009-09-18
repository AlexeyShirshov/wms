using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wms.Interfaces
{
	public interface IClassLoader
	{
		void Load(CodeCompileUnit compileUnit, string assemblyName);
	}
}
