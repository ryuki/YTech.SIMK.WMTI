<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<PrintReceiptViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%
        if (false)
        {%>
    <script src="../../../Scripts/jquery-1.6.3-vsdoc.js" type="text/javascript"></script>
    <%
    }%>
    
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
    <tr>
        <td>
            <label for="LoanAcc">
                No Account :</label>
        </td>
        <td>
            <%= Html.TextBox("LoanAcc", Model.LoanAcc)%>
        </td>
    </tr>  
    <tr>
        <td>
            <label for="InstallmentNo">
                Angsuran ke :</label>
        </td>
        <td>
            <%= Html.TextBox("InstallmentNo", Model.InstallmentNo)%>
        </td>
    </tr> 
    <tr>
        <td colspan="2" align="center">
            <button id="Save" type="submit" name="Save">
                Cetak Kwitansi</button>
        </td>
    </tr> 
</table>
<% } %>

<script language="javascript" type="text/javascript">
    function onSavedSuccess(e) {
        var json = e.get_response().get_object();
        var msg = json.Message;
        var status = json.Success;
        //alert(status);
        if (status) {
             var urlreport = '<%= ResolveUrl("~/ReportViewer.aspx?rpt=") %>' + json.UrlReport;
        //alert(urlreport);
        window.open(urlreport);
        }
        else {
            alert(msg);
        }
    }

    $(document).ready(function () {
        //$("#Save").button();
    });
</script>
</asp:Content>
