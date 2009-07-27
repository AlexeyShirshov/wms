<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Wms.Web.IPage>>" %>
<%@ import namespace="Cms.Web.Extensions"  %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
</head>
<body>
    <div>
    <% foreach (var page in Model)
	   {%>
    <p>
		<%= Html.PageEditLink(page) %> <a href="<%= "/" + page.Url %>"> go to page</a>
    </p>
    <% } %>
    <p>
		<%= Html.ActionLink("Create new  page", "CreatePage") %>
    </p>
   
    </div>
</body>
</html>
