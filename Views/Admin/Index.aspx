<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Wms.Proto.Web.IPage>>" %>
<%@ import namespace="Cms.Proto.Web.Extensions"  %>
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
		<%= Html.PageEditLink(page) %>
    </p>
    <% } %>
    <p>
		<%= Html.ActionLink("Create new  page", "Create") %>
    </p>
   
    </div>
</body>
</html>
