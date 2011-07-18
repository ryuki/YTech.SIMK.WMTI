<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <div id='popup'>
        <iframe width='100%' height='340px' id="popup_frame" frameborder="0"></iframe>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {


            $("#popup").dialog({
                autoOpen: false,
                height: 420,
                width: '80%',
                modal: true,
                close: function (event, ui) {
                    $("#list").trigger("reloadGrid");
                }
            });
            $("#dialog").dialog({
                autoOpen: false
            });

            $.jgrid.nav.addtext = "Tambah";
            $.jgrid.nav.edittext = "Edit";
            $.jgrid.nav.deltext = "Hapus";
            $.jgrid.edit.addCaption = "Tambah Kredit Baru";
            $.jgrid.edit.editCaption = "Edit Kredit";
            $.jgrid.del.caption = "Hapus Kredit";
            $.jgrid.del.msg = "Anda yakin menghapus Kredit yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "Loan") %>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['', 'Id', 'LoanId', 'No PK', 'No Account', 'Tgl Pengajuan Kredit', 'Pemohon', 'Surveyor', 'Wilayah', 'Status'],
                colModel: [
                    {
                        name: 'act', index: 'act', width: 250, sortable: false
                    },
                   { name: 'Id', index: 'Id', width: 75, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: true, editable: false },
                   { name: 'LoanId', index: 'LoanId', width: 100, align: 'left', editrules: { required: true, edithidden: false }, hidedlg: true, hidden: true, editable: false },
                    { name: 'LoanNo', index: 'LoanNo', width: 100, align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                    { name: 'LoanCode', index: 'LoanCode', width: 100, align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                    { name: 'LoanSurveyDate', index: 'LoanSurveyDate', width: 90, align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                    { name: 'CustomerName', index: 'CustomerName', width: 125, align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                   { name: 'SurveyorName', index: 'SurveyorName', width: 125, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                   { name: 'ZoneName', index: 'ZoneName', width: 100, align: 'left', editable: true, edittype: 'text', editrules: { required: false, edithidden: true} },
                     { name: 'LoanStatus', index: 'LoanStatus', width: 100, align: 'left', editable: true, edittype: 'text', editrules: { required: false, edithidden: true} }
                   ],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Kredit',
                autowidth: true,
                loadComplete: function () {
                    var ids = jQuery("#list").getDataIDs();
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        var row = $("#list").getRowData(cl);
                        var status = row.LoanStatus;
                        var disableApprove = '';
                        var disableCancel = '';
                        var disablePostpone = '';
                        var disableReject = '';
                        if (status == 'Approve')
                            disableApprove = 'disabled=disabled';
                        if (status == 'Cancel')
                            disableCancel = 'disabled=disabled';
                        if (status == 'Postpone')
                            disablePostpone = 'disabled=disabled';
                        if (status == 'Reject')
                            disableReject = 'disabled=disabled';
                        var be = "<img src='../Content/Images/window16.gif' title='Edit PK' style='cursor: hand;' onClick=\"OpenPopupPK('" + row.LoanId + "');\" />";
                        be = be + "<img src='../Content/Images/window16.gif' title='Edit Survey' style='cursor: hand;' onClick=\"OpenPopup('" + cl + "');\" />";
                        be = be + "<img src='../Content/Images/window16.gif' title='Approve Kredit' style='cursor: hand;' onClick=\"OpenPopupApprove('" + row.LoanId + "');\" " + disableApprove + " />";
                        be = be + "<img src='../Content/Images/window16.gif' title='Cancel Kredit' style='cursor: hand;' onClick=\"OpenPopupCancel('" + row.LoanId + "');\" " + disableCancel + " />";
                        be = be + "<img src='../Content/Images/window16.gif' title='Postpone Kredit' style='cursor: hand;' onClick=\"OpenPopupPostpone('" + row.LoanId + "');\" " + disablePostpone + " />";
                        be = be + "<img src='../Content/Images/window16.gif' title='Reject Kredit' style='cursor: hand;' onClick=\"OpenPopupReject('" + row.LoanId + "');\" " + disableReject + " />";
                        $(this).setRowData(ids[i], { act: be });
                    }
                },
                ondblClickRow: function (rowid, iRow, iCol, e) {

                }
            })
            //            .navGrid('#listPager',
            //                {
            //                    edit: false, add: false, del: true, search: false, refresh: true
            //                },
            //                editDialog,
            //                insertDialog,
            //                deleteDialog
            //            )

            .navButtonAdd('#listPager', {
                caption: "Tambah",
                buttonicon: "ui-icon-add",
                onClickButton: function () {
                    OpenPopup(null);
                },
                position: "first"
            });
            jQuery("#list").jqGrid('navGrid', '#listPager',
                 { edit: false, add: false, del: false, search: false, refresh: true }, //options 
                  {},
                {},
                {},
                {}
            );
        });

        function OpenPopup(id) {
            var url = '<%= Url.Action("Survey", "Loan" ) %>?';
            if (id) {
                url += 'loanSurveyId=' + id;
                url += '&rand=' + (new Date()).getTime();
            }
            $("#popup_frame").attr("src", url);
            $("#popup").dialog("open");
            return false;
        }

        function OpenPopupApprove(loanId) {
            var conf = confirm('Anda yakin meng-approve kredit?');
            
            if (!conf)
                return false;

            var t = $.ajax({ url: '<%= Url.Action("Approve","Loan") %>?loanId=' + loanId, async: false, type: 'POST', cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the Approve.'); } }).responseText;
            //alert(t);
//            var result = $.parseJSON(t);
//            alert(result.Message);
            $("#list").trigger("reloadGrid");
            return false;
        }

        function OpenPopupCancel(loanId) {
            var conf = confirm('Anda yakin meng-cancel kredit?');

            if (!conf)
                return false;

            var t = $.ajax({ url: '<%= Url.Action("Cancel","Loan") %>?loanId=' + loanId, async: false, type: 'POST', cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the Cancel.'); } }).responseText;

            $("#list").trigger("reloadGrid");
            return false;
        }

        function OpenPopupPostpone(loanId) {
            var conf = confirm('Anda yakin mem-postpone kredit?');

            if (!conf)
                return false;

            var t = $.ajax({ url: '<%= Url.Action("Postpone","Loan") %>?loanId=' + loanId, async: false, type: 'POST', cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the Postpone.'); } }).responseText;

            $("#list").trigger("reloadGrid");
            return false;
        }

        function OpenPopupReject(loanId) {
            var conf = confirm('Anda yakin me-reject kredit?');

            if (!conf)
                return false;

            var t = $.ajax({ url: '<%= Url.Action("Reject","Loan") %>?loanId=' + loanId, async: false, type: 'POST', cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the Reject.'); } }).responseText;

            $("#list").trigger("reloadGrid");
            return false;
        }

        function OpenPopupPK(id) {
            var url = '<%= Url.Action( "CustomerRequest", "Loan") %>?';

            if (id) {
                url += 'loanCustomerRequestId=' + id;
                url += '&rand=' + (new Date()).getTime();
            }
            
            $("#popup_frame").attr("src", url);
            $("#popup").dialog("open");

            return false;
        }
    </script>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
</asp:Content>