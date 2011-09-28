<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<YTech.SIMK.WMTI.Web.Controllers.ViewModel.ReportParamViewModel>" %>
<%--<% using (Html.BeginForm())
   { %>--%>
     <% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
                                           InsertionMode = InsertionMode.Replace,
                                           OnSuccess = "onSavedSuccess",
                                           LoadingElementId = "progress"
                                       }

          ))
                       {%>
<%= Html.AntiForgeryToken() %>
<table>
  <%--  <tr>
        <td>
            <label for="ExportFormat">
                Format Laporan :</label>
        </td>
        <td>
            <%= Html.DropDownList("ExportFormat")%>
        </td>
    </tr>--%>
    <% if (ViewData.Model.ShowDateFrom)
       {	%>
    <tr>
        <td>
            <label for="DateFrom">
                Tanggal :</label>
        </td>
        <td>
            <%= Html.TextBox("DateFrom", CommonHelper.ConvertToString(Model.DateFrom))%>
        </td>
    </tr>
    <% } %>
    <% if (ViewData.Model.ShowDateTo)
       {	%>
    <tr>
        <td>
            <label for="DateTo">
                Sampai Tanggal :</label>
        </td>
        <td>
            <%= Html.TextBox("DateTo", CommonHelper.ConvertToString(Model.DateTo))%>
        </td>
    </tr>
    <% } %>
    <% if (ViewData.Model.ShowRecPeriod)
       {	%>
    <tr>
        <td>
            <label for="RecPeriodId">
                Periode :</label>
        </td>
        <td>
            <%= Html.DropDownList("RecPeriodId", Model.RecPeriodList)%>
        </td>
    </tr>
    <% } %>
    <tr>
        <td colspan="2" align="center">
            <button id="Save" type="submit" name="Save">
                Lihat Laporan</button>
        </td>
    </tr>
</table>
<% } %>

<form action='<%= ResolveUrl("~/ReportViewer.aspx") %>' method="post" target="_blank" id="formid">
   <input type="hidden" name="rpt" id="rpt" />
   <input type="hidden" name="dataSource" id="dataSource" />
   <input type="hidden" name="dataSourceName" id="dataSourceName" />
</form>

<script language="javascript" type="text/javascript">
    function onSavedSuccess(e) {
        var json = e.get_response().get_object();
//        alert(json);
        //set value reports
//        alert(json.UrlReport);
//        $("#rpt").val(json.UrlReport);
//        alert(json.DataSourceName);
//        $("#dataSourceName").val(json.DataSourceName);
//        alert(json.RptDataSource);
//        $("#dataSource").val(json.RptDataSource);

        var urlreport ='<%= ResolveUrl("~/ReportViewer.aspx?rpt=") %>' + json.UrlReport;
        //alert(urlreport);
        window.open(urlreport);
        //$("#formid").submit();
    }
    
    $(document).ready(function () {
        //$("#Save").button();
        $("#DateFrom").datepicker();
        $("#DateTo").datepicker();
    });
</script>
