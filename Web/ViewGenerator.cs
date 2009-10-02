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
using Wms.Helpers;
using Wms.Interfaces;
using Wms.MVC;
using WXML.Model;
using WXML.Model.Descriptors;

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
			cc.BaseTypes.Add(typeof (WmsController));
			cc.Members.Add(new CodeMemberField(typeof (IRepositoryManager), "_repositoryManager"));

			//constructor
			var constructor = new CodeConstructor {Attributes = MemberAttributes.Public};
			constructor.AddParam<IUnityContainer>("container");
			constructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("container"));

			constructor.Statements.Add(CodeGen.AssignField("_repositoryManager", new CodeMethodInvokeExpression(
			                                                                     	new CodeMethodReferenceExpression(
			                                                                     		CodeGen.FieldRef("Container"), "Resolve",
			                                                                     		CodeGen.TypeRef<IRepositoryManager>()
			                                                                     		))));

			cc.Members.Add(constructor);

			//Index action
			CodeMemberMethod index = GetAction("Index");
			index.Statements.Add(
				new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "View",
				                                                             new CodeMethodInvokeExpression(
				                                                             	new CodeMethodReferenceExpression(
				                                                             		CodeGen.FieldRef("_repositoryManager"),
				                                                             		"GetEntityQuery", new CodeTypeReference(clrType))))));


			cc.Members.Add(index);

			//Edit action
			CodeMemberMethod edit = GetAction("Edit");
			var eqExpressions = new List<String>();
			edit.Parameters.AddRange(GetActionParameters(ed));

			//Creating predicate
			foreach (PropertyDefinition pk in ed.GetProperties().Where(pd => pd.IsPrimaryKey()))
			{
				eqExpressions.Add(ed.Name.ToLower() + "." + pk.Name + " == " + pk.Name.ToLower());
			}
			var predicate =
				new CodeVariableDeclarationStatement(
					new CodeTypeReference(
						typeof (Expression<>).MakeGenericType(typeof (Func<,>).MakeGenericType(clrType, typeof (bool)))), "predicate");
			edit.Statements.Add(predicate);
			var lambda = new CodeSnippetExpression(ed.Name.ToLower() + " => " + String.Join(" && ", eqExpressions.ToArray()));
			edit.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("predicate"), lambda));

			edit.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference(clrType), "model"));
			var modelReference = new CodeVariableReferenceExpression("model");

			var source = new CodeMethodInvokeExpression(
				new CodeMethodReferenceExpression(CodeGen.FieldRef("_repositoryManager"), "GetEntityQuery",
				                                  new CodeTypeReference(clrType)));
			edit.Statements.Add(CodeGen.AssignVar("model", new CodeMethodInvokeExpression(source, "First", lambda)));

			edit.Statements.Add(
				new CodeMethodReturnStatement(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "View",
				                                                             modelReference)));
			cc.Members.Add(edit);

			//Edit-save action
			CodeMemberMethod editSave = GetAction("Edit");
			editSave.AddParam<FormCollection>("form")
				.Decorate<AcceptVerbsAttribute>("POST")
				.Parameters.AddRange(GetActionParameters(ed));

			var returnRedirect = new CodeMethodReturnStatement(
				new CodeMethodInvokeExpression(
					new CodeThisReferenceExpression(), "RedirectToAction",
					new CodePrimitiveExpression("Index")));
			editSave.Statements.Add(returnRedirect);
			editSave.Statements.Add(CodeGen.DeclareVar(clrType, "item"));
			editSave.Statements.Add(CodeGen.AssignVar("item", new CodeMethodInvokeExpression(source, "First", lambda)));

			cc.Members.Add(editSave);

			//Create action
			CodeMemberMethod create = GetAction("Create");
			create.Statements.Add(new CodeMethodReturnStatement(
			                      	new CodeMethodInvokeExpression(
			                      		new CodeThisReferenceExpression(), "View")));

			cc.Members.Add(create);

			//Create-save action
			CodeMemberMethod createSave = GetAction("Create");
			createSave.AddParam<FormCollection>("form")
				.Decorate<AcceptVerbsAttribute>("POST");

			createSave.Statements.Add(returnRedirect);

			cc.Members.Add(createSave);

			var ns = new CodeNamespace("Wms.Controllers");
			ns.Types.Add(cc);
			ns.Imports.Add(new CodeNamespaceImport("System.Linq"));

			var ccu = new CodeCompileUnit();
			ccu.Namespaces.Add(ns);

			return ccu;
		}

		private static CodeMemberMethod GetAction(string name)
		{
			return new CodeMemberMethod
			       	{
			       		Name = name,
			       		Attributes = MemberAttributes.Public,
			       		ReturnType = new CodeTypeReference(typeof (ActionResult))
			       	};
		}

		#endregion

		private static CodeParameterDeclarationExpression[] GetActionParameters(EntityDefinition ed)
		{
			return
				ed.GetProperties().Where(pd => pd.IsPrimaryKey()).Select(
					pk => new CodeParameterDeclarationExpression(new CodeTypeReference(pk.PropertyType.ClrType), pk.Name.ToLower())).
					ToArray();
		}


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
