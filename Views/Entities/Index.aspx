<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<WXML.Model.WXMLModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
</head>
<body>
    <table>
        <tr>
            <th>Entities</th>
        </tr>

    <% foreach (var item in Model.ActiveEntities) { %>
        <tr>
            <td>
                <%= Html.ActionLink(item.Name, "Edit", new { /* id=item.PrimaryKey */ }) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</body>
</html>

