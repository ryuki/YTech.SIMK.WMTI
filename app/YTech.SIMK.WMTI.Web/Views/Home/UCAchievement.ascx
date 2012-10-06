<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<HomeViewModel>" %>

<% if (false)
   { %>
<script src="../../../Scripts/jquery-1.6.3-vsdoc.js" type="text/javascript"></script>
<script src="../../../Scripts/jquery.validate-vsdoc.js" type="text/javascript"></script>
<% } %>
<p class="title">Prestasi Target <%= Model.OneMonthAgoAchievement.Month.ToString(CommonHelper.MonthFormat)%> </p>
<table>
<tr>
<td valign="top">
1.
</td>
<td>
Collector Berprestasi adalah <br />
<%= Model.OneMonthAgoAchievement.CollectorName %> = (tertagih) Rp.<%= CommonHelper.ConvertToString(Model.OneMonthAgoAchievement.CollectorAchievement) %> / (Total Tagihannya) Rp.<%= CommonHelper.ConvertToString(Model.OneMonthAgoAchievement.CollectorTarget) %>
</td>
</tr>
<tr>
<td valign="top">
2.
</td>
<td>
Leader Team Sales Berprestasi adalah <br />
<%= Model.OneMonthAgoAchievement.LTSName %> = (Terjual HD) Rp.<%= CommonHelper.ConvertToString(Model.OneMonthAgoAchievement.LTSAchievement) %> / (Target HD) Rp.<%= CommonHelper.ConvertToString(Model.OneMonthAgoAchievement.LTSTarget) %>
</td>
</tr>
<tr>
<td valign="top">
3.
</td>
<td>
Sales Berprestasi adalah <br />
<%= Model.OneMonthAgoAchievement.SalesmanName %> = (Terjual HT) Rp.<%= CommonHelper.ConvertToString(Model.OneMonthAgoAchievement.SalesmanAchievement) %> / (Target HT) Rp.<%= CommonHelper.ConvertToString(Model.OneMonthAgoAchievement.SalesmanTarget) %>
</td>
</tr>
</table>