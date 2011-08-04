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
        <%= Html.ActionLinkForAreas<LoanController>(c => c.CustomerRequest(null), "Form Permohonan Konsumen") %>
       <%-- <%= Html.ActionLinkForAreas<LoanController>(c => c.Survey(null), "Form Survey") %>--%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Request.ToString()), "Daftar Kredit Baru")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Survey.ToString()), "Daftar Kredit Survey")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Approve.ToString()), "Daftar Kredit Disetujui")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Reject.ToString()), "Daftar Kredit Ditolak")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Cancel.ToString()), "Daftar Kredit Dibatalkan")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Postpone.ToString()), "Daftar Kredit Ditunda")%>
    </div>
    <h3>
        <a href="#">Angsuran & Pelunasan</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<InstallmentController>(c => c.Index(), "Input Angsuran Baru")%>
    </div>
    <h3>
        <a href="#">Komisi</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<CommissionController>(c => c.Index(EnumDepartment.TLS.ToString()),"Team Leader Sales") %>
        <%= Html.ActionLinkForAreas<CommissionController>(c => c.Index(EnumDepartment.COL.ToString()),"Kolektor") %>
    </div>
    <h3>
        <a href="#">Administrasi</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<DepartmentController>(c => c.Index(),"Master Departemen") %>
        <%= Html.ActionLinkForAreas<EmployeeController>(c => c.Index(), "Master Karyawan")%>
        <%= Html.ActionLinkForAreas<ZoneController>(c => c.Index(), "Master Wilayah")%>
        <%= Html.ActionLinkForAreas<PartnerController>(c => c.Index(), "Master Toko")%>
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
