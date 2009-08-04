<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<System.Linq.IQueryable<Wms.Data.Internal.Page>>" %>
<%----%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>BrowseEntity</title>
</head>
<body>
    <table>
    <tr>
        <th>ID</th>
        <th>Name</th>
        <th>Url</th>
    </tr>
    
    <% foreach(var item in Model) {%>
    <tr>
        <td><%=item.ID%></td>
        <td><%=item.Name%></td>
        <td><%=item.Url%></td>
    </tr>
    <% } %>
    </table>
</body>
</html>

