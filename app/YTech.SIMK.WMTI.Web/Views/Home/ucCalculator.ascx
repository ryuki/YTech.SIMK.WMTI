<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="formArea">
    <table>
        <tr>
            <td colspan="2">
                <label for="UnitPrice">
                    HARGA UNIT BARANG :</label>
            </td>
            <td>
                <input type="text" id="UnitPrice" />
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <label for="LoanDownPayment">
                    DOWN PAYMENT :</label>
            </td>
            <td>
                <input type="text" id="LoanDownPayment" />
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <label for="LoanBasicPrice">
                    HD (HARGA DASAR) :</label>
            </td>
            <td>
                <input type="text" id="LoanBasicPrice" />
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <label for="LoanCreditPrice">
                    HT (HARGA KREDIT) :</label>
            </td>
            <td>
                <input type="text" id="LoanCreditPrice" />
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                <input type="text" id="LoanTotal" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <label for="LendingRate">
                    BUNGA :</label>
            </td>
            <td>
                <input type="text" id="LendingRate" />
            </td>
            <td>
                <label for="LoanProfit">
                    LABA :</label>
            </td>
            <td>
                <input type="text" id="LoanProfit" />
            </td>
        </tr>
        <tr>
            <td>
                <label for="LoanTenor">
                    ANGSURAN :</label>
            </td>
            <td>
                <input type="text" id="LoanTenor" size="5" />
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <label for="TenorModal">
                                Modal :</label>
                        </td>
                        <td>
                            <input type="text" id="TenorModal" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="TenorRate">
                                Bunga :</label>
                        </td>
                        <td>
                            <input type="text" id="TenorRate" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <label for="TotalTenorMonth">
                    Total Angsuran / Bulan :</label>
            </td>
            <td>
                <input type="text" id="TotalTenorMonth" />
            </td>
        </tr>
    </table>
</div>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('input').autoNumeric();
        $('input').attr("style", "text-align:right;");
        $('input').attr("disabled", "disabled");

        $('#UnitPrice').removeAttr("disabled");
        $('#LoanDownPayment').removeAttr("disabled");
        $('#LoanTenor').removeAttr("disabled");

        $('#UnitPrice').keyup(CalculateInstallment);
        $('#LoanDownPayment').keyup(CalculateInstallment);
        $('#LoanTenor').keyup(CalculateInstallment);
    });

    function CalculateDP() {
        var minUP = 2450000;
        if (UP >= minUP)
            $('#LoanDownPayment').val(UP * 0.20);
        else
            $('#LoanDownPayment').val(0);
    }

        function CalculateInstallment() {
            var UP = ConvertToDecimal($('#UnitPrice').val());
            var minUP = 2450000;
            var CreditCost = 30000;
            var CreditMultiply = 1.35;
            var LendingRatePercent = 0.026;

            //if (!isNaN($('#LoanDownPayment').val()) && $('#LoanDownPayment').val().length != 0) {
                if (ConvertToDecimal($('#LoanDownPayment').val()) > 0)
                    LendingRatePercent = 0.033;
                else
                    LendingRatePercent = 0.026;
            //}
            //alert(LendingRatePercent);
//            if (UP >= minUP)
//                $('#LoanDownPayment').val(UP * 0.20);
//            else
//                $('#LoanDownPayment').val(0);

            $('#LoanBasicPrice').each(function () {
//                alert($('#LoanDownPayment').val().length);
                this.value = UP - ConvertToDecimal($('#LoanDownPayment').val());
//                if (!isNaN($('#LoanDownPayment').val()) && $('#LoanDownPayment').val().length != 0) {
//                    this.value = UP - ConvertToDecimal($('#LoanDownPayment').val());
//                }
//                else
//                    this.value = UP;
            });

            $('#LoanCreditPrice').val(($('#LoanBasicPrice').val() * CreditMultiply) + CreditCost);

            $('#LendingRate').each(function () {
//                if (!isNaN($('#LoanTenor').val()) && $('#LoanTenor').val().length != 0)
                    this.value = ConvertToDecimal($('#LoanCreditPrice').val()) * (ConvertToDecimal($('#LoanTenor').val()) * LendingRatePercent);
//                alert('BUNGA');
//                alert(ConvertToDecimal($('#LoanCreditPrice').val()));
//                alert(ConvertToDecimal($('#LoanTenor').val()));
//                alert(LendingRatePercent);
            });

            $('#TenorModal').each(function () {
//                if (!isNaN($('#LoanTenor').val()) && $('#LoanTenor').val().length != 0)
                    this.value = (ConvertToDecimal($('#LoanCreditPrice').val()) / ConvertToDecimal($('#LoanTenor').val()));
            });

            $('#TenorRate').each(function () {
//                if (!isNaN($('#LoanTenor').val()) && $('#LoanTenor').val().length != 0)
                    this.value = (ConvertToDecimal($('#LendingRate').val()) / ConvertToDecimal($('#LoanTenor').val()));
            });

            $('#TotalTenorMonth').each(function () {
//                if (!isNaN($('#LoanTenor').val()) && $('#LoanTenor').val().length != 0)
                    this.value = ConvertToDecimal($('#TenorModal').val()) + ConvertToDecimal($('#TenorRate').val());
            });

            $('#LoanTotal').each(function () {
//                if (!isNaN($('#LendingRate').val()) && $('#LendingRate').val().length != 0)
                    this.value = ConvertToDecimal($('#LoanCreditPrice').val()) + ConvertToDecimal($('#LendingRate').val());
            });

            $('#LoanProfit').each(function () {
//                if (!isNaN($('#LoanTotal').val()) && $('#LoanTotal').val().length != 0)
                    this.value = ConvertToDecimal($('#LoanTotal').val()) - (ConvertToDecimal($('#LoanDownPayment').val()) + ConvertToDecimal($('#LoanBasicPrice').val()));
            });
        };

        function ConvertToDecimal(str) {
        try
        {
            var result = parseFloat(str.replace(/\./gi, "").replace(/,/gi, "."));
            return result;
            }
            catch(err)
            {
            return 0;
            }
        }
</script>
