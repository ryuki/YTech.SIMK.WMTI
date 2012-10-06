<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<HomeViewModel>" %>
<% if (false)
   { %>
<script src="../../../Scripts/jquery-1.6.3-vsdoc.js" type="text/javascript"></script>
<script src="../../../Scripts/jquery.validate-vsdoc.js" type="text/javascript"></script>
<% } %>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        // Add markItUp! to your textarea in one line
        // $('textarea').markItUp( { Settings }, { OptionalExtraSettings } );
        $('#promoNews').markItUp(mySettings);


        $("#popup").dialog({
            autoOpen: false,
            height: 420,
            width: '80%',
            modal: true,
            close: function (event, ui) {

            }
        });

        $('#btnEditPromo').click(function () {
            $("#popup").dialog("open");
        });

        $('#btnPromoNewsSave').click(function () {
            var posturl = '<%= Url.Action("SavePromo","Home") %>';
            var dataString = 'promoNews=' + $("#promoNews").text();
            //alert (dataString);return false;  
            $.ajax({
                type: "POST",
                url: posturl,
                data: dataString,
                success: function () {
                    alert("Data promo berhasil disimpan");
                }
            });
        });

    });

</script>
<div id='popup' style="text-align: center;">
    <textarea rows="10" style="width: 95%; height: 250px;" id="promoNews"><%= Model.PromoNews.NewsDesc%></textarea><br />
    <br />
    <button id="btnPromoNewsSave">
        Simpan</button>
</div>
<p class="title">
    Gebyar Promosi dan Bonus
    <% if (Membership.GetUser().UserName.ToLower().Equals("admin"))
       { %>
    <button value="Edit" id="btnEditPromo">
        Edit</button>
    <%
       }
    %>
</p>
<%= Model.PromoNews.NewsDesc%>