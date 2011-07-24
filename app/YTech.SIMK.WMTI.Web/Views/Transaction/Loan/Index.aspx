<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <label for="ddlSearchBy">
            Cari berdasar :</label>
        <select id="ddlSearchBy">
            <option value="loan.LoanCode">No Account</option>
            <option value="person.PersonFirstName">Nama</option>
        </select>
        <input id="txtSearch" type="text" />
        <input id="btnSearch" type="button" value="Cari" />
    </div>
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <div id='popup'>
        <iframe width='100%' height='340px' id="popup_frame" frameborder="0"></iframe>
    </div>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
    <div style="width:600px;">
        <div style="float: left;">
        <strong>Keterangan :</strong><br />
            <img src='<%= Url.Content("~/Content/Images/window16.gif") %>' title='Edit PK' style='cursor: hand;
                width: 16px; height: 16px;' alt='Edit PK' />
            = Edit Permohonan Kredit<br />
            <img src='<%= Url.Content("~/Content/Images/edit24_on.gif") %>' title='Edit PK' style='cursor: hand;
                width: 16px; height: 16px;' alt='Edit PK' />
            = Input / Edit Hasil Survey
        </div>
        <div style="float: right;">
            <img src='<%= Url.Content("~/Content/Images/approve24_on.png") %>' title='Edit PK'
                style='cursor: hand; width: 16px; height: 16px;' alt='Edit PK' />
            = Setuju Permohonan Kredit<br />
            <img src='<%= Url.Content("~/Content/Images/reject32_on.png") %>' title='Edit PK'
                style='cursor: hand; width: 16px; height: 16px;' alt='Edit PK' />
            = Tolak Permohonan Kredit<br />
            <img src='<%= Url.Content("~/Content/Images/cancel32_on.png") %>' title='Edit PK'
                style='cursor: hand; width: 16px; height: 16px;' alt='Edit PK' />
            = Pembatalan Permohonan Kredit<br />
            <img src='<%= Url.Content("~/Content/Images/exit32_on.gif") %>' title='Edit PK' style='cursor: hand;
                width: 16px; height: 16px;' alt='Edit PK' />
            = Tunda Proses Permohonan Kredit
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#txtSearch").focus();

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
                postData: {
                    loanStatus: function () { return '<%= Request.QueryString["loanStatus"]%>'; },
                    searchBy: function () { return $('#ddlSearchBy option:selected').val(); },
                    searchText: function () { return $('#txtSearch').val(); }
                },
                datatype: 'json',
                mtype: 'GET',
                colNames: ['', 'Id', 'LoanId', 'No PK', 'No Account', 'Tgl Pengajuan Kredit', 'Pemohon', 'Surveyor', 'Wilayah', 'Status'],
                colModel: [
                    {
                        name: 'act', index: 'act', width: 155, sortable: false
                    },
                   { name: 'Id', index: 'Id', width: 75, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: true, editable: false },
                   { name: 'LoanId', index: 'LoanId', width: 100, align: 'left', editrules: { required: true, edithidden: false }, hidedlg: true, hidden: true, editable: false },
                    { name: 'LoanNo', index: 'LoanNo', width: 100, align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                    { name: 'LoanCode', index: 'LoanCode', width: 100, align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                    { name: 'LoanSurveyDate', index: 'LoanSurveyDate', width: 125, align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
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

                        var disableApprove = "disabled='disabled'";
                        var disableReject = "disabled='disabled'";
                        var disableCancel = "disabled='disabled'";
                        var disablePostpone = "disabled='disabled'";

                        //enable approve if status is survey or postpone
                        if (status == 'Survey' || status == 'Postpone')
                            disableApprove = "";
                        //enable button if status is survey
                        if (status == 'Survey') {
                            disableReject = "";
                            disableCancel = "";
                            disablePostpone = "";
                        }

                        switch (status) {
                            case 'Approve':
                                var be = "";
                                break;

                            case "Cancel":
                                var be = "";
                                break;

                            case "Reject":
                                var be = "";
                                break;

                            case "Postpone":
                                var be = "<img src='../Content/Images/window16.gif' title='Edit PK' style='cursor: hand;width:16px;height:16px;' onClick=\"OpenPopupPK('" + row.LoanId + "');\" />&nbsp;";
                                be = be + "<img src='../Content/Images/edit24_on.gif' title='Edit Survey' style='cursor: hand;width:16px;height:16px;' onClick=\"OpenPopup('" + cl + "');\" />&nbsp; | &nbsp;";

                                be = be + "<img src='../Content/Images/approve24_on.png' title='Approve Kredit' style='cursor: hand;width:16px;height:16px;' onClick=\"OpenPopupApprove('" + row.LoanId + "');\" " + disableApprove + " />&nbsp;";
                                break;

                            default:
                                var be = "<img src='../Content/Images/window16.gif' title='Edit PK' style='cursor: hand;width:16px;height:16px;' onClick=\"OpenPopupPK('" + row.LoanId + "');\" />&nbsp;";
                                be = be + "<img src='../Content/Images/edit24_on.gif' title='Edit Survey' style='cursor: hand;width:16px;height:16px;' onClick=\"OpenPopup('" + cl + "');\" />&nbsp; | &nbsp;";

                                be = be + "<img src='../Content/Images/approve24_on.png' title='Approve Kredit' style='cursor: hand;width:16px;height:16px;' onClick=\"OpenPopupApprove('" + row.LoanId + "');\" " + disableApprove + " />&nbsp;";
                                be = be + "<img src='../Content/Images/reject32_on.png' title='Reject Kredit' style='cursor: hand;width:16px;height:16px;' onClick=\"OpenPopupReject('" + row.LoanId + "');\" " + disableReject + " />&nbsp; | &nbsp;";

                                be = be + "<img src='../Content/Images/cancel32_on.png' title='Cancel Kredit' style='cursor: hand;width:16px;height:16px;' onClick=\"OpenPopupCancel('" + row.LoanId + "');\" " + disableCancel + " />&nbsp;";
                                be = be + "<img src='../Content/Images/exit32_on.gif' title='Tunda Kredit' style='cursor: hand;width:16px;height:16px;' onClick=\"OpenPopupPostpone('" + row.LoanId + "');\" " + disablePostpone + " />";
                        }

                        $(this).setRowData(ids[i], { act: be });
                    }
                    $("img[disabled='disabled']").pixastic("desaturate");
                    //grayscale($("img[disabled='disabled']"));
                    $("img[disabled='disabled']").css("filter", 'progid:DXImageTransform.Microsoft.BasicImage(grayscale=1)');
                    //                    var images = $("img[disabled='disabled']");
                    //                    images.hide();
                    //                    alert('test');
                    //                    //alert(images[0].attr('src'));
                    //                    //images.show();
                    //                    images.complete = function () {
                    //                        Pixastic.process(images, "desaturate", { average: false });
                    //                    };
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

            $('#btnSearch').click(function () {
                $("#list").jqGrid().setGridParam().trigger("reloadGrid");
            });

            $("#txtSearch").keydown(function (event) {
                //if enter pressed
                if (event.keyCode == '13') {
                    $("#list").jqGrid().setGridParam().trigger("reloadGrid");
                }
            });

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
            return PostChangeStatus('Anda yakin menyetujui kredit?','<%= Url.Action("Approve","Loan") %>?loanId=' + loanId);
        }

        function OpenPopupCancel(loanId) {
            return PostChangeStatus('Anda yakin membatalkan kredit?', '<%= Url.Action("Cancel","Loan") %>?loanId=' + loanId);
        }

        function OpenPopupPostpone(loanId) {
            return PostChangeStatus('Anda yakin menunda kredit?', '<%= Url.Action("Postpone","Loan") %>?loanId=' + loanId);
        }

        function OpenPopupReject(loanId) {
            return PostChangeStatus('Anda yakin menolak kredit?', '<%= Url.Action("Reject","Loan") %>?loanId=' + loanId);
        }

        function PostChangeStatus(confirm_msg,posturl) {
            var conf = confirm(confirm_msg);

            if (!conf)
                return false;

            var t = $.ajax({ url: posturl, async: false, type: 'POST', cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the Approve.'); } }).responseText;

            var result = $.parseJSON(t);
            $('#dialog p:first').text(result.Message);
            $("#dialog").dialog("open");

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
</asp:Content>
