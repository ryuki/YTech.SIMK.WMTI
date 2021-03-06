﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SurveyFormViewModel>" %>
<% if (false)
   { %>
<script src="../../../Scripts/jquery-1.6.3-vsdoc.js" type="text/javascript"></script>
<% } %>
<% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
                                           InsertionMode = InsertionMode.Replace,
                                           OnBegin = "ajaxValidate",
                                           OnSuccess = "onSavedSuccess",
                                           LoadingElementId = "progress"
                                       }

          ))
   {%>
<div id="formArea">
    <%=Html.AntiForgeryToken()%>
    <table>
        <tr>
            <td colspan="2">
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <label for="LoanNo">
                                No PK:</label>
                        </td>
                        <td>
                            <%--<%= Html.TextBox("Id",  Model.Customer.Id ?? string.Empty,new {@readonly= Model.CanEditId ? "true" : "false" })%>--%>
                            <%= Model.CanEditId ? Html.TextBox("LoanNo", Model.LoanSurvey.LoanId.LoanNo ?? string.Empty, new { @style = "width:150px" }) :
                                                         Html.TextBox("LoanNo", Model.LoanSurvey.LoanId.LoanNo ?? string.Empty, new { @readonly = Model.CanEditId ? "true" : "false", @style = "width:150px" })
                            %>
                            <%= Html.ValidationMessage("LoanNo")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="LoanCode">
                                No Account:</label>
                        </td>
                        <td>
                            <%= Html.TextBox("LoanCode", Model.LoanSurvey.LoanId.LoanCode, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("LoanCode")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="LoanSubmissionDate">
                                Tanggal PK :</label>
                        </td>
                        <td>
                         <%= Html.TextBox("LoanSubmissionDate", CommonHelper.ConvertToString(Model.LoanSurvey.LoanId.LoanSubmissionDate), new { @style = "width:100px" })%>
                            <%= Html.ValidationMessage("LoanSubmissionDate")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonFirstName">
                                Nama Pemohon
                                <br />
                                (Sesuai KTP) :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonFirstName", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonFirstName, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonFirstName")%>
                        </td>
                    </tr>  
                    <tr>
                        <td>
                            <label for="PersonGender">
                                Jenis Kelamin Pemohon :</label>
                        </td>
                        <td>
                            <%=Html.DropDownList("PersonGender",Model.GenderList )%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonIdCardNo">
                                No. KTP :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonIdCardNo", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonIdCardNo, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonIdCardNo")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonPob">
                                Tempat / Tanggal Lahir :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonPob", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonPob, new { @style = "width:150px" })%>
                            <%= Html.ValidationMessage("PersonPob")%>
                            &nbsp;/&nbsp;
                            <%= Html.TextBox("PersonDob", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonDob.HasValue ? Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonDob.Value.ToString(CommonHelper.DateFormat) : null, new { @style = "width:125px" })%>
                            <%= Html.ValidationMessage("PersonDob")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="AddressLine1">
                                Alamat :</label>
                        </td>
                        <td> 
                            <%= Html.TextBox("AddressLine1", Model.LoanSurvey.LoanId.CustomerId.AddressId.AddressLine1, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("AddressLine1")%> 
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td> 
                            <%= Html.TextBox("AddressLine2", Model.LoanSurvey.LoanId.CustomerId.AddressId.AddressLine2, new { @style = "width:175px" })%>
                            <%= Html.ValidationMessage("AddressLine2")%>
                            <label for="AddressIdCardPostCode">
                                Kode Pos :</label>
                            <%= Html.TextBox("AddressPostCode", Model.LoanSurvey.LoanId.CustomerId.AddressId.AddressPostCode, new { style = "width:50px" })%>
                            <%= Html.ValidationMessage("AddressPostCode")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="ZoneId">
                                Wilayah :</label>
                        </td>
                        <td> 
                            <%= Html.DropDownList("ZoneId", Model.ZoneList)%>
                            <%= Html.ValidationMessage("ZoneId")%> 
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonPhone">
                                No. Telepon :</label>
                        </td>
                        <td> 
                            <%= Html.TextBox("PersonPhone", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonPhone, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonPhone")%> 
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonMobile">
                                No. Handphone :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonMobile", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonMobile, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonMobile")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonOccupation">
                                Pekerjaan :</label>
                        </td>
                        <td> 
                            <%= Html.TextBox("PersonOccupation", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonOccupation, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonOccupation")%> 
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonLastEducation">
                                Pendidikan Terakhir :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("PersonLastEducation", Model.EducationList )%>
                            <%= Html.ValidationMessage("PersonLastEducation")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonAge">
                                Umur :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonAge", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonAge, new { @style = "width:25px" })%>
                            <%= Html.ValidationMessage("PersonAge")%>
                            Tahun
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonReligion">
                                Agama :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("PersonReligion", Model.ReligionList )%>
                            <%= Html.ValidationMessage("PersonReligion")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonIncome">
                                Penghasilan / Bulan :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonIncome", CommonHelper.ConvertToString(Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonIncome), new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonIncome")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonNoOfChildren">
                                Jumlah Anak :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonNoOfChildren", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonNoOfChildren, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonNoOfChildren")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonMarriedStatus">
                                Status Perkawinan :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("PersonMarriedStatus", Model.MarriedStatusList )%>
                            <%= Html.ValidationMessage("PersonMarriedStatus")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonCoupleName">
                                Nama Istri/Suami :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonCoupleName", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonCoupleName, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonCoupleName")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonCoupleOccupation">
                                Pekerjaan :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonCoupleOccupation", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonCoupleOccupation, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonCoupleOccupation")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonCoupleIncome">
                                Penghasilan / Bulan :</label>
                        </td>
                        <td>
                         <%= Html.TextBox("PersonCoupleIncome", CommonHelper.ConvertToString(Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonCoupleIncome), new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonCoupleIncome")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="AddressStatusOwner">
                                Status Tempat Tinggal :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("AddressStatusOwner", Model.HouseOwnerList)%>
                            <%= Html.ValidationMessage("AddressStatusOwner")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonStaySince">
                                Berapa lama tinggal di kota ini :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonStaySince", CommonHelper.ConvertToString(Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonStaySince), new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonStaySince")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonGuarantorName">
                                Nama Penjamin 
                                <br />
                                (Bila belum menikah) :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonGuarantorName", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonGuarantorName, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonGuarantorName")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonGuarantorRelationship">
                                Hubungan dengan Penjamin :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonGuarantorRelationship", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonGuarantorRelationship, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonGuarantorRelationship")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonGuarantorOccupation">
                                Pekerjaan Penjamin :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonGuarantorOccupation", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonGuarantorOccupation, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonGuarantorOccupation")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonGuarantorPhone">
                                No. HP/Telp Penjamin :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonGuarantorPhone", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonGuarantorPhone, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonGuarantorPhone")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonGuarantorStaySince">
                                Mulai tinggal di kota ini :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonGuarantorStaySince", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonGuarantorStaySince.HasValue ? Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonGuarantorStaySince.Value.ToString(CommonHelper.DateFormat) : null, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonGuarantorStaySince")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonGuarantorHouseOwnerStatus">
                                Status Tempat Tinggal Penjamin :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("PersonGuarantorHouseOwnerStatus", Model.GuarantorHouseOwnerList)%>
                            <%= Html.ValidationMessage("PersonGuarantorHouseOwnerStatus")%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr /></td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyNeighbor">
                                Tetangga :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("SurveyNeighbor", Model.NeighborCharList)%>
                            <%= Html.ValidationMessage("SurveyNeighbor")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyNeighborCharacter">
                                Character :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("SurveyNeighborCharacter", Model.LoanSurvey.SurveyNeighborCharacter, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("SurveyNeighborCharacter")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyNeighborConclusion">
                                Keterangan Tetangga :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("SurveyNeighborConclusion", Model.LoanSurvey.SurveyNeighborConclusion, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("SurveyNeighborConclusion")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyNeighborAsset">
                                Accessories :</label>
                        </td>
                        <td>
                            <%= Html.CheckBox("SurveyNeighborAsset_CTV", Model.SurveyNeighborAsset_CTV)%>&nbsp;CTV
                            <%= Html.CheckBox("SurveyNeighborAsset_SF", Model.SurveyNeighborAsset_SF)%>&nbsp;SF
                            <%= Html.CheckBox("SurveyNeighborAsset_KG", Model.SurveyNeighborAsset_KG)%>&nbsp;KG
                            <%= Html.CheckBox("SurveyNeighborAsset_MC", Model.SurveyNeighborAsset_MC)%>&nbsp;MC
                            <%= Html.CheckBox("SurveyNeighborAsset_LH", Model.SurveyNeighborAsset_LH)%>&nbsp;LH
                            <%= Html.CheckBox("SurveyNeighborAsset_PC", Model.SurveyNeighborAsset_PC)%>&nbsp;PC
                            <%= Html.CheckBox("SurveyNeighborAsset_MTR", Model.SurveyNeighborAsset_MTR)%>&nbsp;MTR
                            <%= Html.CheckBox("SurveyNeighborAsset_MBL", Model.SurveyNeighborAsset_MBL)%>&nbsp;MBL
                            <%= Html.CheckBox("SurveyNeighborAsset_AC", Model.SurveyNeighborAsset_AC)%>&nbsp;AC
                            <%= Html.ValidationMessage("SurveyNeighborAsset_CTV")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyHouseType">
                                Kode Rumah :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("SurveyHouseType", Model.HouseTypeList)%>
                            <%= Html.ValidationMessage("SurveyHouseType")%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr /></td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyUnitDeliverDate">
                                Rencana Pengiriman Barang jika Disetujui Tanggal :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("SurveyUnitDeliverDate", CommonHelper.ConvertToString(Model.LoanSurvey.SurveyUnitDeliverDate), new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("SurveyUnitDeliverDate")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyUnitDeliverAddress">
                                Alamat Pengiriman Barang :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("SurveyUnitDeliverAddress", Model.LoanSurvey.SurveyUnitDeliverAddress, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("SurveyUnitDeliverAddress")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="LoanMaturityDate">
                                Jatuh Tempo Pembayaran Perbulan Setiap Tanggal :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("LoanMaturityDate", Model.LoanSurvey.LoanId.LoanMaturityDate, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("LoanMaturityDate")%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr /></td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SalesmanId">
                                Nama SA yg mengajukan :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("SalesmanId", Model.SalesmanList)%>
                            <%= Html.ValidationMessage("SalesmanId")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="TLSId">
                                Kode TL :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("TLSId", Model.TLSList)%>
                            <%= Html.ValidationMessage("TLSId")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="CollectorId">
                                Kode Col :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("CollectorId", Model.CollectorList)%>
                            <%= Html.ValidationMessage("CollectorId")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyorId">
                                Kode Surv :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("SurveyorId", Model.SurveyorList)%>
                            <%= Html.ValidationMessage("SurveyorId")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PartnerId">
                                Nama Toko :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("PartnerId", Model.PartnerList)%>
                            <%= Html.ValidationMessage("PartnerId")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="UnitName">
                                Nama Barang yg akan diambil :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("UnitName", Model.LoanUnit != null ? Model.LoanUnit.UnitName : null, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("UnitName")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="UnitType">
                                Type Barang :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("UnitType", Model.LoanUnit != null ? Model.LoanUnit.UnitType : null, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("UnitType")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="UnitPrice">
                                Harga Barang :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("UnitPrice", CommonHelper.ConvertToString(Model.LoanUnit.UnitPrice), new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("UnitPrice")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="LoanTenor">
                                Lama Angsuran :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("LoanTenor", Model.LoanSurvey.LoanId.LoanTenor, new { @style = "width:25px" })%>
                            <%= Html.ValidationMessage("LoanTenor")%>
                            Bulan
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="LoanBasicInstallment">
                                Jumlah Angsuran per bulan RP:</label>
                        </td>
                        <td>
                         <%= Html.TextBox("LoanBasicInstallment", CommonHelper.ConvertToString(Model.LoanSurvey.LoanId.LoanBasicInstallment), new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("LoanBasicInstallment")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="LoanIsSalesmanKnowCust">
                                SA/TL mengetahui tentang konsumen :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("LoanIsSalesmanKnowCust", Model.KnowCustomerList)%>
                            <%= Html.ValidationMessage("LoanIsSalesmanKnowCust")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="LoanDownPayment">
                                Bila memakai DP, maka DP adalah :</label>
                        </td>
                        <td>
                         <%= Html.TextBox("LoanDownPayment", CommonHelper.ConvertToString(Model.LoanSurvey.LoanId.LoanDownPayment), new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("LoanDownPayment")%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr /></td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyDate">
                                Tanggal Survey :</label>
                        </td>
                        <td>
                         <%= Html.TextBox("SurveyDate", CommonHelper.ConvertToString(Model.LoanSurvey.SurveyDate), new { @style = "width:100px" })%>
                            <%= Html.ValidationMessage("SurveyDate")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyReceivedBy">
                                Laporan diterima oleh :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("SurveyReceivedBy", Model.LoanSurvey.SurveyReceivedBy, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("SurveyReceivedBy")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyProcessBy">
                                Diproses oleh Administrasi :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("SurveyProcessBy", Model.LoanSurvey.SurveyProcessBy, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("SurveyProcessBy")%>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
             Foto :<br />
                <%--<%= Html.TextArea("LoanDesc", Model.Loan.LoanDesc, new { @style = "width: 200px; height: 250px;",@class="tinymce" })%>
                 <textarea rows="10" style="width: 200px; height: 250px;" id="LoanDesc_"
                    name="LoanDesc_" class="tinymces">
<%= Model.Loan.LoanDesc %></textarea>--%>
                <fieldset>
                <legend>Foto 1</legend>
                    <img id="img1" alt="" src='<%= Url.Content(Model.Photo1) %>'  width="300px" height="200px"/>
                        <br />
                    <iframe id='iUpload1' src='<%= Url.Content("~/Home/Upload?src=Photo1") %>' frameborder="0" width="300px"
                        height="50px"></iframe>
                <input type="hidden" id="hidPhoto1" name="hidPhoto1" />
                </fieldset>
                <fieldset>
                <legend>Foto 2</legend>
                    <img id="img2" alt="" src='<%= Url.Content(Model.Photo2) %>' width="300px" height="200px" />
                        <br />
                    <iframe id='iUpload2' src='<%= Url.Content("~/Home/Upload?src=Photo2") %>' frameborder="0" width="300px"
                        height="50px"></iframe>
                <input type="hidden" id="hidPhoto2" name="hidPhoto2" />
                </fieldset>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <div>
                    <span id="toolbar" class="ui-widget-header ui-corner-all">
                        <button id="Save" type="submit">
                            Simpan</button>
                    </span>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <div id="status">
                </div>
                <div class="ui-state-highlight ui-corner-all" style="padding: 5pt; margin-bottom: 5pt;
                    display: none;" id="error">
                    <p>
                        <span class="ui-icon ui-icon-error" style="float: left; margin-right: 0.3em;"></span>
                        <span id="error_msg"></span>.<br clear="all" />
                    </p>
                </div>
            </td>
        </tr>
    </table>
</div>
<%
    }
%>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
<script language="javascript" type="text/javascript">
    function onSavedSuccess(e) {
        //        alert(e.get_response().get_object());
        if (e) {
            var json = e.get_response().get_object();
            var success = json.Success;
            var msg = json.Message;
            //            alert(success);
            //            alert(msg);
            if (success) {
                $("#Save").attr('disabled', 'disabled');
                $('#status').html(msg);
            }
            else {
                $("#Save").removeAttr('disabled');
                if (msg) {
                    $('#dialog p:first').text(msg);
                    $("#dialog").dialog("open");
                    //                    return false;
                }
            }
        }
    }

    function ajaxValidate() {
        var errorimg = '<%= Url.Content("~/Content/Images/cross.gif") %>';
        //var checkIdUrl = '<%= Url.Action("CheckCustomer","Customer") %>';
        return $('form').validate({
            rules: {
                "LoanNo": { required: true
                    //                    <% if (string.IsNullOrEmpty(Request.QueryString["customerId"])) {	%>
                    // ,remote: {
                    //                            url: checkIdUrl,
                    //                            type: "post",
                    //                            data: {
                    //                                customerId: function () {
                    //                                    return $("#Id").val();
                    //                                }
                    //                            }
                    //                        }
                    //  <% } %>                       
                },
                "LoanCode": { required: true },
                "LoanSubmissionDate": { required: true },
                "PersonFirstName": { required: true },
                "AddressLine1": { required: true },
                "ZoneId": { required: true },
                "SurveyUnitDeliverDate": { required: true },
                "LoanMaturityDate": { required: true },
                "SalesmanId": { required: true },
                "TLSId": { required: true },
                "CollectorId": { required: true },
                "SurveyorId": { required: true },
                "UnitName": { required: true },
                "UnitPrice": { required: true },
                "LoanTenor": { required: true },
                "LoanBasicInstallment": { required: true },
                "SurveyDate": { required: true },
                "PartnerId": { required: true }
            },
            messages: {
                "LoanNo": { required: "<img id='Iderror' src='" + errorimg + "' hovertext='No PK harus diisi' />"
                    //                     <% if (string.IsNullOrEmpty(Request.QueryString["customerId"])) {	%>
                    //                    , remote: "<img id='remoteIderror' src='" + errorimg + "' hovertext='No Pasien sudah pernah diinput.' />" <% } %>
                },
                "LoanCode": "<img id='LoanCodeerror' src='" + errorimg + "' hovertext='No Account harus diisi' />",
                "LoanSubmissionDate": "<img id='LoanSubmissionDateerror' src='" + errorimg + "' hovertext='Tanggal PK harus diisi' />",
                "PersonFirstName": "<img id='PersonFirstNameerror' src='" + errorimg + "' hovertext='Nama Pemohon harus diisi' />",
                "AddressLine1": "<img id='AddressLine1error' src='" + errorimg + "' hovertext='Alamat harus diisi' />",
                "ZoneId": "<img id='ZoneIderror' src='" + errorimg + "' hovertext='Wilayah harus diisi' />",
                "SurveyUnitDeliverDate": "<img id='SurveyUnitDeliverDateerror' src='" + errorimg + "' hovertext='Tanggal Rencana Pengiriman Barang harus diisi' />",
                "LoanMaturityDate": "<img id='LoanMaturityDateerror' src='" + errorimg + "' hovertext='Jatuh Tempo Pembayaran harus diisi' />",
                "SalesmanId": "<img id='SalesmanIderror' src='" + errorimg + "' hovertext='Nama SA harus diisi' />",
                "TLSId": "<img id='TLSIderror' src='" + errorimg + "' hovertext='TL harus diisi' />",
                "CollectorId": "<img id='CollectorIderror' src='" + errorimg + "' hovertext='Col harus diisi' />",
                "SurveyorId": "<img id='SurveyorIderror' src='" + errorimg + "' hovertext='Surv harus diisi' />",
                "UnitName": "<img id='UnitNameerror' src='" + errorimg + "' hovertext='Nama Barang harus diisi' />",
                "UnitPrice": "<img id='UnitPriceerror' src='" + errorimg + "' hovertext='Harga Barang harus diisi' />",
                "LoanTenor": "<img id='LoanTenorerror' src='" + errorimg + "' hovertext='Lama Angsuran harus diisi' />",
                "LoanBasicInstallment": "<img id='LoanBasicInstallmenterror' src='" + errorimg + "' hovertext='Angsuran per bulan harus diisi' />",
                "SurveyDate": "<img id='SurveyDateerror' src='" + errorimg + "' hovertext='Tanggal Survey harus diisi' />",
                "PartnerId": "<img id='PartnerIderror' src='" + errorimg + "' hovertext='Pilih toko' />"
            },
            invalidHandler: function (form, validator) {
                var errors = validator.numberOfInvalids();
                if (errors) {
                    var message = "Validasi data kurang";
                    $("div#error span#error_msg").html(message);
                    $("div#error").dialog("open");
                    return false;
                } else {
                    $("div#error").dialog("close");
                }
            },
            errorPlacement: function (error, element) {
                error.insertAfter(element);
                //	generateTooltips();
            }
        }).form();
    }


    $(function () {
        $("#newCustomer").button();
        //$("#Save").button();
        $("#PersonDob").datepicker();
        $("#PersonStaySince").datepicker();
        $("#PersonGuarantorStaySince").datepicker();
        $("#SurveyUnitDeliverDate").datepicker();
        $("#SurveyDate").datepicker();
        $("#LoanSubmissionDate").datepicker();

        $('#PersonIncome').autoNumeric();
        $('#PersonIncome').attr("style", "text-align:right;");
        $('#PersonCoupleIncome').autoNumeric();
        $('#PersonCoupleIncome').attr("style", "text-align:right;");
        $('#LoanBasicInstallment').autoNumeric();
        $('#LoanBasicInstallment').attr("style", "text-align:right;");
        $('#LoanDownPayment').autoNumeric();
        $('#LoanDownPayment').attr("style", "text-align:right;");
        $('#UnitPrice').autoNumeric();
        $('#UnitPrice').attr("style", "text-align:right;");
    });

    $(document).ready(function () {
        $("form").mouseover(function () {
            generateTooltips();
        });

        $("#dialog").dialog({
            autoOpen: false
        });

        $("div#error").dialog({
            autoOpen: false
        });
    });

    //function to generate tooltips
    function generateTooltips() {
        //make sure tool tip is enabled for any new error label
        //          alert('s');
        $("img[id*='error']").tooltip({
            showURL: false,
            opacity: 0.99,
            fade: 150,
            positionRight: true,
            bodyHandler: function () {
                return $("#" + this.id).attr("hovertext");
            }
        });
        //make sure tool tip is enabled for any new valid label
        $("img[src*='tick.gif']").tooltip({
            showURL: false,
            bodyHandler: function () {
                return "OK";
            }
        });
    }

    function UploadSuccess(data) {
        $('#img1').attr('src', data);
        $('#hidPhoto1').val(data);
    }

    function UploadSuccess2(data) {
        $('#img2').attr('src', data);
        $('#hidPhoto2').val(data);
    }

</script>
