<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<DetailLoanViewModel>" %>
<table>
    <tr>
        <td>
            <table>
                <tr>
                    <td colspan="2" align="center">
                        <div style="width: 400px; height: 200px; overflow: auto;">
                        <div style="float:left;width: 180px; height: 150px;">
                            <fieldset>
                                <legend>Foto 1</legend>
                                <img id="img1" alt="" src='<%= Url.Content(Model.Photo1) %>' width="180px" height="150px" />
                            </fieldset>
                        </div>
                        <div style="float:right;width: 180px; height: 150px;">
                            <fieldset>
                                <legend>Foto 2</legend>
                                <img id="img2" alt="" src='<%= Url.Content(Model.Photo2) %>' width="180px" height="150px" />
                            </fieldset>
                        </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Nama :
                    </td>
                    <td>
                        <%= Model.Loan.CustomerId.PersonId.PersonName %>
                    </td>
                </tr>
                <tr>
                    <td>
                        No KTP :
                    </td>
                    <td>
                        <%= Model.Loan.CustomerId.PersonId.PersonIdCardNo %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Alamat :
                    </td>
                    <td>
                        <%= Model.Loan.CustomerId.AddressId.Address %>
                    </td>
                </tr>
                <tr>
                    <td>
                        No Telp / HP :
                    </td>
                    <td>
                        <%= Model.Loan.CustomerId.PersonId.PersonMobile %>
                    </td>
                </tr>
            </table>
        </td>
        <td>
            <table>
                <tr>
                    <td>
                        Tgl Pengajuan Kredit :
                    </td>
                    <td>
                        <%= CommonHelper.ConvertToString(Model.Loan.LoanSubmissionDate) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        SA :
                    </td>
                    <td>
                        <%= Model.Loan.SalesmanId.PersonId.PersonName %>
                    </td>
                </tr>
                <tr>
                    <td>
                        TL :
                    </td>
                    <td>
                        <%= Model.Loan.TLSId.PersonId.PersonName %>
                    </td>
                </tr>
                <tr>
                    <td>
                        SURV :
                    </td>
                    <td>
                        <%= Model.Loan.SurveyorId.PersonId.PersonName %>
                    </td>
                </tr>
                <tr>
                    <td>
                        COL :
                    </td>
                    <td>
                        <%= Model.Loan.CollectorId.PersonId.PersonName %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Nama Toko :
                    </td>
                    <td>
                        <%= Model.Loan.PartnerId.PartnerName %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Nama Barang :
                    </td>
                    <td>
                        <%= Model.Loan.LoanUnits[0].UnitName %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Type Barang :
                    </td>
                    <td>
                        <%= Model.Loan.LoanUnits[0].UnitType %>
                    </td>
                </tr>
                <tr>
                    <td>
                        Harga Barang :
                    </td>
                    <td align="right">
                        Rp.<%= CommonHelper.ConvertToString(Model.Loan.LoanUnits[0].UnitPrice) %>
                    </td>
                </tr>
            </table>
        </td>
        <td>
            <fieldset title="Informasi Pembayaran">
                <table>
                    <tr>
                        <td>
                            Menunggak :
                        </td>
                        <td>
                            <%= CommonHelper.ConvertToString(Model.InstallmentLate)%>
                            kali
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Bayar Kurang :
                        </td>
                        <td align="right">
                            Rp.<%= CommonHelper.ConvertToString(Model.InstallmentMinus)%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Bayar Lebih :
                        </td>
                        <td align="right">
                            Rp.<%= CommonHelper.ConvertToString(Model.InstallmentMinus)%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Denda :
                        </td>
                        <td align="right">
                            Rp.<%= CommonHelper.ConvertToString(Model.InstallmentFine)%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Down Payment / DP :
                        </td>
                        <td align="right">
                            Rp.<%= CommonHelper.ConvertToString(Model.Loan.LoanDownPayment)%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            HT :
                        </td>
                        <td align="right">
                            Rp.<%= CommonHelper.ConvertToString(Model.Loan.LoanCreditPrice)%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Lama Angsuran :
                        </td>
                        <td>
                            <%= CommonHelper.ConvertToString(Model.Loan.LoanTenor)%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Angsuran Per Bulan :
                        </td>
                        <td align="right">
                            Rp.<%= CommonHelper.ConvertToString(Model.Loan.LoanBasicInstallment)%>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </td>
        <td>
            Catatan :<br />
            <% if (Model.Loan.LoanFeedbacks.Count > 0)
               {%>
            <b>
                <%=Model.Loan.LoanFeedbacks[0].LoanFeedbackType%>
                : </b>
            <br />
            <%=Model.Loan.LoanFeedbacks[0].LoanFeedbackDesc%><br />
            <b>
                <%=Model.Loan.LoanFeedbacks[1].LoanFeedbackType%>
                : </b>
            <br />
            <%=Model.Loan.LoanFeedbacks[1].LoanFeedbackDesc%><br />
            <b>
                <%=Model.Loan.LoanFeedbacks[2].LoanFeedbackType%>
                : </b>
            <br />
            <%=Model.Loan.LoanFeedbacks[2].LoanFeedbackDesc%><br />
            <b>
                <%=Model.Loan.LoanFeedbacks[3].LoanFeedbackType%>
                : </b>
            <br />
            <%=Model.Loan.LoanFeedbacks[3].LoanFeedbackDesc%><br />
            <%
               }%>
        </td>
    </tr>
</table>
