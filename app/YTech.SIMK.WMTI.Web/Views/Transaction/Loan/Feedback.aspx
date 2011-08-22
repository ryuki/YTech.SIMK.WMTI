<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Feedback.aspx.cs" MasterPageFile="~/Views/Shared/MyMaster.master"
    Inherits="YTech.SIMK.WMTI.Web.Views.Transaction.Loan.Feedback" %>

<asp:Content ContentPlaceHolderID="head" ID="headContent" runat="server">
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("FeedbackDetail", ViewData); %>
</asp:Content>
