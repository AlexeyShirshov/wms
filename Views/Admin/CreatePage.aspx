<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<PageEditorModel>" %>
<%@ Import Namespace="Wms.Web.Models.Admin"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Create</title>
    
    <script src="/js/jquery-1.3.2.js" type="text/javascript"></script>
    <% if (false)
	   { %>
    <script src="../../js/jquery-1.3.2-vsdoc2.js" type="text/javascript"></script>
    <% } %>
    
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
        <p>Contents</p>        
        <p><%= Html.DropDownList("Controls", new SelectList(Model.Controls, "Name", "Name" )) %> <a id="insertControlLink" href="#">Insert control</a> <br />
		<%= Html.TextArea("Contents", new {width = "100%", height = "500px"}) %>
    </p>
    <p>
		<input type="submit" value="Save" />
	</p>
	<% } %>
    </div>
</body>
</html>
