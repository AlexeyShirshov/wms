using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Wms.Web;
using WXML.Model.Descriptors;

namespace Wms.Web.Extensions
{
	public static class HtmlExtension
	{
		public static string PageEditLink(this HtmlHelper html, IPage page)
		{
			return html.ActionLink(page.Name,  "Edit", "Admin", new { id = page.Name }, null);
		}

        public static string PropertyEditLink(this HtmlHelper html, string linkText, PropertyDefinition propertyDefinition)
        {
            return html.ActionLink(linkText, "Edit", "Property",
                                   new
                                       {
                                           entityId = propertyDefinition.Entity.Identifier,
                                           propertyId = propertyDefinition.Identifier
                                       }, null);
        }

        public static string PropertyDeleteLink(this HtmlHelper html, string linkText, PropertyDefinition propertyDefinition)
        {
            return html.ActionLink(linkText, "Delete", "Property",
                                   new
                                       {
                                           entityId = propertyDefinition.Entity.Identifier,
                                           propertyId = propertyDefinition.Identifier
                                       }, null);

        }
        
        public static string EntityEditLink(this HtmlHelper html, string linkText, EntityDefinition entityDefinition)
        {
            return html.ActionLink(linkText, "EditDefinition", "Entities", new {entityId = entityDefinition.Identifier},
                                   null);
        }

        public static string CreatePropertyLink(this HtmlHelper html, string linkText, EntityDefinition entityDefinition)
        {
            return html.ActionLink(linkText, "Create", "Property", new {entityId = entityDefinition.Identifier}, null);
        }

        public static void RenderView(this HtmlHelper html, string id, string name, object model)
        {
            //html.ViewContext.HttpContext.Response.Write("<div style='border: solid 1px red;float:both;'>");
            html.RenderPartial(name, model);
            //html.ViewContext.HttpContext.Response.Write("</div>");

           xxx(new {key=1}); 
        }

        private static void xxx<T>(T ds)
        { }
	}
}
