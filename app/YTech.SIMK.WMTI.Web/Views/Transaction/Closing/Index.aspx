<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.Master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<ClosingViewModel>" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% if (false)
       { %>
    <script src="../../Scripts/jquery-1.6.3-vsdoc.js" type="text/javascript"></script>
    <% } %>
    <% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
                                           //InsertionMode = InsertionMode.Replace,
                                           OnSuccess = "onSavedSuccess"
                                       }

          ))
       {%>
    <%=Html.AntiForgeryToken()%>
    <table>
        <tr>
            <td>
                <label for="StartDate">
                    Dari Tanggal :</label>
            </td>
            <td>
                <%= Html.TextBox("StartDate", CommonHelper.ConvertToString(Model.StartDate))%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="EndDate">
                    Sampai Tanggal :</label>
            </td>
            <td>
                <%= Html.TextBox("EndDate", CommonHelper.ConvertToString(Model.EndDate))%>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <span id="toolbar" class="ui-widget-header ui-corner-all">
                    <button id="Save" type="submit">
                        Ok</button>
                </span>
            </td>
        </tr>
    </table>
    <%
       }%>
    <script language="javascript" type="text/javascript">

        $(function () {
            $("#Save").button();
            $("#StartDate").datepicker();
            $("#EndDate").datepicker();
        });

        $(document).ready(function () {


        });

        function onSavedSuccess(e) {
            var json = e.get_response().get_object();
            var msg = json.Message;
            var status = json.Success;
            //alert(status);
            alert(msg);
            if (status) {
                $('#btnSave').attr('disabled', 'disabled');
            }
        }
    </script>
</asp:Content>
