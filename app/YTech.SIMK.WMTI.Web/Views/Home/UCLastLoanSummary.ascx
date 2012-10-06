<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<HomeViewModel>" %>

<% if (false)
   { %>
<script src="../../../Scripts/jquery-1.6.3-vsdoc.js" type="text/javascript"></script>
<script src="../../../Scripts/jquery.validate-vsdoc.js" type="text/javascript"></script>
<% } %>
<p class="title">Data Bulan <%= Model.OneMonthAgoLoanSummary.Month.ToString(CommonHelper.MonthFormat)%> </p>

<table>
<tr>
<td rowspan="3">
Total Penjualan :
</td>
<td>
HD = Rp.<%= CommonHelper.ConvertToString(Model.OneMonthAgoLoanSummary.TotalHD)%>
</td>
</tr>
<tr>
<td>
HT = Rp.<%= CommonHelper.ConvertToString(Model.OneMonthAgoLoanSummary.TotalHT)%>
</td>
</tr>
<tr>
<td>
Angsuran = Rp.<%= CommonHelper.ConvertToString(Model.OneMonthAgoLoanSummary.TotalInstallment)%>
</td>
</tr>

<tr>
<td rowspan="2">
Penagihan Collector :
</td>
<td>
Total Tagihan = Rp.<%= CommonHelper.ConvertToString(Model.OneMonthAgoLoanSummary.TotalMustPaidInstallment)%>
</td>
</tr>
<tr>
<td>
Total Tertagih = Rp.<%= CommonHelper.ConvertToString(Model.OneMonthAgoLoanSummary.TotalPaidInstallment)%>
</td>
</tr>

<tr>
<td rowspan="5">
Jumlah PK Masuk :
</td>
<td>
Total PK = <%= CommonHelper.ConvertToString(Model.OneMonthAgoLoanSummary.TotalLoan)%> Permohonan
</td>
</tr>
<tr>
<td>
PK Terima = <%= CommonHelper.ConvertToString(Model.OneMonthAgoLoanSummary.TotalLoanApprove)%> Permohonan
</td>
</tr>
<tr>
<td>
PK Tolak = <%= CommonHelper.ConvertToString(Model.OneMonthAgoLoanSummary.TotalLoanReject)%> Permohonan
</td>
</tr>
<tr>
<td>
PK Tunda = <%= CommonHelper.ConvertToString(Model.OneMonthAgoLoanSummary.TotalLoanPostpone)%> Permohonan
</td>
</tr>
<tr>
<td>
PK Cancel = <%= CommonHelper.ConvertToString(Model.OneMonthAgoLoanSummary.TotalLoanCancel)%> Permohonan
</td>
</tr>
</table>
