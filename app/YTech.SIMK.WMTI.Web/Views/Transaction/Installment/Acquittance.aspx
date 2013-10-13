<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPopup.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<AcquittanceViewModel>" %>

<asp:Content ContentPlaceHolderID="head" ID="headContent" runat="server">
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% if (false)
       {%>
    <script src="../../../Scripts/jquery-1.6.3-vsdoc.js" type="text/javascript"></script>
    <% } %>
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
        <tr>
            <td>
                <label for="BasicPrice">
                    HD :</label>
            </td>
            <td align="right">
                <%= CommonHelper.ConvertToString(Model.BasicPrice) %>
            </td>
        </tr>
        <tr>
            <td>
                <label for="CreditPrice">
                    HT :</label>
            </td>
            <td align="right">
                <%= CommonHelper.ConvertToString(Model.CreditPrice) %>
            </td>
        </tr>
        <tr>
            <td>
                <label for="Installment">
                    Angsuran :</label>
            </td>
            <td align="right">
                <%= CommonHelper.ConvertToString(Model.Installment) %>
            </td>
        </tr>
        <tr>
            <td>
                <label for="LoanTenor">
                    Tenor :</label>
            </td>
            <td align="right">
                <%= CommonHelper.ConvertToString(Model.LoanTenor, 0)%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="PaidInstallment">
                    Telah Dibayar :</label>
            </td>
            <td align="right">
                <%= CommonHelper.ConvertToString(Model.PaidInstallment, 0)%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="RequiredInstallment">
                    Norma Wajib :</label>
            </td>
            <td align="right">
                <%= CommonHelper.ConvertToString(Model.RequiredInstallment,0) %>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <fieldset>
                    <legend>Pembayaran</legend>
                    <table>
                        <tr>
                            <td>
                                Norma Wajib
                            </td>
                            <td>
                                75 %
                            </td>
                            <td>
                                = Rp.
                            </td>
                            <td align="right">
                                <%= CommonHelper.ConvertToString(Model.RequiredInstallmentTotal) %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Lose Sort
                            </td>
                            <td>
                                <%= CommonHelper.ConvertToString(Model.LoseSort, 0)%>
                            </td>
                            <td>
                                = Rp.
                            </td>
                            <td align="right">
                                <%= CommonHelper.ConvertToString(Model.LoseSortTotal) %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Provisi
                            </td>
                            <td>
                                10 %
                            </td>
                            <td>
                                = Rp.
                            </td>
                            <td align="right">
                                <%= CommonHelper.ConvertToString(Model.Provision) %>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <b>Jumlah Yang Harus Dibayar</b>
                            </td>
                            <td>
                                <b>= Rp.</b>
                            </td>
                            <td align="right">
                                <b>
                                    <%= CommonHelper.ConvertToString(Model.MustPaid) %></b>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td>
                <label for="EmployeeId">
                    Kolektor :</label>
            </td>
            <td>
                <%= Html.DropDownList("EmployeeId", Model.CollectorList)%>
                            <%= Html.ValidationMessage("EmployeeId")%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="InstallmentPaymentDate">
                    Tgl Pembayaran :</label>
            </td>
            <td>
                <%= Html.TextBox("InstallmentPaymentDate", CommonHelper.ConvertToString(Model.InstallmentPaymentDate))%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="ReceiptNo">
                    No Kwitansi :</label>
            </td>
            <td>
               <%= Html.TextBox("ReceiptNo", ViewData.Model.ReceiptNo)%>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <%= Html.Hidden("MustPaid", CommonHelper.ConvertToString(Model.MustPaid))%>
                <input id="btnSave" type="submit" value="Simpan" />
            </td>
        </tr>
    </table>
    <%
       }%>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#InstallmentPaymentDate").datepicker();
        });

        function onSavedSuccess(e) {
            var json = e.get_response().get_object();
            var msg = json.Message;
            var status = json.Success;
            //alert(status);
            alert(msg);
            if (status) {
                $('#btnSave').attr('disabled', 'disabled');
            }
        }
    </script>
</asp:Content>
