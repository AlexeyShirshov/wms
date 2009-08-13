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
	   <div id='grid'>
    <% for (int i = 0; i < Model.EntityDescription.ActiveProperties.Count; i++)
	   {
		   var property = Model.EntityDescription.ActiveProperties[i]; %>
		
		<div id='line<%= i %>'>
		<%= Html.CheckBox(i + ".IsPrimaryKey", property.Entity.PkProperties.Contains(property))%>
		<%= Html.TextBox(i + ".Name", property.Name)%>
		<%= Html.DropDownList(i + ".ClrTypeName", new SelectList(Model.AllowedTypes, property.PropertyType.ClrType.Name))%>
		</div>
				

    <% } %>
		</div>
		<div id='placeholder' />
		<p><input type="submit" value="Save" /></p>
		<p><a href="#" id="addPropertyLink">Add property</a></p>
	<% } %>
</asp:Content>
