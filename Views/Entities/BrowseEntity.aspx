<%@ Page Language="VB" Inherits="System.Web.Mvc.ViewPage(Of WXML.Model.Descriptors.EntityDescription)" Strict="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>BrowseEntity</title>
</head>
<body>
    <table>
    <tr>
    <% For Each item In Model.ActiveProperties%>
        <th><%= item.Name %></th>
    <% Next %>
    </tr>
    
    <% For Each item In Wms.Repository.WmsDataFacade.GetEntityQuery(Model.Name)%>
    <tr>
        <td><%=item.ID%></td>
        <td><%=item.Name%></td>
        <td><%=item.Url%></td>
    </tr>
    <% Next %>    
    </table>
</body>
</html>

