<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Wms.Master" Inherits="System.Web.Mvc.ViewPage<EntityDescription>" %>
<%@ Import Namespace="WXML.Model.Descriptors"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Model.Identifier %></h2>    
    <table>
    <% foreach(var property in Model.ActiveProperties)
	   { %>
	   <tr>
		<td><%= property.Name %></td>
		<td><%= property.PropertyType.ClrTypeName %></td>
	   </tr>    
    <% } %>
    </table>
</asp:Content>
