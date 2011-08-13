<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>

<div id="formArea">
    <table>
        <tr>
            <td colspan="2"><label for="UnitPrice" style="font-size:x-large;">HARGA UNIT BARANG :</label></td>
            <td><input type="text" id="UnitPrice" style="font-size:x-large;" /></td>
            <td style="width: 106px;">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2"><label for="LoanDownPayment" style="font-size:x-large;">DOWN PAYMENT :</label></td>
            <td><input type="text" id="LoanDownPayment" style="font-size:x-large;" /></td>
            <td style="width: 106px;">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2"><label for="LoanBasicPrice" style="font-size:x-large;">HD (HARGA DASAR) :</label></td>
            <td><input type="text" id="LoanBasicPrice" style="font-size:x-large;" /></td>
            <td style="width: 106px;">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2"><label for="LoanCreditPrice" style="font-size:x-large;">HT (HARGA KREDIT) :</label></td>
            <td><input type="text" id="LoanCreditPrice" style="font-size:x-large;" /></td>
            <td style="width: 106px;">&nbsp;</td>
            <td><input type="text" id="LoanTotal" style="font-size:x-large; width: 200px;" /></td>
        </tr>
        <tr>
            <td colspan="2"><label for="LendingRate" style="font-size:x-large;">BUNGA :</label></td>
            <td><input type="text" id="LendingRate" style="font-size:x-large;" /></td>
            <td style="width: 106px;"><label for="LoanProfit" style="font-size:x-large;">LABA :</label></td>
            <td><input type="text" id="LoanProfit" style="font-size:x-large; width: 200px;" /></td>
        </tr>
        <tr>
            <td style="width: 188px;"><label for="LoanTenor" style="font-size:x-large;">ANGSURAN :</label></td>
            <td style="width: 57px;"><input type="text" id="LoanTenor" 
                    style="font-size:x-large; width:73px" /></td>
            <td>
                <table>
                    <tr>
                        <td style="width: 95px;">
                            <label for="TenorModal">Modal :</label></td>
                        <td style="width: 118px;">
                            <input type="text" id="TenorModal" style="width: 124px;" /></td>
                    </tr>
                    <tr>
                        <td style="width: 95px;">
                            <label for="TenorRate">Bunga :</label></td>
                        <td style="width: 118px;">
                            <input type="text" id="TenorRate" style="width: 124px;" /></td>
                    </tr>
                </table>
            </td>
            <td style="width: 106px;">
                <label for="TotalTenorMonth">Total Angsuran / Bulan :</label></td>
            <td>
                <input type="text" id="TotalTenorMonth" 
                    style="font-size:x-large; width: 200px;" /></td>
        </tr>
        </table>
</div>

<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        function onTyped() {
            var UP = parseFloat($('#UnitPrice').val());
            var minUP = 2450000;
            var CreditCost = 25000;
            var CreditMultiply = 1.35;
            var LendingRatePercent = 0.03

            if (UP >= minUP)
                $('#LoanDownPayment').val(UP * 0.20);
            else
                $('#LoanDownPayment').val(0);

            $('#LoanBasicPrice').each(function () {
                if (!isNaN($('#LoanDownPayment').val()) && $('#LoanDownPayment').val().length != 0)
                    this.value = UP - parseFloat($('#LoanDownPayment').val());
                else
                    this.value = UP;
            });

            $('#LoanCreditPrice').val(($('#LoanBasicPrice').val() * CreditMultiply) + CreditCost);

            $('#LendingRate').each(function () {
                if (!isNaN($('#LoanTenor').val()) && $('#LoanTenor').val().length != 0)
                    this.value = parseFloat($('#LoanCreditPrice').val()) * ($('#LoanTenor').val() * LendingRatePercent);
            });

            $('#TenorModal').each(function () {
                if (!isNaN($('#LoanTenor').val()) && $('#LoanTenor').val().length != 0)
                    this.value = (parseFloat($('#LoanCreditPrice').val()) / parseFloat($('#LoanTenor').val()));
            });

            $('#TenorRate').each(function () {
                if (!isNaN($('#LoanTenor').val()) && $('#LoanTenor').val().length != 0)
                    this.value = (parseFloat($('#LendingRate').val()) / parseFloat($('#LoanTenor').val()));
            });

            $('#TotalTenorMonth').each(function () {
                if (!isNaN($('#LoanTenor').val()) && $('#LoanTenor').val().length != 0)
                    this.value = parseFloat($('#TenorModal').val()) + parseFloat($('#TenorRate').val());
            });

            $('#LoanTotal').each(function () {
                if (!isNaN($('#LendingRate').val()) && $('#LendingRate').val().length != 0)
                    this.value = parseFloat($('#LoanDownPayment').val()) + parseFloat($('#LoanCreditPrice').val()) + parseFloat($('#LendingRate').val());
            });

            $('#LoanProfit').each(function () {
                if (!isNaN($('#LoanTotal').val()) && $('#LoanTotal').val().length != 0)
                    this.value = parseFloat($('#LoanTotal').val()) - (parseFloat($('#LoanDownPayment').val()) + parseFloat($('#LoanBasicPrice').val()));
            });
        };

        $('#UnitPrice').keyup(onTyped);
        $('#LoanTenor').keyup(onTyped);
    });
</script>
