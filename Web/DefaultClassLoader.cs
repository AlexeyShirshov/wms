using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using Microsoft.CSharp;
using Microsoft.Practices.Unity;
using Wms.Interfaces;
using System.Linq;

namespace Wms.Web
{
	public class DefaultClassLoader : IClassLoader
	{
		#region Implementation of IClassLoader

		public void Load(CodeCompileUnit compileUnit, string assemblyName, params Assembly[] additionalAssemblies)
		{
			var codeProvider = new CSharpCodeProvider(new Dictionary<string, string> { {"CompilerVersion", "v3.5"} });

#if DEBUG
			codeProvider.GenerateCodeFromCompileUnit(compileUnit, Console.Out, new CodeGeneratorOptions());
#endif
			///Searching for System.Web.Mvc
			
			var required = new[]
			            	{
			            		Assembly.GetAssembly(typeof (Controller)),
			            		Assembly.GetAssembly(typeof (IUnityContainer)),
								Assembly.GetAssembly(typeof(Func<>)),
								Assembly.GetAssembly(typeof(IQueryable<>))
			            	};

			var files = required.Select(a => a.Location).Union(additionalAssemblies.Select(a => a.Location)).ToArray();
			

			if (Path.GetExtension(assemblyName) != "dll")
			{
				assemblyName += ".dll";
			}
			CompilerResults result =
				codeProvider.CompileAssemblyFromDom(
					new CompilerParameters(files) {GenerateInMemory = true, OutputAssembly = assemblyName}, compileUnit);

			if (result.Errors.Count > 0)
			{
				throw new CompilerException(result.Errors);
			}

			//Console.WriteLine(result.CompiledAssembly.FullName);
		}

		#endregion
	}
}
