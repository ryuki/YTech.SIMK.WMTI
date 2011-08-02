<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <br />
    <table id="subGrid" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="subGridPager" class="scroll" style="text-align: center;">
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
                    // alert(eparams);
                }
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
            };
            var insertDialog = {
                url: '<%= Url.Action("Insert", "Commission") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , afterShowForm: function (eparams) {
                    $('#Id').removeAttr('disabled');
                    $('#CommissionStartDate').datepicker({ dateFormat: "dd-MM-yy" });
                    $('#CommissionEndDate').datepicker({ dateFormat: "dd-MM-yy" });
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
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Detail Komisi', 'Mulai Tanggal', 'Sampai Tanggal', 'Target'],
                colModel: [
                    { name: 'act', index: 'act', width: 30, sortable: false },
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
                caption: 'Komisi Team Leader Sales',
                autowidth: true,
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    $("#list").editGridRow(rowid, editDialog);
                },
                subGrid: true,
                subGridRowExpanded: function
                //subGridUrl: '<%= Url.Action("ListForSubGrid", "Commission") %>',
                //subGridModel: [{name: ['Level','Penjualan dari','Penjualan Sampai','Komisi'],
                //                width: [100, 100, 100, 100],
                //                align: ['right','right','right','right'],
                //                params: ['Id']}]
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
