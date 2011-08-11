<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>

<div style="width:600px;">
    <div style="float: left;">
    <strong>Keterangan :</strong><br />
        <img src='<%= Url.Content("~/Content/Images/window16.gif") %>' title='Edit PK' style='cursor: hand;
            width: 16px; height: 16px;' alt='Edit PK' />
        = Edit Permohonan Kredit<br />
        <img src='<%= Url.Content("~/Content/Images/edit24_on.gif") %>' title='Edit PK' style='cursor: hand;
            width: 16px; height: 16px;' alt='Edit PK' />
        = Input / Edit Hasil Survey
    </div>
    <div style="float: right;">
        <img src='<%= Url.Content("~/Content/Images/approve24_on.png") %>' title='Edit PK'
            style='cursor: hand; width: 16px; height: 16px;' alt='Edit PK' />
        = Setuju Permohonan Kredit<br />
        <img src='<%= Url.Content("~/Content/Images/ok24_on.png") %>'title='Proses PK'
            style='cursor: hand; width: 16px; height: 16px' alt='Proses PK' />
        = Proses Permohonan Kredit / Kredit OK<br />
        <img src='<%= Url.Content("~/Content/Images/reject32_on.png") %>' title='Edit PK'
            style='cursor: hand; width: 16px; height: 16px;' alt='Edit PK' />
        = Tolak Permohonan Kredit<br />
        <img src='<%= Url.Content("~/Content/Images/cancel32_on.png") %>' title='Edit PK'
            style='cursor: hand; width: 16px; height: 16px;' alt='Edit PK' />
        = Pembatalan Permohonan Kredit<br />
        <img src='<%= Url.Content("~/Content/Images/exit32_on.gif") %>' title='Edit PK' style='cursor: hand;
            width: 16px; height: 16px;' alt='Edit PK' />
        = Tunda Proses Permohonan Kredit
    </div>
</div>
