using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wms.Helpers
{
	public static class CodeGen
	{
		public static CodeVariableDeclarationStatement DeclareVar<T>(string name)
		{
			return new CodeVariableDeclarationStatement(typeof(T), name);
		}

		public static CodeAssignStatement AssignVar(string varName, CodeExpression value)
		{
			return new CodeAssignStatement(new CodeVariableReferenceExpression(varName), value);
		}

		public static CodeAssignStatement AssignField(string fieldName, CodeExpression value)
		{
			return new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName), value);
		}

		public static CodeFieldReferenceExpression FieldRef(string fieldName)
		{
			return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName);
		}

		public static CodeTypeReference TypeRef<T>()
		{
			return new CodeTypeReference(typeof(T));
		}
	}
}
