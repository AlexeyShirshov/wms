<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Wms.Master" Inherits="System.Web.Mvc.ViewPage<EntityDescriptionViewModel>" %>
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

    <h2><%= Model.EntityDescription.Identifier %></h2>        
    <% using (Html.BeginForm())
	   { %>
	   <div id="table">
	   <table id="grid" cellpadding="0" cellspacing="0" border="0">
	   <caption>sdlfk</caption>
	   <thead>
	   <tr>	   
	    <th></th>
	    <th>Name</th>
	    <th>Type</th>
	   </tr>
	   </thead>
    <% for (int i = 0; i < Model.EntityDescription.GetActiveProperties().Count(); i++)
	   {
	   
		   var property = Model.EntityDescription.GetActiveProperties().Skip(i).First(); %>
		
		<tr id="line<%= i %>" class='<%=(i % 2) == 0?"":"odd"%>'>
		<td><%= Html.Hidden("propID."+i, property.PropertyAlias)%>
		<%= Html.CheckBox(i + ".IsPrimaryKey", property.HasAttribute(WXML.Model.Field2DbRelations.PK))%></td>
		<td><%= Html.TextBox(i + ".Name", property.Name)%></td>
		<td><%= Html.DropDownList(i + ".ClrTypeName", new SelectList(Model.AllowedTypes, property.PropertyType.ClrType.ToString()))%></td>
		</tr>

    <% } %>
        <tfoot>
        <tr>
            <td colspan="3">
            <a href="#" id="addPropertyLink">Add property</a>
            </td>
        </tr>
        </tfoot>
		</table>
		</div>
		<div id='placeholder' />
		<p><input type="submit" value="Save" /></p>
	<% } %>
</asp:Content>