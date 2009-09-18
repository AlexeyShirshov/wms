using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wms.Web;

namespace Wms.Tests
{
	[TestFixture]
	public class DefaultClassLoaderTest
	{
		[Test]
		public void Can_Compile_And_Load()
		{
			///Arrange
			var m = new CodeMemberMethod { Name = "Do", ReturnType = new CodeTypeReference(typeof(void)) };
			var t = new CodeTypeDeclaration("Foo") ;
			t.Members.Add(m);
			var ns = new CodeNamespace("Test");
			ns.Types.Add(t);
			var ccu = new CodeCompileUnit();
			ccu.Namespaces.Add(ns);

			///Act
			new DefaultClassLoader().Load(ccu, "Code.dll");

			///Assert
			var type = Type.GetType("Test.Foo,Code");
			Assert.IsNotNull(type);
		}

		[Test]
		public void Can_Compile_And_Load_Mvc()
		{
			var m = new CodeMemberMethod { Name = "Do", ReturnType = new CodeTypeReference(typeof(void)) };
			var t = new CodeTypeDeclaration("Foo") ;
			t.BaseTypes.Add(new CodeTypeReference(typeof(System.Web.Mvc.Controller)));
			t.Members.Add(m);
			var ns = new CodeNamespace("Test");
			ns.Types.Add(t);
			var ccu = new CodeCompileUnit();
			ccu.Namespaces.Add(ns);

			///Act
			new DefaultClassLoader().Load(ccu, "Code.dll");

			///Assert
			var type = Type.GetType("Test.Foo,Code");
			Assert.IsNotNull(type);
		}

	}
}
