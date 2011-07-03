<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<CRFormViewModel>" %>

<% if (false)
   { %>
<script src="../../../Scripts/jquery-1.5.2-vsdoc.js" type="text/javascript"></script>
<% } %>
<% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
                                           InsertionMode = InsertionMode.Replace,
                                           OnSuccess = "onSavedSuccess"
                                       }

          ))
   {%>
        <div id="formArea">
            <%=Html.AntiForgeryToken()%>
            <table>
                <tr>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <label for="Id">
                                        No PK:</label>
                                </td>
                                <td>
                                    <%= Model.CanEditId ? Html.TextBox("LoanNo", Model.Loan.LoanNo ?? string.Empty, new { @style = "width:150px" }) :
                                                                 Html.TextBox("LoanNo", Model.Loan.LoanNo ?? string.Empty, new { @readonly = Model.CanEditId ? "true" : "false", @style = "width:150px" })
                                    %>
                                    <%= Html.ValidationMessage("Id")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="TLSId">
                                        Team Name :</label>
                                </td>
                                <td>
                                    <%= Html.DropDownList("TLSId", Model.TLSList)%>
                                    <%= Html.ValidationMessage("TLSId")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="SalesmanId">
                                        SA CODE :</label>
                                </td>
                                <td>
                                    <%= Html.DropDownList("SalesmanId", Model.SalesmanList)%>
                                    <%= Html.ValidationMessage("SalesmanId")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="SurveyorId">
                                        Survey Code :</label>
                                </td>
                                <td>
                                    <%= Html.DropDownList("SurveyorId", Model.SurveyorList)%>
                                    <%= Html.ValidationMessage("SurveyorId")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="LoanBasicPrice">
                                        HD :</label>
                                </td>
                                <td>
                                    <%= Html.TextBox("LoanBasicPrice", Model.Loan.LoanBasicPrice, new { @style = "width:150px" })%>
                                    <%= Html.ValidationMessage("LoanBasicPrice")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="LoanCreditPrice">
                                        HT :</label>
                                </td>
                                <td>
                                    <%= Html.TextBox("LoanCreditPrice", Model.Loan.LoanCreditPrice, new { @style = "width:150px" })%>
                                    <%= Html.ValidationMessage("LoanCreditPrice")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="LoanSubmissionDate">
                                        Tanggal :</label>
                                </td>
                                <td>
                                 <%= Html.TextBox("LoanSubmissionDate", Model.Loan.LoanSubmissionDate.HasValue ? Model.Loan.LoanSubmissionDate.Value.ToString(CommonHelper.DateFormat) : null, new { @style = "width:100px" })%>
                                    <%= Html.ValidationMessage("LoanSubmissionDate")%>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <div>
                            <span id="toolbar" class="ui-widget-header ui-corner-all">
                                <button id="Save" type="submit">
                                    Simpan</button>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <div id="status">
                        </div>
                        <div class="ui-state-highlight ui-corner-all" style="padding: 5pt; margin-bottom: 5pt;
                            display: none;" id="error">
                            <p>
                                <span class="ui-icon ui-icon-error" style="float: left; margin-right: 0.3em;"></span>
                                <span id="error_msg"></span>.<br clear="all" />
                            </p>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
<%
    }
%>

<div id="dialog" title="Status">
    <p>
    </p>
</div>

<script language="javascript" type="text/javascript">
    function onSavedSuccess(e) {
        if (e) {
            var json = e.get_response().get_object();
            var success = json.Success;
            var msg = json.Message;
            if (success) {
                $("#Save").attr('disabled', 'disabled');
                $('#status').html(msg);
            }
            else {
                $("#Save").removeAttr('disabled');
                if (msg) {
                    $('#dialog p:first').text(msg);
                    $("#dialog").dialog("open");
                }
            }
        }
    }

    function ajaxValidate() {
        var errorimg = '<%= Url.Content("~/Content/Images/cross.gif") %>';
        return $('form').validate({
            rules: {
                "Id": { required: true
                },
                "PersonFirstName": { required: true }
            },
            messages: {
                "Id": { required: "<img id='Iderror' src='" + errorimg + "' hovertext='No PK harus diisi' />"
                },
                "PersonFirstName": "<img id='PersonFirstNameerror' src='" + errorimg + "' hovertext='Nama harus diisi' />"
            },
            invalidHandler: function (form, validator) {
                var errors = validator.numberOfInvalids();
                if (errors) {
                    var message = "Validasi data kurang";
                    $("div#error span#error_msg").html(message);
                    $("div#error").dialog("open");
                    return false;
                } else {
                    $("div#error").dialog("close");
                }
            },
            errorPlacement: function (error, element) {
                error.insertAfter(element);
                //	generateTooltips();
            }
        }).form();
    }

    $(document).ready(function () {
        $("form").mouseover(function () {
            generateTooltips();
        });

        $("#dialog").dialog({
            autoOpen: false
        });

        $("div#error").dialog({
            autoOpen: false
        });
    });

    //function to generate tooltips
    function generateTooltips() {
        //make sure tool tip is enabled for any new error label
        $("img[id*='error']").tooltip({
            showURL: false,
            opacity: 0.99,
            fade: 150,
            positionRight: true,
            bodyHandler: function () {
                return $("#" + this.id).attr("hovertext");
            }
        });
        //make sure tool tip is enabled for any new valid label
        $("img[src*='tick.gif']").tooltip({
            showURL: false,
            bodyHandler: function () {
                return "OK";
            }
        });
    }
</script>
