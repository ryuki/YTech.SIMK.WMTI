<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="YTech.SIMK.WMTI.Web.Controllers.Transaction" %>
<%@ Import Namespace="YTech.SIMK.WMTI.Web.Controllers.Master" %>
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
    <h3>
        <a href="#">Data Pokok</a></h3>
    <div>
        <div>
            <%= Html.ActionLinkForAreas<DepartmentController>(c => c.Index(),"Master Departemen") %></div>
        <div>
            <%= Html.ActionLinkForAreas<EmployeeController>(c => c.Index(), "Master Karyawan")%>
        </div>
    </div>

            <h3><a href="#">Pengajuan Kredit</a></h3>
            <div>
                <div><%= Html.ActionLinkForAreas<LoanController>(c => c.Survey(), "Form Survey") %></div>
                <div><%= Html.ActionLinkForAreas<LoanController>(c => c.Index(), "Daftar Survey")%></div>
            </div>

            <h3><a href="#">Cicilan</a></h3>
            <div>
                <div><%= Html.ActionLinkForAreas<InstallmentController>(c => c.Index(), "Input Cicilan Baru")%></div>
            </div>
    <%
        }
%>
</div>
