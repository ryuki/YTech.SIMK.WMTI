<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="YTech.SIMK.WMTI.Web.Controllers.Transaction" %>
<div id="accordion">
    <h3>
        <a href="#">Home</a></h3>
    <div>
        <div>
            <%=Html.ActionLinkForAreas<HomeController>(c => c.Index(), "Home") %></div>
    </div>
    <% if (Request.IsAuthenticated)
       {
%>
            <h3><a href="#">Survey</a></h3>
            <div>
                <div><%= Html.ActionLinkForAreas<SurveyController>(c => c.Index(), "Daftar Survey")%></div>
            </div>
    <%
        }
%>
</div>
