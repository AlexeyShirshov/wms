<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Wms.Master" Inherits="System.Web.Mvc.ViewPage<Wms.Web.Models.PropertyDefinitionViewModel>" %>
<%@ Import Namespace="WXML.Model"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit property</h2>
    <%= Html.EntityEditLink(Model.PropertyDefinition.Entity.Name, Model.PropertyDefinition.Entity) %>.<%= Model.PropertyDefinition.Identifier %>
	<% using (Html.BeginForm())
 {%>
    <p><%= Html.ValidationSummary() %></p>
    <p>
		Is primary key </br>
    <%=Html.CheckBox("IsPrimaryKey", Model.PropertyDefinition.Attributes == Field2DbRelations.PrimaryKey)%>
    <%= Html.ValidationMessage("IsPrimaryKey") %>
    </p>

    <p>
        Name <br />
        <%=Html.TextBox("Name", Model.PropertyDefinition.Name)%>
        <%= Html.ValidationMessage("Name") %>
    </p>
    <p>
        Type <br />
        <%=Html.DropDownList("Type",
 	                                    new SelectList(Model.AllowedTypes,
 	                                                   Model.PropertyDefinition.PropertyType.Identifier))%>
 	                                                   <%= Html.ValidationMessage("Type") %>
    </p>
    <input type="submit" value="Save" />
    <%
 }%>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
