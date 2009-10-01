using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Wms.Data;
using Wms.Extensions;
using Wms.Interfaces;
using WXML.Model;
using WXML.Model.Descriptors;
using Wms.Helpers;
//using LinqToCodedom;
//using LinqToCodedom.Extensions;

namespace Wms.Web
{
	public class ViewGenerator : IViewGenerator
	{

		#region Implementation of IViewGenerator

		public void GenerateCreateView(EntityDefinition ed, TextWriter tw)
		{
			if (ed == null)
				throw new ArgumentNullException("ed");
			if (tw == null)
				throw new ArgumentNullException("tw");

			GenerateGenericView(tw, ed, false);
		}

		public void GenerateBrowseView(EntityDefinition ed, TextWriter tw)
		{
			if (ed == null)
				throw new ArgumentNullException("ed");
			if (tw == null)
				throw new ArgumentNullException("tw");

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
			if (ed == null)
				throw new ArgumentNullException("ed");
			if (tw == null)
				throw new ArgumentNullException("tw");

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

		public CodeCompileUnit GenerateController(EntityDefinition ed, Type clrType)
		{
			if (ed == null)
				throw new ArgumentNullException("ed");
			if (clrType == null)
				throw new ArgumentNullException("clrType");

			var cc = new CodeTypeDeclaration(ed.Identifier + "Controller");
			cc.BaseTypes.Add(typeof (Controller));
			cc.Members.Add(new CodeMemberField(typeof(IUnityContainer), "_container"));
			cc.Members.Add(new CodeMemberField(typeof(IRepositoryManager), "_repositoryManager"));

			//constructor
			var constructor = new CodeConstructor {Attributes = MemberAttributes.Public};
			var containerParam = new CodeParameterDeclarationExpression(typeof (IUnityContainer), "container");
			constructor.Parameters.Add(containerParam);
			constructor.Statements.Add(
				new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_container"),
				                        new CodeVariableReferenceExpression("container")));

			constructor.Statements.Add(CodeGen.AssignField("_repositoryManager", new CodeMethodInvokeExpression(
				new CodeMethodReferenceExpression(CodeGen.FieldRef("_container"), "Resolve", new CodeTypeReference(typeof(IRepositoryManager))))));

			cc.Members.Add(constructor);

			//Index action
			var index = CreateAction("Index");
			index.Statements.Add(new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "View")));

			cc.Members.Add(index);

			//Edit action
			var edit = CreateAction("Edit");

			var eqExpressions = new List<String>();
			foreach(PropertyDefinition pk in ed.GetProperties().Where(pd => pd.IsPrimaryKey()))
			{
				edit.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(pk.PropertyType.ClrType), pk.Name.ToLower()));
				eqExpressions.Add(ed.Name.ToLower() + "." + pk.Name + " == " + pk.Name.ToLower());
			}
			//Creating predicate
			var predicate = new CodeVariableDeclarationStatement(new CodeTypeReference(typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(clrType, typeof(bool)))), "predicate");
			edit.Statements.Add(predicate);
			var lambda = new CodeSnippetExpression(ed.Name.ToLower() + " => " + String.Join(" && ", eqExpressions.ToArray()));
			edit.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("predicate"), lambda));
			
			edit.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(clrType), "model"));
			var modelReference = new CodeVariableReferenceExpression("model");

			var source = new CodeMethodInvokeExpression(
				new CodeMethodReferenceExpression(CodeGen.FieldRef("_repositoryManager"), "GetEntityQuery", new CodeTypeReference(clrType)));
			edit.Statements.Add(CodeGen.AssignVar("model", new CodeMethodInvokeExpression(source, "FirstOrDefault", lambda)));

			//Edit-save action
			var editSave = CreateAction("Edit");
			//edit.Parameters.Add()
            

			
			var returnStmt = new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "View", modelReference));
			edit.Statements.Add(returnStmt);

			cc.Members.Add(edit);

			var ns = new CodeNamespace("Wms.Controllers");
			ns.Types.Add(cc);
			ns.Imports.Add(new CodeNamespaceImport("System.Linq"));

			var ccu = new CodeCompileUnit();
			ccu.Namespaces.Add(ns);
			

			return ccu;


			//var generator = new CodeDomGenerator();

			//var rt = WmsDefinitionManager.GetRepositoryProvider().RepositoryType;

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
			if (propertyDefinition == null)
				throw new ArgumentNullException("propertyDefinition");

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
