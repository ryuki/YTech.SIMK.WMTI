<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="YTech.SIMK.WMTI.Web.Controllers.Transaction" %>
<%@ Import Namespace="YTech.SIMK.WMTI.Web.Controllers.Master" %>
<%@ Import Namespace="YTech.SIMK.WMTI.Web.Controllers.Utility" %>
<div id="accordion">
    <h3>
        <a href="#">Home</a></h3>
    <div class="child-menu-container">
        <%=Html.ActionLinkForAreas<HomeController>(c => c.Index(), "Home") %>
    </div>
    <% if (Request.IsAuthenticated)
       {
    %>
    <h3>
        <a href="#">Pengajuan Kredit</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Survey(null), "Form Survey") %>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(), "Daftar Kredit")%>
    </div>
    <h3>
        <a href="#">Angsuran & Pelunasan</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<InstallmentController>(c => c.Index(), "Input Angsuran Baru")%>
    </div>
    <h3>
        <a href="#">Administrasi</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<DepartmentController>(c => c.Index(),"Master Departemen") %>
        <%= Html.ActionLinkForAreas<EmployeeController>(c => c.Index(), "Master Karyawan")%>
        <%= Html.ActionLinkForAreas<ZoneController>(c => c.Index(), "Master Wilayah")%>
    </div>
    <h3>
        <a href="#">Utiliti</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<UserAdministrationController>(c => c.ListUsers(), "Daftar Pengguna")%>
    </div>
    <h3>
        <a href="#">Laporan</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptDueInstallment), "Lap. Angsuran Jatuh Tempo")%>
    </div>
    <%
       }
    %>
</div>
