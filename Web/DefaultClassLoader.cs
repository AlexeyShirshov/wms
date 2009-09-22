using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using Wms.Interfaces;

namespace Wms.Web
{
	public class DefaultClassLoader : IClassLoader
	{
		#region Implementation of IClassLoader

		public void Load(CodeCompileUnit compileUnit, string assemblyName)
		{
			var codeProvider = new CSharpCodeProvider();
			
#if DEBUG
			codeProvider.GenerateCodeFromCompileUnit(compileUnit, Console.Out, new CodeGeneratorOptions());
#endif
			///Searching for System.Web.Mvc
			string[] names = new[] { Assembly.GetAssembly(typeof(System.Web.Mvc.Controller)).Location } ;
			if(Path.GetExtension(assemblyName) != "dll")
			{
				assemblyName += ".dll";
			}
			var result = codeProvider.CompileAssemblyFromDom(new CompilerParameters(names) { GenerateInMemory = true, OutputAssembly = assemblyName }, compileUnit);

			if(result.Errors.Count > 0)
			{
				throw new CompilerException(result.Errors);
			}

			Console.WriteLine(result.CompiledAssembly.FullName);
		}

		#endregion
	}
}
