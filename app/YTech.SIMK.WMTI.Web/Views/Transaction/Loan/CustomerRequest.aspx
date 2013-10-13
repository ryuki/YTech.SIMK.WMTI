<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Views/Shared/MyMaster.master" 
    CodeBehind="CustomerRequest.aspx.cs" Inherits="YTech.SIMK.WMTI.Web.Views.Transaction.Loan.CustomerRequest"  ValidateRequest="false" %>

<asp:Content ContentPlaceHolderID="head" ID="headContent" runat="server">
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("CustomerRequestDetail", ViewData); %>
</asp:Content>
