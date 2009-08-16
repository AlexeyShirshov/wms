<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Wms.Master" Inherits="System.Web.Mvc.ViewPage<EntityDefinitionViewModel>" %>
<%@ Import Namespace="WXML.Model"%>
<%@ Import Namespace="Wms.Web.Models.Entities"%>

<asp:content id="Content2" contentplaceholderid="Head" runat="server" >
	<script type="text/javascript">
		$(document).ready(
			function() {
				$('#addPropertyLink').click(
					function() {
						$('#grid').append('test');
						return false;
					}
				)
			}
		)
	</script>
</asp:content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Model.EntityDefinition.Identifier %></h2>        
    <% using (Html.BeginForm())
 { %>
    <table>
        <% foreach (var pd in Model.EntityDefinition.GetProperties())
           { %>
        <tr>
            <td>
                <%= Html.CheckBox("IsPrimaryKey", pd.Attributes == Field2DbRelations.PrimaryKey)%>
            </td>
            <td>
                <%= pd.Name%>
            </td>
            <td>
                <%= pd.PropertyType.Identifier%>
            </td>
            <td>
                <%= Html.PropertyEditLink("Edit", pd)%>
                <%= Html.PropertyDeleteLink("Delete", pd)%>
            </td>
        </tr>
        <% } %>
    </table>
     
 <% } %>
    <%= Html.CreatePropertyLink("Add property", Model.EntityDefinition) %>
    
</asp:Content>
