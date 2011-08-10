<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>

<table id="list" class="scroll" cellpadding="0" cellspacing="0">
</table>
<div id="listPager" class="scroll" style="text-align: center;">
</div>
<div id="listPsetcols" class="scroll" style="text-align: center;">
</div>
<script type="text/javascript">

    $(document).ready(function () {
        var now = new Date();
        var dateNow = dateFormat(now, "dd mmmm yyyy");
        //alert(dateNow);

        $("#list").jqGrid({
            url: '<%= Url.Action("ListToday", "Loan") %>',
            datatype: 'json',
            mtype: 'GET',
            colNames: ['Id', 'No PK', 'Nama Pemohon', 'Alamat', 'Unit', 'HD', 'HT', 'Tenor', 'SA', 'TLS', 'Status'],
            colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, hidden: true },
                    { name: 'LoanNo', index: 'LoanNo', width: 100, align: 'left' },
                    { name: 'PersonFirstName', index: 'PersonFirstName', width: 150, align: 'left' },
                    { name: 'AddressLine1', index: 'AddressLine1', width: 200, align: 'left' },
                    { name: 'UnitType', index: 'UnitType', width: 150, align: 'left' },
                    { name: 'LoanBasicPrice', index: 'LoanBasicPrice', width: 100, align: 'right' },
                    { name: 'LoanCreditPrice', index: 'LoanCreditPrice', width: 100, align: 'right' },
                    { name: 'LoanTenor', index: 'LoanTenor', width: 50, align: 'left' },
                    { name: 'SalesmanId', index: 'SalesmanId', width: 50, align: 'left' },
                    { name: 'TLSId', index: 'TLSId', width: 50, align: 'left' },
                    { name: 'LoanStatus', index: 'LoanStatus', width: 75, align: 'left'}],

            pager: $('#listPager'),
            rowNum: 20,
            rowList: [20, 30, 50, 100],
            rownumbers: true,
            sortname: 'Id',
            sortorder: "asc",
            viewrecords: true,
            height: 200,
            caption: 'Permohonan Konsumen per Tanggal ' + dateNow,
            autowidth: true
        });
        jQuery("#list").jqGrid('navGrid', '#listPager',
                 { edit: false, add: false, del: false, search: false, refresh: true },
                 {},
                 {},
                 {},
                 {}
            );
    });       

</script>
<div id="subdialog" title="subStatus">
    <p></p>
</div>
    