using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Wms.Interfaces;
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

		public void GenerateController(EntityDefinition ed, TextWriter tw)
		{
			var generator = new CodeDomGenerator();
            var controller = generator.AddReference(
                //typeof(System.Web.Mvc.Controller).Assembly.CodeBase)
                @"C:\WINDOWS\assembly\GAC_MSIL\System.Web.Mvc\1.0.0.0__31bf3856ad364e35\System.Web.Mvc.dll")
				.AddNamespace("Wms.Controllers").AddClass(ed.Identifier + "Controller")
				.Implements(typeof(System.Web.Mvc.Controller));

			tw.WriteLine(generator.GenerateCode(CodeDomGenerator.Language.CSharp));

            Assembly a = generator.Compile();
            
            if (a == null)
                throw new ApplicationException("Облом");
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
