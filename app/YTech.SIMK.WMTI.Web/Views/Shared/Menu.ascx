<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="YTech.SIMK.WMTI.Web.Controllers.Transaction" %>
<%@ Import Namespace="YTech.SIMK.WMTI.Web.Controllers.Master" %>
<%@ Import Namespace="YTech.SIMK.WMTI.Web.Controllers.Utility" %>
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

            <h3><a href="#">Pengajuan Kredit</a></h3>
            <div>
                <div><%= Html.ActionLinkForAreas<LoanController>(c => c.Survey(null), "Form Survey") %></div>
                <div><%= Html.ActionLinkForAreas<LoanController>(c => c.Index(), "Daftar Kredit")%></div>
            </div>

            <h3><a href="#">Angsuran & Pelunasan</a></h3>
            <div>
                <div><%= Html.ActionLinkForAreas<InstallmentController>(c => c.Index(), "Input Angsuran Baru")%></div>
            </div>
            
    <h3>
        <a href="#">Administrasi</a></h3>
    <div>
        <div>
            <%= Html.ActionLinkForAreas<DepartmentController>(c => c.Index(),"Master Departemen") %></div>
        <div>
            <%= Html.ActionLinkForAreas<EmployeeController>(c => c.Index(), "Master Karyawan")%>
        </div>
    </div>

            <h3><a href="#">Utiliti</a></h3>
            <div>
                <div><%= Html.ActionLinkForAreas<UserAdministrationController>(c => c.ListUsers(), "Daftar Pengguna")%></div>
            </div>
            
            <h3><a href="#">Laporan</a></h3>
            <div>
                <div><%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReports.RptDueInstallment), "Lap. Angsuran Jatuh Tempo")%></div>
            </div>
    <%
        }
%>
</div>
