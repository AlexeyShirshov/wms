<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Wms.Web.IControl>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CreateControl</title>
</head>
<body>
    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
            <label for="Name">Name</label>
            <%= Html.TextBox("Name") %>
            </p>
            <p>
            <label for="Contents">Contents</label>
            <%= Html.TextBox("Contents") %>
            </p>

            
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Controls") %>
    </div>

</body>
</html>

