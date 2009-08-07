<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage"  masterpagefile="~/Views/Shared/Wms.Master"%>
<asp:content runat="server" contentplaceholderid="MainContent">
		<p><%= Html.ActionLink("Pages", "Pages") %></p>
		<p><%= Html.ActionLink("Controls", "Controls") %></p>
		<p><%= Html.ActionLink("Entities", "Index", "Entities") %></p>
</asp:content>
