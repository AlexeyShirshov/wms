<%@ Page Language="VB" Inherits="System.Web.Mvc.ViewPage<WXML.Model.Descriptors.EntityDescription>" Strict="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>BrowseEntity</title>
</head>
<body>
    <table>
    <% For Each item In Model.ActiveProperties%>
        <tr>
            <th><%= item.Name %></th>
        </tr>
    <% Next %>

    <% For Each item In Wms.Repository.WmsDataFacade.GetEntity(Model.Name)%>
        <tr>
            <th><%= item.ID %></th>
        </tr>
    <% Next %>
    </table>


</body>
</html>

