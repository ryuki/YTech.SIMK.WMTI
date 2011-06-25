<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true" CodeBehind="Survey.aspx.cs" Inherits="YTech.SIMK.WMTI.Web.Views.Transaction.Loan.Survey" %>

<asp:Content ContentPlaceHolderID="head" ID="headContent" runat="server">
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("Detail", ViewData); %>
</asp:Content>
