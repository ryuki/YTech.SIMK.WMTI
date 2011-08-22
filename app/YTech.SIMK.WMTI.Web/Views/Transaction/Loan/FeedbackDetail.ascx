<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<FeedbackViewModel>" %>

<% if (false)
   { %>
<script src="../../../Scripts/jquery-1.6.2-vsdoc.js" type="text/javascript"></script>
<script src="../../../Scripts/jquery.validate-vsdoc.js" type="text/javascript"></script>
<% } %>

<% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           InsertionMode = InsertionMode.Replace,
                                           OnBegin = "ajaxValidate",
                                           OnSuccess = "onSavedSuccess"
                                       }

          ))
   {%>

        <div id="formArea">
            <%=Html.AntiForgeryToken()%>
            <table>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <label for="LoanFeedbackTypeCommon">Umum :</label>
                                </td>
                                <td>
                                    <textarea id="LoanFeedbackTypeCommon" rows="3" cols="50"></textarea>
                                    <%= Html.TextArea("LoanFeedbackCommon", Model.LoanFeedbackCommon) %>
                                    <%= Html.ValidationMessage("LoanFeedbackCommon")%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="LoanFeedbackTypePaymentCharacter">Karakter Pembayaran :</label>
                                </td>
                                <td>
                                    <textarea id="LoanFeedbackTypePaymentCharacter" rows="3" cols="50"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="LoanFeedbackTypeProblem">Problem :</label>
                                </td>
                                <td>
                                    <textarea id="LoanFeedbackTypeProblem" rows="3" cols="50"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="LoanFeedbackTypeSolution">Solusi :</label>
                                </td>
                                <td>
                                    <textarea id="LoanFeedbackTypeSolution" rows="3" cols="50"></textarea>
                                </td>
                            </tr>
                        </table>
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
                    <td colspan="2"></td>
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
                $("#list").trigger("reloadGrid");
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