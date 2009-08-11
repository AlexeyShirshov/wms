<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Wms.Master" Inherits="System.Web.Mvc.ViewPage<EntityDescriptionViewModel>" %>
<%@ Import Namespace="Wms.Web.Models.Entities"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Model.EntityDescription.Identifier %></h2>    
    <% using (Html.BeginForm())
	   { %>
    <table>
    <% for (int i = 0; i < Model.EntityDescription.ActiveProperties.Count; i++)
	   {
		   var property = Model.EntityDescription.ActiveProperties[i]; %>
	   <tr>
		<td><%= Html.CheckBox(i + ".IsPrimaryKey", property.Entity.PkProperties.Contains(property))%></td>
		<td><%= Html.TextBox(i + ".Name", property.Name)%></td>
		<td><%= Html.DropDownList(i + ".ClrTypeName", new SelectList(Model.AllowedTypes, property.PropertyType.ClrType.Name))%></td>
	   </tr>    
    <% } %>
    </table>
		<input type="submit" value="Save" />
	<% } %>
    
</asp:Content>
