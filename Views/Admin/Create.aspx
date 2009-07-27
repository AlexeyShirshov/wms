<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Wms.Web.IPage>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Create</title>
</head>
<body>
    <div>
    <% using(Html.BeginForm()) { %>
    <p>Name<br />
		<%= Html.TextBox("Name") %>
    </p>
    <p>Url<br />
		<%= Html.TextBox("Url") %>
    </p>
        <p>Contents<br />
		<%= Html.TextArea("Contents", new {width = "100%", height = "500px"}) %>
    </p>
    <p>
		<input type="submit" value="Save" />
	</p>
	<% } %>
    </div>
</body>
</html>
