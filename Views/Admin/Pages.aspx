<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Wms.Web.IPage>>" masterpagefile="~/Views/Shared/Wms.Master" %>
<%@ import namespace="Cms.Web.Extensions"  %>

    <% foreach (var page in Model)
	   {%>
    <p>
		<%= Html.PageEditLink(page) %> <a href="<%= "/" + page.Url %>"> go to page</a>
    </p>
    <% } %>
    <p>
		<%= Html.ActionLink("Create new  page", "CreatePage") %>
    </p>
   
