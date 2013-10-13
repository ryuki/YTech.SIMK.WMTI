<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPopUp.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery.form.js") %>"></script>
    <form id="form1" enctype="multipart/form-data" action="/WMTI/Home/Upload" method="post">
    <input type="file" id="photo" name="photo" />
    <input type="submit" id="save" name="save" value="Upload" />
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#form1').ajaxForm({
                beforeSubmit: function () {
                    $('#status').html('Submitting...');
                },
                success: function (data) {
                    //                    alert(data);
                    var $out = $('#status');
                    $out.html('Your results:');
                    $out.append('<div><pre>' + data + '</pre></div>');
                    var src = '<%= Request.QueryString["src"] %>';
                    if (src == 'Photo1')
                        window.parent.UploadSuccess(data);
                    else if (src == 'Photo2')
                        window.parent.UploadSuccess2(data);
                    else
                        window.parent.UploadSuccess(data);
                }
            });
        });    
    </script>
</asp:Content>
