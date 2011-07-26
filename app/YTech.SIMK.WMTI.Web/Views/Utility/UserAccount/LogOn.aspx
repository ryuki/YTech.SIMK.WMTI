<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPopup.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
   <link href="<%= Url.Content("~/Content/css/login.css") %>" rel="stylesheet" type="text/css" media="screen" />
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
   <%-- <h2>Log On</h2>
    <p>
        Please enter your username and password. <%= Html.ActionLink("Register", "Register", "UserAccount")%> if you don't have an account.
    </p>--%>
    <%= Html.ValidationSummary("Login was unsuccessful. Please correct the errors and try again.") %>

    <div id="login">
        <div id="content">
            <% using (Html.BeginForm())
               { %>
            <label class="login-info" for="username">
                Username</label>
            <%= Html.TextBox("username",null, new { @class = "input" })%>
            <label class="login-info" for="password">
                Password</label>
            <%= Html.Password("password", null, new { @class = "input" })%>
            <%= Html.ValidationMessage("password") %>
            <div id="remember-forgot">
                <div class="checkbox">
                    <%= Html.CheckBox("rememberMe") %></div>
                <div class="rememberme">
                    <label class="inline" for="rememberMe">
                        Remember me?</label></div>
               <%-- <div id="forgot-password">
                    <a href="#">Forgot Password ?</a>
                </div>--%>
                <div id="login-buttton">
                    <%--<input type="submit" value="Log On" />--%>
                    <input name="Submit" src="<%= Url.Content("~/Content/images/login-button.jpg") %>" type="image" value="Giriş" />
                </div>
            </div>
            <% } %>
        </div>
    </div>
</asp:Content>
