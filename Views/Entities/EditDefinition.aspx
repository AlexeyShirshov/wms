<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Wms.Master" Inherits="System.Web.Mvc.ViewPage<EntityDefinitionViewModel>" Trace="true" %>
<%@ Import Namespace="WXML.Model"%>
<%@ Import Namespace="Wms.Web.Models.Entities"%>
<%@ Import Namespace="MvcContrib.UI.Grid" %>

<asp:content id="Content2" contentplaceholderid="Head" runat="server" >
</asp:content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Model.EntityDefinition.Identifier %></h2>        
    <% using (Html.BeginForm())
 { %>
   <%-- <table>
		<tr>
			<td>PK</td>
			<td>Required</td>
			<td>Name</td>
			<td>Type</td>
			<td>&nbsp;</td>
		</tr>
        <% foreach (var pd in Model.EntityDefinition.GetProperties())
           { %>
        <tr>
            <td>
                <%= Html.CheckBox("IsPrimaryKey", (pd.Attributes & Field2DbRelations.PrimaryKey) > 0, new { disabled = "disabled" })%>
            </td>
            <td>
				<%= Html.CheckBox("IsRequired", pd.PropertyType.IsNullableType, new {disabled = "disabled"} ) %>
            </td>
            <td>
                <%= pd.Name%>
            </td>
            <td>
                <%= pd.PropertyType.Identifier%>
            </td>
            <td>
                <%= Html.PropertyEditLink("Edit", pd)%>
                <%= Html.PropertyDeleteLink("Delete", pd)%>
            </td>
        </tr>
        <% } %>
    </table>--%>
     
     <%--этот грид в 10 раз медленее рендерит чем закомментированный код--%>
     
     <% =Html.Grid(Model.EntityDefinition.GetProperties())
         .Attributes(id=>"errt")
         .Columns(item =>
         {
             item.For((pd) => Html.CheckBox("IsPrimaryKey", (pd.Attributes & Field2DbRelations.PrimaryKey) > 0, new { disabled = "disabled" })).Named("PK").DoNotEncode();
             item.For((pd) => Html.CheckBox("IsRequired", pd.PropertyType.IsNullableType, new { disabled = "disabled" })).Named("Required").DoNotEncode();
             item.For((pd) => pd.Name).Named("Name");
             item.For((pd) => pd.PropertyType.Identifier).Named("Type");
             item.For((pd) => Html.PropertyEditLink("Edit", pd) + "&nbsp;" + Html.PropertyDeleteLink("Delete", pd)).Named("Control").DoNotEncode();
         })
     %>
     
 <% } %>
    <%= Html.CreatePropertyLink("Add property", Model.EntityDefinition) %>
    
</asp:Content>
