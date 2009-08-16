<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Wms.Master" Inherits="System.Web.Mvc.ViewPage<Wms.Web.Models.PropertyDefinitionViewModel>" %>
<%@ Import Namespace="WXML.Model"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Add property</h2>
    <%= Html.EntityEditLink(Model.EntityDefinition.Name, Model.EntityDefinition) %>
	<% using (Html.BeginForm())
 {%>
    <p><%= Html.ValidationSummary() %></p>
    <p>
		Is primary key <br />
    <%=Html.CheckBox("IsPrimaryKey")%>
    <%= Html.ValidationMessage("IsPrimaryKey") %>
    </p>

    <p>
        Name <br />
        <%=Html.TextBox("Name")%>
        <%= Html.ValidationMessage("Name") %>
    </p>
    <p>
        Type <br />
        <%=Html.DropDownList("Type", new SelectList(Model.AllowedTypes)) %>
 	                                                   
 	                                                   <%= Html.ValidationMessage("Type") %>
    </p>
    <input type="submit" value="Save" />
    <%
 }%>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
