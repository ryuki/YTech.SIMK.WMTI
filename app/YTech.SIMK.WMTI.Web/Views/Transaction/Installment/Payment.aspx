<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPopup.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<InstallmentPaymentFormViewModel>" %>

<asp:Content ContentPlaceHolderID="head" ID="headContent" runat="server">
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% if (false)
       {%>
    <script src="../../../Scripts/jquery-1.5.2-vsdoc.js" type="text/javascript"></script>
    <% } %>
    <% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
                                           InsertionMode = InsertionMode.Replace,
                                           OnSuccess = "onSavedSuccess"
                                       }

          ))
       {%>
    <%= Html.AntiForgeryToken() %>
    <%= Html.Hidden("Id", (ViewData.Model.installment != null) ? ViewData.Model.installment.Id : "")%>
    <table>
        <tr>
            <td>
                <label for="InstallmentNo">
                    Angsuran Ke :</label>
            </td>
            <td>
                <%= ViewData.Model.installment.InstallmentNo %>
            </td>
        </tr>
        <tr>
            <td>
                <label for="InstallmentNo">
                    Tgl Jatuh Tempo :</label>
            </td>
            <td>
                <%= ViewData.Model.installment.InstallmentMaturityDate.HasValue ? ViewData.Model.installment.InstallmentMaturityDate.Value.ToString(CommonHelper.DateFormat) : null %>
            </td>
        </tr>
        <tr>
            <td>
                <label for="InstallmentNo">
                    Total Angsuran :</label>
            </td>
            <td>
                <%= ViewData.Model.installment.InstallmentTotal.ToString(CommonHelper.NumberFormat) %>
            </td>
        </tr>
        <tr>
            <td>
                <label for="InstallmentFine">
                    Denda :</label>
            </td>
            <td>
                <%= ViewData.Model.installment.InstallmentFine.HasValue ? ViewData.Model.installment.InstallmentFine.Value.ToString(CommonHelper.NumberFormat) : null%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="InstallmentMustPaid">
                    Total Yg Harus Dibayar :</label>
            </td>
            <td>
                <%= ViewData.Model.installment.InstallmentMustPaid.ToString(CommonHelper.NumberFormat) %>
            </td>
        </tr>
        <tr>
            <td>
                <label for="InstallmentPaymentDate">
                    Tgl Pembayaran :</label>
            </td>
            <td>
                <%= Html.TextBox("InstallmentPaymentDate", ViewData.Model.installment.InstallmentPaymentDate.HasValue ? ViewData.Model.installment.InstallmentPaymentDate.Value.ToString(CommonHelper.DateFormat) : null)%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="InstallmentPaid">
                    Jumlah Dibayar :</label>
            </td>
            <td>
                <%= Html.TextBox("InstallmentPaid", ViewData.Model.installment.InstallmentPaid.HasValue ? ViewData.Model.installment.InstallmentPaid.Value.ToString(CommonHelper.NumberFormat) : null)%>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <input id="btnSave" type="submit" value="Simpan"  <% if (string.IsNullOrEmpty(ViewData.Model.installment.Id)) { %>disabled='disabled' <% } %> />
            </td>
        </tr>
    </table>
    <%
                       }%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#InstallmentPaid').autoNumeric();
            $('#InstallmentPaid').attr("style", "text-align:right;");
//            $('#btnSave').click(function () {
//                alert('submit;');
//                //$('form').submit();
//            });
        });

        function onSavedSuccess(e) {
            var json = e.get_response().get_object();
            var success = json.Success;
            var msg = json.Message;
            alert(msg);
        }
    </script>
</asp:Content>
