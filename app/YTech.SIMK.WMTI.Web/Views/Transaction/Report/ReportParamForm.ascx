<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<YTech.SIMK.WMTI.Web.Controllers.ViewModel.ReportParamViewModel>" %>
<%--<% using (Html.BeginForm())
   { %>--%>
     <% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
                                           InsertionMode = InsertionMode.Replace,
                                           OnSuccess = "onSavedSuccess"
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
            <%= Html.TextBox("DateFrom", (Model.DateFrom.HasValue) ? Model.DateFrom.Value.ToString("dd-MMM-yyyy") : "")%>
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
            <%= Html.TextBox("DateTo", (Model.DateTo.HasValue) ? Model.DateTo.Value.ToString("dd-MMM-yyyy") : "")%>
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
        $("#DateFrom").datepicker({ dateFormat: "dd-M-yy" });
        $("#DateTo").datepicker({ dateFormat: "dd-M-yy" });
    });
</script>
