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
                url: '<%= Url.Action("Update", "ZoneEmployee") %>'
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
                    $('#StartDate').datepicker();
                    $('#EndDate').datepicker();
                }
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
            };
            var insertDialog = {
                url: '<%= Url.Action("Insert", "ZoneEmployee") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , afterShowForm: function (eparams) {
                    $('#Id').removeAttr('disabled');
                    $('#StartDate').datepicker();
                    $('#EndDate').datepicker();
                }
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
                , width: "400"
            };
            var deleteDialog = {
                url: '<%= Url.Action("Delete", "ZoneEmployee") %>'
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
            $.jgrid.edit.addCaption = "Tambah Pembagian Wilayah Baru";
            $.jgrid.edit.editCaption = "Edit Pembagian Wilayah";
            $.jgrid.del.caption = "Hapus Pembagian Wilayah";
            $.jgrid.del.msg = "Anda yakin menghapus Pembagian Wilayah yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "ZoneEmployee") %>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Id', 'Nama Surveyor/Kolektor', 'Nama Surveyor/Kolektor', 'Mulai Tanggal', 'Sampai Tanggal', 'Wilayah', 'Wilayah'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: false, edithidden: false }, hidedlg: true, hidden: true, editable: false },
                    { name: 'EmployeeId', index: 'EmployeeId', width: 200, align: 'left', editable: true, edittype: 'select', editrules: { edithidden: true, required: true }, hidden: true, formoptions: { elmsuffix: ' *'} },
                    { name: 'PersonName', index: 'PersonName', width: 200, align: 'left', editable: false, edittype: 'select', editrules: { edithidden: true} },
                    { name: 'StartDate', index: 'StartDate', width: 100, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                    { name: 'EndDate', index: 'EndDate', width: 150, sortable: false, align: 'left', editable: true, edittype: 'text', editrules: { required: false }, formoptions: { elmsuffix: ' *'} },
                    { name: 'ZoneId', index: 'ZoneId', width: 200, align: 'left', editable: true, edittype: 'select', editrules: { edithidden: true, required: true }, hidden: true, formoptions: { elmsuffix: ' *'} },
                    { name: 'ZoneName', index: 'ZoneName', width: 200, align: 'left', editable: false, edittype: 'select', editrules: { edithidden: true}}],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Pembagian Wilayah Kerja',
                autowidth: true,
                loadComplete: function () {
                    $('#list').setColProp('EmployeeId', { editoptions: { value: employees} });
                    $('#list').setColProp('ZoneId', { editoptions: { value: zones} });
                },
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    $("#list").editGridRow(rowid, editDialog);
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

        var employees = $.ajax({ url: '<%= Url.Action("GetListSuCol","Employee") %>', async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the Employee.'); } }).responseText;  
        var zones = $.ajax({ url: '<%= Url.Action("GetList","Zone") %>', async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the Zone.'); } }).responseText;  

    </script>
    <div id="dialog" title="Status">
        <p></p>
    </div>
</asp:Content>
