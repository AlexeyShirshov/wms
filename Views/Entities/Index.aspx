<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Wms.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<EntityDefinition>>" %>
<%@ Import Namespace="WXML.Model.Descriptors"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Entities</h2>
    <table>
    <% foreach (var item in Model) { %>
        <tr>
            <td>
                <%= Html.ActionLink(item.Name, "Browse", new { type = item.Identifier }) %>
            </td>
            <td>
				<%= Html.ActionLink("Edit definition", "EditDefinition", new { type= item.Identifier }) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>
