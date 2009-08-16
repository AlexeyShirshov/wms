<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Wms.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<EntityDefinition>>" %>
<%@ Import Namespace="WXML.Model.Descriptors"%>
<asp:content id="Content2" contentplaceholderid="Head" runat="server">
</asp:content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Entities</h2>
    <table>
    <% foreach (var item in Model) { %>
        <tr>
            <td>
                <span class="my"><%= Html.ActionLink(item.Name, "Browse", new { entityId = item.Identifier }) %></span>
            </td>
            <td>
				<%= Html.EntityEditLink("Edit", item) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>
