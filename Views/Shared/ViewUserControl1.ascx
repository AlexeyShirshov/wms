<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" Description="xxxxxxx"  %>
<table style="border: solid 1px red;display: inline;"><tr><td><%=UniqueID %></td></tr></table>
<asp:Repeater runat="server" ID="dsfg" />
<%= dsfg.UniqueID %>
<%--<% Html.RenderView("sdfg", "ViewUserControl2", Model.);  %>--%>