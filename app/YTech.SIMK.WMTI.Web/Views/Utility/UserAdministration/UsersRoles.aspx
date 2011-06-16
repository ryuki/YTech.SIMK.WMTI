﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPopup.Master" Inherits="System.Web.Mvc.ViewPage<YTech.SIMK.WMTI.Web.Controllers.ViewModel.UserAdministration.DetailsViewModel>" %>

<asp:Content ContentPlaceHolderID="title" runat="server">
	User Details: <%= Model.DisplayName %>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

	<link href='<% =Url.Content("~/Content/MvcMembership.css") %>' rel="stylesheet" type="text/css" />

	<h2 class="mvcMembership">User Details: <%= Model.DisplayName %> [<% =Model.Status %>]</h2>

	<ul class="mvcMembership-tabs">
		<li><% =Html.ActionLink("Details", "Details", "UserAdministration", new {  id = Model.User.ProviderUserKey }, null) %></li>
		<li><% =Html.ActionLink("Password", "Password", "UserAdministration", new {  id = Model.User.ProviderUserKey }, null) %></li>
		<li>Roles</li>
	</ul>

	<h3 class="mvcMembership">Roles</h3>
	<div class="mvcMembership-userRoles">
		<ul class="mvcMembership">
			<% foreach(var role in Model.Roles){ %>
			<li>
				<% =Html.ActionLink(role.Key, "Role", "UserAdministration", new { id = role.Key },null)%>
				<% if(role.Value){ %>
					<% using(Html.BeginForm("RemoveFromRole", "UserAdministration", new{id = Model.User.ProviderUserKey, role = role.Key})){ %>
					<input type="submit" value="Remove From" />
					<% } %>
				<% }else{ %>
					<% using(Html.BeginForm("AddToRole", "UserAdministration", new{id = Model.User.ProviderUserKey, role = role.Key})){ %>
					<input type="submit" value="Add To" />
					<% } %>
				<% } %>
			</li>
			<% } %>
		</ul>
	</div>

</asp:Content>