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
        <%= Html.ActionLinkForAreas<LoanController>(c => c.CustomerRequest(null), "Entry Permohonan Konsumen") %>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Request.ToString()), "Daftar Permohonan Konsumen")%>
    </div>

     <h3>
         <a href="#">Data Konsumen</a></h3>
    <div class="child-menu-container">       
       <%-- <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Request.ToString()), "Daftar Konsumen Baru")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Survey.ToString()), "Daftar Konsumen Survey")%>--%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Approve.ToString()), "Daftar Konsumen Disetujui")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Reject.ToString()), "Daftar Konsumen Ditolak")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Cancel.ToString()), "Daftar Konsumen Cancel / Batal")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Postpone.ToString()), "Daftar Konsumen Tunda")%>
    </div>
    <h3>
        <a href="#">Cicilan & Pelunasan Kredit</a></h3>
    <div class="child-menu-container">
     <a href="#">Daftar Kredit Berjalan</a>
     <a href="#">Cetak Kwitansi Jatuh Tempo</a>
        <%= Html.ActionLinkForAreas<InstallmentController>(c => c.Index(), "Pembayaran Angsuran")%>
     <a href="#">Daftar Menunggak</a>
     <a href="#">Daftar Konsumen Lunas Kredit</a>
    </div>
    <h3>
        <a href="#">Komisi</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<CommissionController>(c => c.Index(EnumDepartment.TLS.ToString()),"Team Leader Sales") %>
        <%= Html.ActionLinkForAreas<CommissionController>(c => c.Index(EnumDepartment.SA.ToString()),"Sales") %>
        <%= Html.ActionLinkForAreas<CommissionController>(c => c.Index(EnumDepartment.SU.ToString()),"Surveyor") %>
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
