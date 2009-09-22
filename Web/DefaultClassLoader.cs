using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using Microsoft.CSharp;
using Microsoft.Practices.Unity;
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
			var names = new[]
			            	{
			            		Assembly.GetAssembly(typeof (Controller)).Location,
			            		Assembly.GetAssembly(typeof (IUnityContainer)).Location
			            	};
			if (Path.GetExtension(assemblyName) != "dll")
			{
				assemblyName += ".dll";
			}
			CompilerResults result =
				codeProvider.CompileAssemblyFromDom(
					new CompilerParameters(names) {GenerateInMemory = true, OutputAssembly = assemblyName}, compileUnit);

			if (result.Errors.Count > 0)
			{
				throw new CompilerException(result.Errors);
			}

			Console.WriteLine(result.CompiledAssembly.FullName);
		}

		#endregion
	}
}
