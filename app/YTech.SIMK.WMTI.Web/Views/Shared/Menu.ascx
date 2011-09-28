<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="YTech.SIMK.WMTI.Web.Controllers.Transaction" %>
<%@ Import Namespace="YTech.SIMK.WMTI.Web.Controllers.Master" %>
<%@ Import Namespace="YTech.SIMK.WMTI.Web.Controllers.Utility" %>
<div id="accordion">
    <h3>
        <a href="#">Home</a></h3>
    <div class="child-menu-container">
        <%=Html.ActionLinkForAreas<HomeController>(c => c.Index(), "Home") %>
        <%= Html.ActionLinkForAreas<HomeController>(c => c.Calculate(), "Simulasi Kredit") %>
    </div>
    <% if (Request.IsAuthenticated)
       {
    %>
    <h3>
        <a href="#">Pengajuan Kredit</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<LoanController>(c => c.CustomerRequest(null), "Entry Permohonan Konsumen") %>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(null), "Daftar Permohonan Konsumen")%>
    </div>
    <h3>
        <a href="#">Data Konsumen</a></h3>
    <div class="child-menu-container">
        <%-- <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Request.ToString()), "Daftar Konsumen Baru")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Survey.ToString()), "Daftar Konsumen Survey")%>--%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Approve), "Daftar Konsumen Disetujui")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Reject), "Daftar Konsumen Ditolak")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Cancel), "Daftar Konsumen Cancel / Batal")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Postpone), "Daftar Konsumen Tunda")%>
    </div>
    <h3>
        <a href="#">Cicilan & Pelunasan Kredit</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.OK), "Daftar Kredit Berjalan")%>
        <%= Html.ActionLinkForAreas<InstallmentController>(c => c.PrintReceipt(), "Cetak Kwitansi Jatuh Tempo")%>
        <%= Html.ActionLinkForAreas<InstallmentController>(c => c.Index(), "Pembayaran Angsuran")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.LatePay), "Daftar Menunggak")%>
        <%= Html.ActionLinkForAreas<LoanController>(c => c.Index(EnumLoanStatus.Paid), "Daftar Konsumen Lunas Kredit")%>
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
        <%= Html.ActionLinkForAreas<ZoneEmployeeController>(c => c.Index(), "Master Pembagian Wilayah Kerja") %>
        <%= Html.ActionLinkForAreas<PartnerController>(c => c.Index(), "Master Toko")%>
    </div>
    <h3>
        <a href="#">Utiliti</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<UserAdministrationController>(c => c.ListUsers(), "Daftar Pengguna")%>
        <%= Html.ActionLinkForAreas<ClosingController>(c => c.Index(), "Tutup Buku")%>
        <%= Html.ActionLinkForAreas<ClosingController>(c => c.Opening(), "Buka Buku")%>
    </div>
    <h3>
        <a href="#">Laporan</a></h3>
    <div class="child-menu-container">
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptDueInstallment,null), "Lap. Angsuran Jatuh Tempo")%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptCommission,EnumDepartment.SU), "Lap. Komisi Surveyor")%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptCommission,EnumDepartment.SA ), "Lap. Komisi Salesman")%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptCommission,EnumDepartment.TLS), "Lap. Komisi TLS")%>
        <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptCommission,EnumDepartment.COL), "Lap. Komisi Kolektor")%>
    </div>
    <%
       }
    %>
</div>
