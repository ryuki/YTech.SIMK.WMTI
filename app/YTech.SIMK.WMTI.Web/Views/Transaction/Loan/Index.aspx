<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<LoanViewModel>" %>

<%@ Register TagPrefix="uc" TagName="Legend" Src="~/Views/Transaction/Loan/Legend.ascx" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .imgButton
        {
            cursor: hand;
            width: 16px;
            height: 16px;
        }
    </style>
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <label for="ddlSearchBy">
                        Cari berdasar :</label>
                </td>
                <td>
                    <select id="ddlSearchBy">
                        <option value="loan.LoanCode">No Account</option>
                        <option value="person.PersonFirstName">Nama</option>
                    </select>
                    <input id="txtSearch" type="text" />
                </td>
                <td>
                    <label for="CollectorId">
                        Kolektor :</label>
                </td>
                <td>
                    <%= Html.DropDownList("CollectorId", Model.CollectorList)%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="ZoneId">
                        Wilayah :</label>
                </td>
                <td>
                    <%= Html.DropDownList("ZoneId", Model.ZoneList)%>
                </td>
                <td>
                    <label for="TLSId">
                        LTS :</label>
                </td>
                <td>
                    <%= Html.DropDownList("TLSId", Model.TLSList)%>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="ddlMonth">
                        Periode Pengajuan Kredit :</label>
                </td>
                <td>
                    <select id="ddlMonth">
                        <option value=""></option>
                        <option value="1">Jan</option>
                        <option value="2">Feb</option>
                        <option value="3">Mar</option>
                        <option value="4">Apr</option>
                        <option value="5">Mei</option>
                        <option value="6">Jun</option>
                        <option value="7">Jul</option>
                        <option value="8">Agust</option>
                        <option value="9">Sep</option>
                        <option value="10">Okt</option>
                        <option value="11">Nov</option>
                        <option value="12">Des</option>
                    </select>
                    <select id="ddlYear">
                        <option value=""></option>
                        <option value="<%= DateTime.Today.Year %>">
                            <%= DateTime.Today.Year %></option>
                        <option value="<%= DateTime.Today.AddYears(-1).Year %>">
                            <%= DateTime.Today.AddYears(-1).Year %></option>
                        <option value="<%= DateTime.Today.AddYears(-2).Year %>">
                            <%= DateTime.Today.AddYears(-2).Year %></option>
                    </select>
                </td>
                <td>
                    <label for="SalesmanId">
                        SA :</label>
                </td>
                <td>
                    <%= Html.DropDownList("SalesmanId", Model.SalesmanList)%>
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <input id="btnSearch" type="button" value="Cari" />
                </td>
            </tr>
        </table>
    </div>
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <div id="total" style="text-align: right; width: 860px;">
        <p style="font-weight: bold;">
        </p>
    </div>
    <div id='popup'>
        <iframe width='100%' height='340px' id="popup_frame" frameborder="0"></iframe>
    </div>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
    <div id="LoanOK" title="Tanggal Cicilan Pertama">
        <input id="hidLoanId" type="hidden" />
        Tanggal Cicilan Pertama :
        <br />
        <input id="txtFirstInstallment" name="txtFirstInstallment" />
        <br />
        <input id="btnOk" type="button" value="Simpan" />
    </div>
    <% if (Model.ShowLegend)
       { %>
    <uc:Legend ID="ucLegendNotEmpty" runat="server" />
    <% } %>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#txtSearch").focus();
            $("#txtFirstInstallment").datepicker();

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
            $("#LoanOK").dialog({
                autoOpen: false
            });

            var loanStatus = '<%= Request.QueryString["loanStatus"]%>';
               // alert(loanStatus);
            var showLongDayLate = true ;
            if (loanStatus == 'LatePay')
                showLongDayLate=false;
                //alert(showLongDayLate);

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
                    searchText: function () { return $('#txtSearch').val(); },
                    zoneId: function () { return $('#ZoneId option:selected').val(); },
                    collectorId: function () { return $('#CollectorId option:selected').val(); },
                    tLSId: function () { return $('#TLSId option:selected').val(); },
                    salesmanId: function () { return $('#SalesmanId option:selected').val(); },
                    month: function () { return $('#ddlMonth option:selected').val(); },
                    year: function () { return $('#ddlYear option:selected').val(); }
                },
                datatype: 'json',
                mtype: 'GET',
                colNames: ['', 'Id', 'LoanId', 'No PK', 'No Account', 'Tgl Pengajuan Kredit', 'Pemohon', 'Surveyor', 'Angsuran (Rp)', 'Status', 'Tunggakan'],
                colModel: [
                    {
                        name: 'act', index: 'act', width: 200, sortable: false
                    },
                   { name: 'Id', index: 'Id', width: 75, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: true, editable: false },
                   { name: 'LoanId', index: 'LoanId', width: 100, align: 'left', editrules: { required: true, edithidden: false }, hidedlg: true, hidden: true, editable: false },
                    { name: 'LoanNo', index: 'LoanNo', width: 100, align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                    { name: 'LoanCode', index: 'LoanCode', width: 95, align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                    { name: 'LoanSubmissionDate', index: 'LoanSubmissionDate', width: 125, align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                    { name: 'CustomerName', index: 'CustomerName', width: 125, align: 'left', editable: false, edittype: 'text', editrules: { required: false} },
                   { name: 'SurveyorName', index: 'SurveyorName', width: 125, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                   { name: 'LoanBasicInstallment', index: 'LoanBasicInstallment', width: 100, align: 'right', editable: true, edittype: 'text', editrules: { required: false, edithidden: true} },
                     { name: 'LoanStatus', index: 'LoanStatus', width: 60, align: 'left', editable: true, edittype: 'text', editrules: { required: false, edithidden: !showLongDayLate}, hidden : !showLongDayLate },
                     { name: 'LongDayLate', index: 'LongDayLate', width: 130, align: 'left', editable: true, edittype: 'text', editrules: { required: false, edithidden: showLongDayLate}, hidden : showLongDayLate }
                   ],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'LoanSubmissionDate',
                sortorder: "desc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Kredit',
                autowidth: true,
                loadComplete: function () {
                    CalculateTotal();
                    SetLoanFunctionButton();                    
                },
<% if (Model.ShowInstallmentSubGrid)
   { %>
                subGrid: true,
                subGridRowExpanded: function (subGridId, rowId) {

                    var subGridTableId = subGridId + "_t";
                    var pagerId = "p_" + subGridTableId;
                    var row = $("#list").getRowData(rowId);
                    
                    $("#" + subGridId).html("<table id='" + subGridTableId + "' class='scroll'></table><div id='" + pagerId + "' class='scroll'></div>");
                    $("#" + subGridTableId).jqGrid({
                        url: '<%= Url.Action("ListAll", "Installment") %>',
                        postData: { loanCode: function () { return row.LoanCode; } },
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
                           { name: 'InstallmentNo', index: 'InstallmentNo', width: 25, align: 'left', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                           { name: 'InstallmentMaturityDate', index: 'InstallmentMaturityDate', width: 100, align: 'left', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                           { name: 'InstallmentTotal', index: 'InstallmentTotal', width: 100, align: 'right', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                           { name: 'InstallmentFine', index: 'InstallmentFine', width: 100, align: 'right', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                           { name: 'InstallmentMustPaid', index: 'InstallmentMustPaid', width: 100, align: 'right', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                           { name: 'InstallmentPaid', index: 'InstallmentPaid', width: 100, align: 'right', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                           { name: 'InstallmentPaymentDate', index: 'InstallmentPaymentDate', width: 100, align: 'left', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} },
                           { name: 'InstallmentSisa', index: 'InstallmentSisa', width: 100, align: 'right', editable: false, edittype: 'text', editrules: { required: false, edithidden: true}},
                           { name: 'PersonName', index: 'PersonName', width: 100, align: 'left', editable: false, edittype: 'text', editrules: { required: false, edithidden: true} }
                           ],
                        pager: pagerId,
                        rowNum: 20,
                        rowList: [20, 30, 50, 100],
                        rownumbers: false,
                        sortname: 'Id',
                        sortorder: "asc",
                        viewrecords: true,
                        caption: 'Daftar Angsuran'
                    });
                    $("#" + subGridTableId).jqGrid('navGrid', "#" + pagerId,
                        { edit: false, add: false, del: false, search: false, refresh: true }
                    );
                },
<%
   } %>
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

            $('#btnOk').click(function () {
            var loanId=$("#hidLoanId").val();
            var installDate=$("#txtFirstInstallment").val();
//            alert(loanId);
//            alert(installDate);
            if (installDate == '')
            alert('Tanggal Cicilan pertama harus diisi.');
            else
            {
                $("#LoanOK").dialog("close");
                return PostChangeStatus('Anda yakin kredit Ok?', '<%= Url.Action("Oke","Loan") %>?loanId=' + loanId + '&installDate=' + installDate);
                }
            });

        });

        function CalculateTotal()
        {
            var loanStatus = '<%= Request.QueryString["loanStatus"]%>';
            var searchBy = $('#ddlSearchBy option:selected').val(); 
            var searchText = $('#txtSearch').val(); 
            var zoneId = $('#ZoneId option:selected').val(); 
            var collectorId = $('#CollectorId option:selected').val();
            var tLSId= $('#TLSId option:selected').val(); 
            var  salesmanId= $('#SalesmanId option:selected').val(); 
            var  month= $('#ddlMonth option:selected').val(); 
            var  year= $('#ddlYear option:selected').val(); 
            
            var posturl = '<%= Url.Action( "GetTotal", "Loan") %>';
            posturl = posturl + '?loanStatus=' + loanStatus;
            posturl = posturl + '&searchBy=' + searchBy;
            posturl = posturl + '&searchText=' + searchText;
            posturl = posturl + '&zoneId=' + zoneId;
            posturl = posturl + '&collectorId=' + collectorId;
            posturl = posturl + '&tLSId=' + tLSId;
            posturl = posturl + '&salesmanId=' + salesmanId;
            posturl = posturl + '&month=' + month;
            posturl = posturl + '&year=' + year;
            var total = $.ajax({ url: posturl, async: false, type: 'POST', cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the page.'); } }).responseText;
             $('#total p:first').text("Total Angsuran : Rp." + total);
        }

        function SetLoanFunctionButton()
        {
            var ids = jQuery("#list").getDataIDs();

            var disableApprove = "disabled='disabled'";
            var disableReject = "disabled='disabled'";
            var disableCancel = "disabled='disabled'";
            var disablePostpone = "disabled='disabled'";
            var disableOk = "disabled='disabled'";

            var loanStatus = '<%= Request.QueryString["loanStatus"]%>';
            //alert(loanStatus);

            var status;
            var row;
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                row = $("#list").getRowData(cl);
                status = row.LoanStatus;
                        
                //enable approve if status is survey or postpone
                if (status == 'Survey' || status == 'Postpone')
                    disableApprove = "";
                //enable cancel if status is postpone
                if (status == 'Postpone')
                    disableCancel = "";
                //enable ok if status is approve
                if (status == 'Approve')
                    disableOk = "";
                    disableCancel = "";
                    disablePostpone = "";
                //enable button if status is survey
                if (status == 'Survey') {
                    disableReject = "";
                    disableCancel = "";
                    disablePostpone = "";
                }

                var separator = " | &nbsp;"; 
                var del = "<img src='../Content/Images/cross.gif' title='Hapus Kredit' class='imgButton' onClick=\"OpenPopupDelete('" + row.LoanId + "');\" />&nbsp;";
                var survey = "<img src='../Content/Images/edit24_on.gif' title='Edit Survey' class='imgButton' onClick=\"OpenPopup('" + cl + "');\" />&nbsp;";
                var ok = "<img src='../Content/Images/ok24_on.png' title='Kredit Oke' class='imgButton' onClick=\"OpenPopupOke('" + row.LoanId + "');\" " + disableOk + " />";
                var notes = "<img src='../Content/Images/Note-48.png' title='Catatan' class='imgButton' onClick=\"OpenPopupNotes('" + row.LoanId + "');\" />&nbsp;";
                var edit = "<img src='../Content/Images/window16.gif' title='Edit PK' class='imgButton' onClick=\"OpenPopupPK('" + row.LoanId + "');\" />&nbsp;";
                var approve = "<img src='../Content/Images/approve24_on.png' title='Approve Kredit' class='imgButton' onClick=\"OpenPopupApprove('" + row.LoanId + "');\" " + disableApprove + " />&nbsp;";
                var cancel = "<img src='../Content/Images/cancel32_on.png' title='Cancel Kredit' class='imgButton' onClick=\"OpenPopupCancel('" + row.LoanId + "');\" " + disableCancel + " />&nbsp;";
                var reject = "<img src='../Content/Images/reject32_on.png' title='Reject Kredit' class='imgButton' onClick=\"OpenPopupReject('" + row.LoanId + "');\" " + disableReject + " />&nbsp;";
                var postpone = "<img src='../Content/Images/exit32_on.gif' title='Tunda Kredit' class='imgButton' onClick=\"OpenPopupPostpone('" + row.LoanId + "');\" " + disablePostpone + " />&nbsp;";
                var sp = "<a href='#' onClick=\"OpenPopupSP('" + row.LoanId + "');\">Cetak SP</a>&nbsp;";
                var tarik = "<a href='#' onClick=\"OpenPopupTarik('" + row.LoanId + "');\">Penarikan</a>&nbsp;";
                        
                var be = "";
                <% if (Request.IsAuthenticated && User.Identity.Name.ToLower() == "admin")
                   {
%>
be = del + separator;
    <%
                   }%>
                
                switch (status) {
                    case 'Approve':
                        be = be + survey + separator + cancel + postpone;
                        if (loanStatus == 'Approve')
                            be = be + ok;
                        break;

                    case "OK":
                        if (loanStatus == 'OK' || loanStatus == '')
                            be = be + survey + notes;
                        else if (loanStatus == 'LatePay')
                            be = sp + separator + tarik;
                        break;

                    case "Cancel":
                        be = "";
                        break;

                    case "Reject":
                        be = "";
                        break;

                    case "Postpone":
                        be = be + edit + survey + separator + approve + cancel;
                        break;

                    case "Paid":
                        be = "";
                        break;

                    case "Revert":
                        be = "";
                        break;                        

                    case "Delete":
                        be = "";
                        break;

                    default:
                        be = be + edit + survey + separator + approve + reject + separator + cancel + postpone;
                }
                //alert(be);
                $("#list").setRowData(ids[i], { act: be });
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
        }

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

        function OpenPopupOke(loanId) {
         $("#hidLoanId").val(loanId);
         $("#txtFirstInstallment").val("");
         $("#LoanOK").dialog("open");
            //return PostChangeStatus('Anda yakin kredit Ok?', '<%= Url.Action("Oke","Loan") %>?loanId=' + loanId);
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

        function OpenPopupDelete(loanId) {
            return PostChangeStatus('Anda yakin menghapus kredit?\nData yang telah dihapus tidak dapat dikembalikan!!', '<%= Url.Action("Delete","Loan") %>?loanId=' + loanId);
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

        function OpenPopupNotes(loanId) {
            var url = '<%= Url.Action( "FeedBack", "Loan") %>?';

            if (loanId) {
                url += 'loanId=' + loanId;
                url += '&rand=' + (new Date()).getTime();
            }

            $("#popup_frame").attr("src", url);
            $("#popup").dialog("open");

            return false;
        }

        function OpenPopupSP(loanId) {
            return Print('Anda yakin mencetak Surat Peringatan?', '<%= Url.Action("Print","Loan") %>?loanId=' + loanId + '&letterType=SP');
        }

        function OpenPopupTarik(loanId) {
             return Print('Anda yakin melakukan penarikan?', '<%= Url.Action("Print","Loan") %>?loanId=' + loanId + '&letterType=Tarik');
        }

         function Print(confirm_msg,posturl) {
            var conf = confirm(confirm_msg);

            if (!conf)
                return false;

            var printPage = $.ajax({ url: posturl, async: false, type: 'POST', cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the page.'); } }).responseText;

            var result = $.parseJSON(printPage);
              var urlreport ='<%= ResolveUrl("~/ReportViewer.aspx?rpt=") %>' + result.UrlReport;
        //alert(urlreport);
        window.open(urlreport);
            return false;
        }
    </script>
</asp:Content>
