using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LinqToCodedom.Generator;
using Wms.Data;
using Wms.Interfaces;
using Wms.Repository;
using WXML.Model.Descriptors;
using LinqToCodedom;
using LinqToCodedom.Extensions;
using System.CodeDom;
using System.Web.UI;
using WXML.Model;
using System.Reflection;

namespace Wms.Web
{
	public class ViewGenerator : IViewGenerator
	{
		#region Implementation of IViewGenerator

		public void GenerateCreateView(EntityDefinition ed, TextWriter tw)
		{
			GenerateGenericView(tw, ed, false);
		}

		public void GenerateBrowseView(EntityDefinition ed, TextWriter tw)
		{
			throw new NotImplementedException();
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
				.Tag("input", new { type = "submit", value = "save" }));
			
			hb.GetWriter().Flush();
		}

		public CodeCompileUnit GenerateController(EntityDefinition ed)
		{
			var generator = new CodeDomGenerator();

			var location = Assembly.LoadWithPartialName("System.Web.Mvc").Location;
		    var controller = generator
		        .AddReference(location)
                .AddReference("System.dll")
                .AddReference("System.Web.dll")
                .AddReference(typeof (IWmsDataFacade).Assembly.Location)
                .AddReference("System.Data.Linq.dll")
                .AddReference("System.Core.dll")
		        .AddNamespace("Wms.Controllers").AddClass(ed.Identifier + "Controller")
		        .Implements(typeof (Controller));

            controller.AddField(typeof(IWmsDataFacade), MemberAttributes.Private, "_dataFacade");

		    controller.AddCtor(Emit.assignField("_dataFacade", () => new WmsDataFacade()));

		    var index = controller.AddMethod(MemberAttributes.Public, typeof (ActionResult), () => "Index",
                 Emit.declare("model", () => 
                     CodeDom.@this.Field<IWmsDataFacade>("_dataFacade").GetEntityQuery(ed.Name)
                 ),
                 Emit.@return(() => CodeDom.@this.Call<ActionResult>("View")(CodeDom.VarRef("model")))
            );

		    Console.WriteLine(generator.GenerateCode(CodeDomGenerator.Language.CSharp));
		    generator.Compile();

		    return generator.GetCompileUnit(CodeDomGenerator.Language.CSharp);
		}


		#endregion

		public static string GetEditControl(PropertyDefinition propertyDefinition, bool isEditView)
		{
			string htmlAttributes = String.Empty;
			if ((propertyDefinition.Attributes & Field2DbRelations.ReadOnly ) > 0)
				htmlAttributes = @", new { disabled = ""disabled"" }";
			
			if (propertyDefinition.PropertyType.ClrType == typeof(bool))
				return String.Format(@"<%= Html.CheckBox(""{0}""{1}{2}) %>", propertyDefinition.Name,
					isEditView ? @",Model." + propertyDefinition.Name : String.Empty, htmlAttributes);

			return String.Format(@"<%= Html.TextBox(""{0}""{1}{2}) %>", propertyDefinition.Name,
					isEditView ? @",Model." + propertyDefinition.Name : String.Empty, htmlAttributes);
		}
	}
}
