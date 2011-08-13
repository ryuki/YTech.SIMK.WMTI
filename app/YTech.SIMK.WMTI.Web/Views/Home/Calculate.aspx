<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Register TagPrefix="uc" TagName="Calculator" Src="~/Views/Home/ucCalculator.ascx" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <uc:Calculator ID="ucCalculator" runat="server" />
</asp:Content>