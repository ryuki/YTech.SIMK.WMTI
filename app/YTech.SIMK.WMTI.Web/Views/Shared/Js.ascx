﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<link href="<%= Url.Content("~/Content/css/samplebrowser.css")%>" rel="stylesheet"
        type="text/css" />
    <%--This code will hide from other browser than 7--%>
    <!--[if IE 7]>
        <link rel="stylesheet" type="text/css" href="../../Content/css/Stylesheet1.css" />
        <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/css/master.css")%>" />
        <![endif]-->

<%--<link href="<%= Url.Content("~/Content/Site.css")%>" rel="stylesheet" type="text/css" />--%>
<link href="<%= Url.Content("~/Content/css/buttons.css")%>" rel="stylesheet" type="text/css" />
    <![if !(IE 7)]>
    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/css/master.css")%>" />
    <![endif]>
    <link href="<%= Url.Content("~/Content/css/start/jquery-ui.css") %>" rel="Stylesheet"
        type="text/css" media="screen" />
    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/css/layout-default-latest.css")%>" />
    <link rel="Stylesheet" type="text/css" href="<%= Url.Content("~/Content/css/complex.css")%>" />
    <link href="<%= Url.Content("~/Content/ui.jqgrid.css") %>" rel="stylesheet" type="text/css" media="screen" />
   
    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.debug.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.debug.js") %>" type="text/javascript"></script>
   <%-- <script src="<% = Url.Content("~/Scripts/Templates/MicrosoftAjaxAdoNet.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/Templates/MicrosoftAjaxTemplates.js") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcValidation.debug.js") %>" type="text/javascript"></script>--%>

    <script src="<%= Url.Content("~/Scripts/jquery-1.6.4.min.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-ui.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.validate-1.8.1.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/grid.locale-en.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.jqGrid-4.1.2.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.maskedinput-1.2.2.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.mousewheel-3.0.4.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.layout.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.layout.state.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/autoNumeric-1.6.2.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.tooltip.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/date.format.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/NumberFormat.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/complex.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/pixastic.core.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/pixastic.jquery.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/desaturate.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/grayscale.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.tree.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/plugins/jquery.tree.checkbox.js") %>" type="text/javascript"></script>
    
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/tiny_mce/jquery.tinymce.js") %>"></script>

    <script src="<%= Url.Content("~/Scripts/default.js?v1.2") %>" type="text/javascript"></script>