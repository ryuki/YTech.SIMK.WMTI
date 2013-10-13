<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <%
        if (false)
        {%>
    <script src="../../../Scripts/jquery-1.6.3-vsdoc.js" type="text/javascript"></script>
    <%
        }%>
    <div class="box">
        <label for="ddlSearchBy">
            No Account :</label>
        <input id="txtLoanCode" type="text" />
        <img src='<%= Url.Content("~/Content/Images/window16.gif") %>' style='cursor: hand;'
            id='imgLoanCode' />
        <%--<a href="#" id="btnSearch" class  <% Html.RenderPartial("CustomerRequestDetail", ViewData); %>="btn info">OK</a>
            <a href="#" id="btnPayment" class="btn info">Pembayaran</a>--%>
        <input id="btnSearch" type="button" value="OK" />
        <input id="btnPayment" type="button" value="Pembayaran" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <input id="btnPaid" type="button" value="Pelunasan" />
        <input id="btnRevert" type="button" value="Penarikan" />
        <input id="hidLoanId" type="hidden" />
    </div>
    <div id="divDetailLoan">
    </div>
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
    <div id='popup'>
        <iframe width='100%' height='100%' id="popup_frame" frameborder="0"></iframe>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtSearch").focus();
            $("#btnPayment").attr('disabled', 'disabled');
            $("#btnPaid").attr('disabled', 'disabled');
            $("#btnRevert").attr('disabled', 'disabled');

            $("#dialog").dialog({
                autoOpen: false
            });
            $("#popup").dialog({
                autoOpen: false,
                height: 360,
                width: 500,
                modal: true,
                close: function (event, ui) {
                    $("#list").trigger("reloadGrid");
                }
            });

            $.jgrid.nav.addtext = "Tambah";
            $.jgrid.nav.edittext = "Edit";
            $.jgrid.nav.deltext = "Hapus";
            $.jgrid.edit.addCaption = "Tambah Angsuran Baru";
            $.jgrid.edit.editCaption = "Edit Angsuran";
            $.jgrid.del.caption = "Hapus Angsuran";
            $.jgrid.del.msg = "Anda yakin menghapus Angsuran yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("ListOK", "Installment") %>',
                postData: {
                    loanCode: function () { return $('#txtLoanCode').val(); }
                },
                datatype: 'json',
                mtype: 'GET',
                colNames: ['',
                            'Angsuran Ke',
                            'Jatuh Tempo',
                            'Jlh Angsuran (Rp)',
                            'Denda (Rp)',
                            'Total Harus Dibayar (Rp)',
                            'Total Bayar (Rp)',
                            'Tgl Bayar',
                            'Lebih Bayar (Rp)',
                            'Kolektor'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: true }, hidedlg: true, hidden: true, editable: true },
                   { name: 'InstallmentNo', index: 'InstallmentNo', width: 200, align: 'left', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                   { name: 'InstallmentMaturityDate', index: 'InstallmentMaturityDate', width: 200, align: 'left', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                   { name: 'InstallmentTotal', index: 'InstallmentTotal', width: 200, align: 'right', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                   { name: 'InstallmentFine', index: 'InstallmentFine', width: 200, align: 'right', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                   { name: 'InstallmentMustPaid', index: 'InstallmentMustPaid', width: 200, align: 'right', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                   { name: 'InstallmentPaid', index: 'InstallmentPaid', width: 200, align: 'right', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                   { name: 'InstallmentPaymentDate', index: 'InstallmentPaymentDate', width: 200, align: 'left', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                   { name: 'InstallmentSisa', index: 'InstallmentSisa', width: 200, align: 'right', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                   { name: 'PersonName', index: 'PersonName', width: 200, align: 'left', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} }
                   ],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 250,
                caption: 'Daftar Angsuran',
                autowidth: true,
                loadComplete: function () {

                },
                ondblClickRow: function (rowid, iRow, iCol, e) {

                }
            }).navGrid('#listPager',
                {
                    edit: false, add: false, del: false, search: false, refresh: true
                }
            );

            $('#btnSearch').click(function () {
                LoadInstallment();
            });
            $("#txtSearch").keydown(function (event) {
                //if enter pressed
                if (event.keyCode == '13') {
                    LoadInstallment();
                }
            });

            $('#imgLoanCode').click(function () {
                OpenPopupInstallment();
            });
            $('#btnPayment').click(function () {
                OpenPopupInstallmentPayment();
            });
            $('#btnPaid').click(function () {
                OpenPopupAcquittance();
            });
            $('#btnRevert').click(function () {
                var loanId = $('#hidLoanId').val();
                var confirm_msg = 'Anda yakin melakukan revert kredit?';
                var posturl = '<%= Url.Action("Revert","Loan") %>?loanId=' + loanId;
                var conf = confirm(confirm_msg);

                if (!conf)
                    return false;

                var t = $.ajax({ url: posturl, async: false, type: 'POST', cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the Approve.'); } }).responseText;

                var result = $.parseJSON(t);
                $('#dialog p:first').text(result.Message);
                $("#dialog").dialog("open");


            });
        });

        function LoadInstallment() {
            $("#divDetailLoan").hide("slow");

            var getLoanIdUrl = '<%= Url.Action("GetLoanIdWithStatusOk","Loan") %>?loanCode=' + $('#txtLoanCode').val();
            var loanId = $.ajax({ url: getLoanIdUrl, async: false, type: 'POST', cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the Approve.'); } }).responseText;
            $('#hidLoanId').val(loanId);
            if (loanId != '') {
                $("#btnPayment").removeAttr('disabled');
                $("#btnPaid").removeAttr('disabled');
                $("#btnRevert").removeAttr('disabled');
                var url = '<%= Url.Action("DetailLoan", "Installment") %>?loanCode=' + $('#txtLoanCode').val() + '&rand=' + (new Date()).getTime();
                $("#divDetailLoan").load(url);
                $("#divDetailLoan").show("slow");
            }
            else {
                alert('Kredit dengan No Account ' + $('#txtLoanCode').val() + ' tidak ada atau status kredit tidak sedang berjalan.');
                $("#btnPayment").attr('disabled', 'disabled');
                $("#btnPaid").attr('disabled', 'disabled');
                $("#btnRevert").attr('disabled', 'disabled');
            }
            $("#list").jqGrid().setGridParam().trigger("reloadGrid");
        }

        function OpenPopupInstallment() {
            var src = '<%= Url.Action("Search", "Installment") %>';
            src = src + '?rand=' + (new Date()).getTime();
            $("#popup_frame").attr("src", src);
            $("#popup").dialog("open");
            return false;
        }
        function OpenPopupInstallmentPayment() {
            var src = '<%= Url.Action("Payment", "Installment") %>';
            src = src + '?loanCode=' + $("#txtLoanCode").val();
            src = src + '&rand=' + (new Date()).getTime();
            $("#popup_frame").attr("src", src);
            $("#popup").dialog("open");

            return false;
        }
        function OpenPopupAcquittance() {
            var src = '<%= Url.Action("Acquittance", "Installment") %>';
            src = src + '?loanCode=' + $("#txtLoanCode").val();
            src = src + '&rand=' + (new Date()).getTime();
            $("#popup_frame").attr("src", src);
            $("#popup").dialog("open");

            return false;
        }   
    </script>
</asp:Content>
