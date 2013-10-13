<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<HomeViewModel>" %>
<% if (false)
   { %>
<script src="../../../Scripts/jquery-1.6.3-vsdoc.js" type="text/javascript"></script>
<script src="../../../Scripts/jquery.validate-vsdoc.js" type="text/javascript"></script>
<% } %>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $("#popupAnnouncement").dialog({
            autoOpen: false,
            height: 420,
            width: '80%',
            modal: true,
            close: function (event, ui) {

            }
        });

        $('#btnEditAnnouncement').click(function () {
            $("#popupAnnouncement").dialog("open");
        });

        $('#btnAnnouncementNewsSave').click(function () {
            var posturl = '<%= Url.Action("SaveAnnouncement","Home") %>';
            var dataString = 'announcementNews=' + $("#announcementNews").html();
            //alert (dataString);return false;  
            $.ajax({
                type: "POST",
                url: posturl,
                data: dataString,
                success: function () {
                    alert("Data pengumuman berhasil disimpan");
                }
            });
            return false;
        });

    });

</script>
<div id='popupAnnouncement' style="text-align: center;">
    <textarea rows="10" style="width: 95%; height: 250px;" id="announcementNews" class="tinymce">
<%= Model.AnnouncementNews.NewsDesc%></textarea><br />
    <br />
    <button id="btnAnnouncementNewsSave">
        Simpan</button>
</div>
<p class="title">
    Pengumuman dan Target <%= DateTime.Today.ToString(CommonHelper.MonthFormat) %>
    <% if (Membership.GetUser().UserName.ToLower().Equals("admin"))
       { %>
    <button value="Edit" id="btnEditAnnouncement">
        Edit</button>
    <%
       }
    %>
</p>
<%= Model.AnnouncementNews.NewsDesc%>