﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<YTech.SIMK.WMTI.Web.Controllers.ViewModel.ReportParamViewModel>" EnableViewState="true" ValidateRequest="false"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
  <% Html.RenderPartial("ReportParamForm", ViewData); %>
</asp:Content>
