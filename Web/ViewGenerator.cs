﻿using System;
using System.CodeDom;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Wms.Extensions;
using Wms.Interfaces;
using WXML.Model;
using WXML.Model.Descriptors;
//using LinqToCodedom;
//using LinqToCodedom.Extensions;

namespace Wms.Web
{
	public class ViewGenerator : IViewGenerator
	{
		private readonly IUnityContainer _container;

		public ViewGenerator(IUnityContainer container)
		{
			_container = container;
		}

		#region Implementation of IViewGenerator

		public void GenerateCreateView(EntityDefinition ed, TextWriter tw)
		{
			GenerateGenericView(tw, ed, false);
		}

		public void GenerateBrowseView(EntityDefinition ed, TextWriter tw)
		{
			var hb = new HtmlBuilder(tw);
			hb.Begin("table").Begin("tr");
			foreach (PropertyDefinition pd in ed.GetProperties())
			{
				hb.Begin("td").Text(pd.Name).End();
			}

			hb.End().End();
		}

		public void GenerateEditView(EntityDefinition ed, TextWriter tw)
		{
			GenerateGenericView(tw, ed, true);
		}

		


		private static void GenerateGenericView(TextWriter tw, EntityDefinition ed, bool isEditView)
		{
			var hb = new HtmlBuilder(tw);
			hb.Tag("form", h => ed.GetProperties()
			                    	.Aggregate(h, (h1, p) => h1.Begin("p").Text(p.Name).Text(GetEditControl(p, isEditView)).End())
			                    	.Tag("input", new {type = "submit", value = "save"}));

			hb.GetWriter().Flush();
		}

		public CodeCompileUnit GenerateController(EntityDefinition ed)
		{
			var cc = new CodeTypeDeclaration(ed.Identifier + "Controller");
			cc.BaseTypes.Add(typeof (Controller));
			cc.Members.Add(new CodeMemberField(typeof (IUnityContainer), "_container"));

			var constructor = new CodeConstructor {Attributes = MemberAttributes.Public};
			var containerParam = new CodeParameterDeclarationExpression(typeof (IUnityContainer), "container");
			constructor.Parameters.Add(containerParam);
			constructor.Statements.Add(
				new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_container"),
				                        new CodeVariableReferenceExpression("container")));

			cc.Members.Add(constructor);

			///Index action
			var index = CreateAction("Index"); 
			var returnStmt = new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "View"));
			index.Statements.Add(returnStmt);

			cc.Members.Add(index);

			///Edit action
			var edit = CreateAction("Edit");

			foreach(PropertyDefinition pk in ed.GetProperties().Where(pd => pd.IsPrimaryKey()))
			{
				edit.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(pk.PropertyType.ClrType), pk.Name));
			}
			edit.Statements.Add(returnStmt);

			cc.Members.Add(edit);

			var ns = new CodeNamespace("Wms.Controllers");
			ns.Types.Add(cc);

			var ccu = new CodeCompileUnit();
			ccu.Namespaces.Add(ns);

			return ccu;


			//var generator = new CodeDomGenerator();

			//var rt = WmsDataFacade.GetRepositoryProvider().RepositoryType;

			//var controller = generator
			//    //.AddReference(typeof(Controller).Assembly.Location)
			//    //.AddReference("System.dll")
			//    //.AddReference("System.Web.dll")
			//    //.AddReference(rt.Assembly.Location)
			//    //.AddReference("System.Data.Linq.dll")
			//    //.AddReference("System.Core.dll")
			//    .AddNamespace("Wms.Controllers")
			//    .AddClass(ed.Identifier + "Controller").Inherits(typeof (Controller));

			//controller.AddField(rt, MemberAttributes.Private, "_ctx");

			//controller.AddCtor(Emit.assignField("_ctx", () => CodeDom.@new(rt)));

			//var propName = WXMLCodeDomGeneratorNameHelper.GetMultipleForm(ed.Name);

			//var index = controller.AddMethod(MemberAttributes.Public, typeof (ActionResult), () => "Index",
			//     Emit.@return(() => CodeDom.@this.Call<ActionResult>("View")(CodeDom.VarRef("_ctx").Property(propName)))
			//);

			//Console.WriteLine(generator.GenerateCode(CodeDomGenerator.Language.CSharp));
			////generator.Compile();

			//return generator.GetCompileUnit(CodeDomGenerator.Language.CSharp);
		}

		private static CodeMemberMethod CreateAction(string name)
		{
			return new CodeMemberMethod { Name = name, Attributes = MemberAttributes.Public, ReturnType = new CodeTypeReference(typeof(ActionResult)) };
		}

		#endregion

		public static string GetEditControl(PropertyDefinition propertyDefinition, bool isEditView)
		{
			string htmlAttributes = String.Empty;
			if ((propertyDefinition.Attributes & Field2DbRelations.ReadOnly) > 0)
				htmlAttributes = @", new { disabled = ""disabled"" }";

			if (propertyDefinition.PropertyType.ClrType == typeof (bool))
				return String.Format(@"<%= Html.CheckBox(""{0}""{1}{2}) %>", propertyDefinition.Name,
				                     isEditView ? @",Model." + propertyDefinition.Name : String.Empty, htmlAttributes);

			return String.Format(@"<%= Html.TextBox(""{0}""{1}{2}) %>", propertyDefinition.Name,
			                     isEditView ? @",Model." + propertyDefinition.Name : String.Empty, htmlAttributes);
		}
	}
}
