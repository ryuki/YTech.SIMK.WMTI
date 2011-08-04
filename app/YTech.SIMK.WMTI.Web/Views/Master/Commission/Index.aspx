<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <script type="text/javascript">

        $(document).ready(function () {

            $("#dialog").dialog({
                autoOpen: false
            });


            var editDialog = {
                url: '<%= Url.Action("Update", "Commission") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true

                , onclickSubmit: function (params) {
                    var ajaxData = {};

                    var list = $("#list");
                    var selectedRow = list.getGridParam("selrow");
                    rowData = list.getRowData(selectedRow);
                    ajaxData = { Id: rowData.Id };

                    return ajaxData;
                }
                , afterShowForm: function (eparams) {
                    $('#Id').attr('disabled', 'disabled');
                    $('#CommissionStartDate').datepicker();
                    $('#CommissionEndDate').datepicker();
                    $('#CommissionValue').autoNumeric();
                    $('#CommissionValue').attr("style", "text-align:right;");
                }
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
            };
            var insertDialog = {
                url: '<%= Url.Action("Insert", "Commission") %>?department=<%= Request.QueryString["department"]%>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , afterShowForm: function (eparams) {
                    $('#Id').removeAttr('disabled');
                    $('#CommissionStartDate').datepicker();
                    $('#CommissionEndDate').datepicker();
                    $('#CommissionValue').autoNumeric();
                    $('#CommissionValue').attr("style", "text-align:right;");
                }
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
                , width: "400"
            };
            var deleteDialog = {
                url: '<%= Url.Action("Delete", "Commission") %>'
                , modal: true
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
            };

            $.jgrid.nav.addtext = "Tambah";
            $.jgrid.nav.edittext = "Edit";
            $.jgrid.nav.deltext = "Hapus";
            $.jgrid.edit.addCaption = "Tambah Komisi Baru";
            $.jgrid.edit.editCaption = "Edit Komisi";
            $.jgrid.del.caption = "Hapus Komisi";
            $.jgrid.del.msg = "Anda yakin menghapus Komisi yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "Commission") %>',
                postData: { department: function () { return '<%= Request.QueryString["department"]%>'; } },
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Mulai Tanggal', 'Sampai Tanggal', 'Target'],
                colModel: [
                    { name: 'CommissionStartDate', index: 'CommissionStartDate', width: 100, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                    { name: 'CommissionEndDate', index: 'CommissionEndDate', width: 100, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                    { name: 'CommissionValue', index: 'CommissionValue', width: 100, align: 'right', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'}}],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Komisi',
                autowidth: true,
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    $("#list").editGridRow(rowid, editDialog);
                },
                subGrid: true,
                subGridRowExpanded: function (subGridId, rowId) {

                    var subGridTableId = subGridId + "_t";
                    var pagerId = "p_" + subGridTableId;

                    var addSubDialog = {
                        url: '<%= Url.Action("InsertSub", "Commission") %>?commissionId=' + rowId,
                        closeAfterAdd: true,
                        closeAfterEdit: true,
                        modal: true,
                        afterComplete: function (response, postdata, formid) {
                            $('#dialog p:first').text(response.responseText);
                            $("#dialog").dialog("open");
                        }, 
                        width: "400"
                    };

                    var editSubDialog = {
                        url: '<%= Url.Action("UpdateSub", "Commission") %>'
                        , closeAfterAdd: true
                        , closeAfterEdit: true
                        , modal: true

                        , onclickSubmit: function (params) {
                            var ajaxData = {};

                            var list = $("#" + subGridTableId);
                            var selectedRow = list.getGridParam("selrow");
                            rowData = list.getRowData(selectedRow);
                            ajaxData = { Id: rowData.Id };

                            return ajaxData;
                        }
                        , afterShowForm: function (eparams) {
                            //$('#Id').attr('disabled', 'disabled');
                            // alert(eparams);
                        }
                        , width: "400"
                        , afterComplete: function (response, postdata, formid) {
                            $('#dialog p:first').text(response.responseText);
                            $("#dialog").dialog("open");
                        }
                    };

                    var deleteSubDialog = {
                        url: '<%= Url.Action("DeleteSub", "Commission") %>'
                        , modal: true
                        , width: "400"
                        , afterComplete: function (response, postdata, formid) {
                            $('#dialog p:first').text(response.responseText);
                            $("#dialog").dialog("open");
                        }
                    };

                    $("#" + subGridId).html("<table id='" + subGridTableId + "' class='scroll'></table><div id='" + pagerId + "' class='scroll'></div>");
                    $("#" + subGridTableId).jqGrid({
                        url: '<%= Url.Action("ListSub", "Commission") %>',
                        postData: { commissionId: function () { return rowId; } },
                        datatype: 'json',
                        mtype: 'GET',
                        colNames: ['Id', 'Level', 'Penjualan dari', 'Penjualan Sampai', 'Komisi'],
                        colModel: [
                            { name: 'Id', index: 'Id', width: 75, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: true, editable: false },
                            { name: 'DetailType', index: 'DetailType', align: 'right', editable: true, edittype: 'text' },
                            { name: 'DetailLowTarget', index: 'DetailLowTarget', align: 'right', editable: true, edittype: 'text' },
                            { name: 'DetailHighTarget', index: 'DetailHighTarget', align: 'right', editable: true, edittype: 'text' },
                            { name: 'DetailValue', index: 'DetailValue', align: 'right', editable: true, edittype: 'text' }
                        ],
                        pager: pagerId,
                        rowNum: 20,
                        rowList: [20, 30, 50, 100],
                        rownumbers: false,
                        sortname: 'Id',
                        sortorder: "asc",
                        viewrecords: true,
                        caption: 'Detail Komisi'
                    });
                    $("#" + subGridTableId).jqGrid('navGrid', "#" + pagerId,
                        { edit: true, add: true, del: true, search: false, refresh: true },
                        editSubDialog, addSubDialog, deleteSubDialog
                    );
            }
        });
        jQuery("#list").jqGrid('navGrid', '#listPager',
                 { edit: true, add: true, del: true, search: false, refresh: true }, //options 
                  editDialog,
                insertDialog,
                deleteDialog,
                {}
            );
    });       

    </script>
    <div id="dialog" title="Status">
        <p></p>
    </div>
</asp:Content>
