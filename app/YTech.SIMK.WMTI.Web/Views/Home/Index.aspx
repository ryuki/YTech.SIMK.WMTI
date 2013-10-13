<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.Master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" ValidateRequest="false" %>

<%@ Register TagPrefix="uc" TagName="UCPromo" Src="~/Views/Home/UCPromo.ascx" %>
<%@ Register TagPrefix="uc" TagName="UCAnnouncement" Src="~/Views/Home/UCAnnouncement.ascx" %>
<%@ Register TagPrefix="uc" TagName="UCLoanSummary" Src="~/Views/Home/UCLoanSummary.ascx" %>
<%@ Register TagPrefix="uc" TagName="UCAchievement" Src="~/Views/Home/UCAchievement.ascx" %>
<%@ Register TagPrefix="uc" TagName="UCLastLoanSummary" Src="~/Views/Home/UCLastLoanSummary.ascx" %>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% if (false)
       { %>
    <script src="../../Scripts/jquery-1.6.3-vsdoc.js" type="text/javascript"></script>
    <% } %>
    <h2>
        Sistem Informasi Manajemen Kredit Wahana Finance</h2>
    <% if (Request.IsAuthenticated)
       {%>
    <style type="text/css">
        .kiri
        {
            float: left;
            width: 49%;
        }
        .kanan
        {
            float: right;
            width: 49%;
        }
        .boxkiri
        {
            height: 210px;
            border: 1px black solid;
            margin: 5px;
            overflow: auto;
            padding: 5px;
        }
        .boxkanan
        {
            height: 135px;
            border: 1px black solid;
            margin: 5px;
            overflow: auto;
            padding: 5px;
        }
        .title
        {
            font-weight: bold;
            color: Blue;
        }
    </style>
    <div>
        <div class="kiri">
            <div class="boxkiri">
                <uc:UCPromo ID="UCPromo1" runat="server" />
            </div>
            <div class="boxkiri">
                <uc:UCAnnouncement ID="UCAnnouncement1" runat="server" />
            </div>
        </div>
        <div class="kanan">
            <div class="boxkanan">
                <uc:UCLoanSummary ID="UCLoanSummary1" runat="server" />
            </div>
            <div class="boxkanan">
                <uc:UCLastLoanSummary ID="UCLastLoanSummary1" runat="server" />
            </div>
            <div class="boxkanan">
                <uc:UCAchievement ID="UCAchievement1" runat="server" />
            </div>
        </div>
    </div>
    <%
       }
    %>
</asp:Content>
