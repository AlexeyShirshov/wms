<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Wms.Master" Inherits="System.Web.Mvc.ViewPage<EntityDefinitionViewModel>" %>
<%@ Import Namespace="WXML.Model"%>
<%@ Import Namespace="Wms.Web.Models.Entities"%>

<asp:content id="Content2" contentplaceholderid="Head" runat="server" >
</asp:content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Model.EntityDefinition.Identifier %></h2>        
    <% using (Html.BeginForm())
 { %>
    <table>
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
                <%= Html.CheckBox("IsPrimaryKey", (pd.Attributes & Field2DbRelations.PrimaryKey) > 0, new {disabled = "disabled")%>
            </td>
            <td>
				<%= Html.CheckBox("IsRequired", pd.PropertyType.IsNullableType, new {disabled = "disabled" ) %>
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
    </table>
     
 <% } %>
    <%= Html.CreatePropertyLink("Add property", Model.EntityDefinition) %>
    
</asp:Content>
