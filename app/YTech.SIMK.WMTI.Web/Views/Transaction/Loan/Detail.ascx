<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SurveyFormViewModel>" %>
<% if (false)
   { %>
<script src="../../../Scripts/jquery-1.5.2-vsdoc.js" type="text/javascript"></script>
<% } %>
<% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
                                           InsertionMode = InsertionMode.Replace,
                                           OnSuccess = "onSavedSuccess"
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
                            <label for="Id">
                                No PK:</label>
                        </td>
                        <td>
                            <%--<%= Html.TextBox("Id",  Model.Customer.Id ?? string.Empty,new {@readonly= Model.CanEditId ? "true" : "false" })%>--%>
                            <%= Model.CanEditId ? Html.TextBox("LoanNo", Model.LoanSurvey.LoanId.LoanNo ?? string.Empty, new { @style = "width:150px" }) :
                                                         Html.TextBox("LoanNo", Model.LoanSurvey.LoanId.LoanNo ?? string.Empty, new { @readonly = Model.CanEditId ? "true" : "false", @style = "width:150px" })
                            %>
                            <%= Html.ValidationMessage("Id")%>
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
                            <label for="ZoneName">
                                Wilayah :</label>
                        </td>
                        <td> 
                            <%= Html.DropDownList("ZoneId", Model.ZoneList)%>
                           <%-- <%= Html.TextBox("ZoneId", Model.LoanSurvey.LoanId.ZoneId != null ? Model.LoanSurvey.LoanId.ZoneId.Id : null, new { @style = "width:300px" })%>--%>
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
                            <%= Html.TextBox("PersonAge", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonAge, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonAge")%>
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
                            <%= Html.TextBox("PersonIncome", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonIncome.HasValue ? Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonIncome.Value.ToString(CommonHelper.NumberFormat) : null, new { @style = "width:300px" })%>
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
                         <%= Html.TextBox("PersonCoupleIncome", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonCoupleIncome.HasValue ? Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonCoupleIncome.Value.ToString(CommonHelper.NumberFormat) : null, new { @style = "width:300px" })%>
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
                            <%= Html.TextBox("PersonStaySince", Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonStaySince.HasValue ? Model.LoanSurvey.LoanId.CustomerId.PersonId.PersonStaySince.Value.ToString(CommonHelper.DateFormat) : null, new { @style = "width:300px" })%>
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
                            <label for="PersonStaySince">
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
                            <input type="checkbox" name="SurveyNeighborAsset" value="CTV" />TV
                            <input type="checkbox" name="SurveyNeighborAsset" value="SF" />Sofa
                            <input type="checkbox" name="SurveyNeighborAsset" value="KG" />Kompor Gas
                            <input type="checkbox" name="SurveyNeighborAsset" value="MC" />Mesin Cuci
                            <input type="checkbox" name="SurveyNeighborAsset" value="LH" />Lemari Hias
                            <input type="checkbox" name="SurveyNeighborAsset" value="PC" />Komputer
                            <input type="checkbox" name="SurveyNeighborAsset" value="MTR" />Motor
                            <input type="checkbox" name="SurveyNeighborAsset" value="MBL" />Mobil
                            <input type="checkbox" name="SurveyNeighborAsset" value="AC" />AC
                            <%-- <%= Html.DropDownList("AddressStatusOwner")%> --%>
                            <%= Html.ValidationMessage("SurveyNeighborAsset")%>
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
                            <%= Html.TextBox("SurveyUnitDeliverDate", Model.LoanSurvey.SurveyUnitDeliverDate.HasValue ? Model.LoanSurvey.SurveyUnitDeliverDate.Value.ToString(CommonHelper.DateFormat) : null, new { @style = "width:300px" })%>
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
                            <label for="LoanSalesman">
                                Nama SA yg mengajukan :</label>
                        </td>
                        <td>
                            <%= Html.DropDownList("SalesmanId", Model.SalesmanList)%>
                            <%--<input type="text" name="LoanSalesman" style="width:300px;" />--%>
                             <%--<%= Html.TextBox("LoanSalesman", Model.LoanSurvey.LoanId.SalesmanId.PersonId.PersonFirstName, new { @style = "width:300px" })%>--%> 
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
                           <%-- <%= Html.TextBox("TLSId", Model.LoanSurvey.LoanId.TLSId, new { @style = "width:300px" })%>--%>
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
                           <%-- <%= Html.TextBox("CollectorId", Model.LoanSurvey.LoanId.CollectorId, new { @style = "width:300px" })%>--%>
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
                           <%-- <%= Html.TextBox("SurveyorId", Model.LoanSurvey.LoanId.SurveyorId, new { @style = "width:300px" })%>--%>
                            <%= Html.ValidationMessage("SurveyorId")%>
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
                            <%= Html.TextBox("UnitPrice", Model.LoanUnit.UnitPrice.HasValue ? Model.LoanUnit.UnitPrice.Value.ToString(CommonHelper.NumberFormat) : null, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("UnitType")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="LoanTenor">
                                Lama Angsuran :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("LoanTenor", Model.LoanSurvey.LoanId.LoanTenor, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("LoanTenor")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="LoanBasicInstallment">
                                Jumlah Angsuran per bulan RP:</label>
                        </td>
                        <td>
                         <%= Html.TextBox("LoanBasicInstallment", Model.LoanSurvey.LoanId.LoanBasicInstallment.HasValue ? Model.LoanSurvey.LoanId.LoanBasicInstallment.Value.ToString(CommonHelper.NumberFormat) : null, new { @style = "width:300px" })%>
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
                         <%= Html.TextBox("LoanDownPayment", Model.LoanSurvey.LoanId.LoanDownPayment.HasValue ? Model.LoanSurvey.LoanId.LoanDownPayment.Value.ToString(CommonHelper.NumberFormat) : null, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("LoanDownPayment")%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr /></td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyDate">
                                Tanggal :</label>
                        </td>
                        <td>
                         <%= Html.TextBox("SurveyDate", Model.LoanSurvey.SurveyDate.HasValue ? Model.LoanSurvey.SurveyDate.Value.ToString(CommonHelper.DateFormat) : null, new { @style = "width:300px" })%>
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
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <div>
                    <span id="toolbar" class="ui-widget-header ui-corner-all">
                        <%--<a id="newCustomer" href="Registration">
                            Registrasi Pasien Baru</a>--%>
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
                "Id": { required: true
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
                "PersonFirstName": { required: true }
            },
            messages: {
                "Id": { required: "<img id='Iderror' src='" + errorimg + "' hovertext='No PK harus diisi' />"
                    //                     <% if (string.IsNullOrEmpty(Request.QueryString["customerId"])) {	%>
                    //                    , remote: "<img id='remoteIderror' src='" + errorimg + "' hovertext='No Pasien sudah pernah diinput.' />" <% } %>
                },
                "PersonFirstName": "<img id='PersonFirstNameerror' src='" + errorimg + "' hovertext='Nama harus diisi' />"
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
        $("#PersonDob").datepicker({ dateFormat: "dd-M-yy" });
        $("#PersonStaySince").datepicker({ dateFormat: "dd-M-yy" });
        $("#PersonGuarantorStaySince").datepicker({ dateFormat: "dd-M-yy" });
        $("#SurveyUnitDeliverDate").datepicker({ dateFormat: "dd-M-yy" });
        $("#SurveyDate").datepicker({ dateFormat: "dd-M-yy" });

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
</script>
