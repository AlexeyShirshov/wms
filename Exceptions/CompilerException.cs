using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wms
{
	public class CompilerException : Exception
	{
		public CompilerErrorCollection Errors { get; private set; }

		public CompilerException(CompilerErrorCollection errors)
		{
			Errors = errors;
		}

		public override string Message
		{
			get
			{
				var sb = new StringBuilder();
				foreach (var e in Errors)
				{
					sb.AppendLine(e.ToString());
				}
				return sb.ToString();
			}
		}
	}
}
