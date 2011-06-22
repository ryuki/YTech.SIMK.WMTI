<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<RegistrationFormViewModel>" %>
<% if (false)
   { %>
<script src="../../../Scripts/jquery-1.5-vsdoc.js" type="text/javascript"></script>
<% } %>
<%-- <% using (Html.BeginForm())
   {%> --%>
<% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
                                           InsertionMode = InsertionMode.Replace,
                                           OnBegin = "ajaxValidate",
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
                                No :</label>
                        </td>
                        <td>
                            <%--<%= Html.TextBox("Id",  Model.Customer.Id ?? string.Empty,new {@readonly= Model.CanEditId ? "true" : "false" })%>--%>
                            <%= Model.CanEditId ? Html.TextBox("Id", Model.Customer.Id ?? string.Empty, new { @style = "width:150px" }) :
                                    Html.TextBox("Id", Model.Customer.Id ?? string.Empty, new { @readonly = Model.CanEditId ? "true" : "false", @style = "width:150px" })
                            %>
                            <%= Html.ValidationMessage("Id")%>
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
                            <%= Html.TextBox("PersonFirstName", Model.Customer.PersonId.PersonFirstName, new { @style = "width:300px" })%>
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
                            <%= Html.TextBox("PersonIdCardNo", Model.Customer.PersonId.PersonIdCardNo, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonIdCardNo")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonPob">
                                Tempat / Tanggal Lahir :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonPob", Model.Customer.PersonId.PersonPob, new { @style = "width:150px" })%>
                            <%= Html.ValidationMessage("PersonPob")%>
                            &nbsp;/&nbsp;
                            <%= Html.TextBox("PersonDob", Model.Customer.PersonId.PersonDob.HasValue ? Model.Customer.PersonId.PersonDob.Value.ToString(CommonHelper.DateFormat) : null, new { @style = "width:125px" })%>
                            <%= Html.ValidationMessage("PersonDob")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="AddressLine1">
                                Alamat :</label>
                        </td>
                        <td> 
                            <%= Html.TextBox("AddressLine1", Model.Customer.AddressId.AddressLine1, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("AddressLine1")%> 
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td> 
                            <%= Html.TextBox("AddressLine2", Model.Customer.AddressId.AddressLine2, new { @style = "width:175px" })%>
                            <%= Html.ValidationMessage("AddressLine2")%>
                            <label for="AddressIdCardPostCode">
                                Kode Pos :</label>
                            <%= Html.TextBox("AddressPostCode", Model.Customer.AddressId.AddressPostCode, new { style = "width:50px" })%>
                            <%= Html.ValidationMessage("AddressPostCode")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonPhone">
                                No. Telepon :</label>
                        </td>
                        <td> 
                            <%= Html.TextBox("PersonPhone", Model.Customer.PersonId.PersonPhone, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonPhone")%> 
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonMobile">
                                No. Handphone :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonMobile", Model.Customer.PersonId.PersonMobile, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonMobile")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonOccupation">
                                Pekerjaan :</label>
                        </td>
                        <td> 
                            <%= Html.TextBox("PersonOccupation", Model.Customer.PersonId.PersonOccupation, new { @style = "width:300px" })%>
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
                            <%= Html.TextBox("PersonAge", Model.Customer.PersonId.PersonAge, new { @style = "width:300px" })%>
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
                            <%= Html.TextBox("PersonIncome", Model.Customer.PersonId.PersonIncome, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonIncome")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonNoOfChildren">
                                Jumlah Anak :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonNoOfChildren", Model.Customer.PersonId.PersonNoOfChildren, new { @style = "width:300px" })%>
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
                            <%= Html.TextBox("PersonCoupleName", Model.Customer.PersonId.PersonCoupleName, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonCoupleName")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonCoupleOccupation">
                                Pekerjaan :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonCoupleOccupation", Model.Customer.PersonId.PersonCoupleOccupation, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonCoupleOccupation")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonCoupleIncome">
                                Penghasilan / Bulan :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonCoupleIncome", Model.Customer.PersonId.PersonCoupleIncome, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonCoupleIncome")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="AddressStatusOwner">
                                Status Tempat Tinggal :</label>
                        </td>
                        <td>
                            <select>
                              <option value="Milik Sendiri">Milik Sendiri</option>
                              <option value="SewaKontrak">Sewa/Kontrak</option>
                              <option value="Keluarga">Keluarga</option>
                              <option value="KostAsrama">Kost/Asrama</option>
                            </select>
                            <%-- <%= Html.DropDownList("AddressStatusOwner")%>
                            <%= Html.ValidationMessage("AddressStatusOwner")%> --%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonStaySince">
                                Berapa lama tinggal di kota ini :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonStaySince", Model.Customer.PersonId.PersonStaySince.HasValue ? Model.Customer.PersonId.PersonStaySince.Value.ToString(CommonHelper.DateFormat) : null, new { @style = "width:300px" })%>
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
                            <%= Html.TextBox("PersonGuarantorName", Model.Customer.PersonId.PersonGuarantorName, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonGuarantorName")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonGuarantorRelationship">
                                Hubungan dengan Penjamin :</label>
                        </td>
                        <td>
                            <input type="text" name="PersonGuarantorRelationship" style="width:300px" />
                            <%-- <%= Html.TextBox("PersonEmail", Model.Customer.PersonId.PersonEmail, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonEmail")%>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonGuarantorOccupation">
                                Pekerjaan Penjamin :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonGuarantorOccupation", Model.Customer.PersonId.PersonGuarantorOccupation, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonGuarantorOccupation")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonGuarantorPhone">
                                No. HP/Telp Penjamin :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonGuarantorPhone", Model.Customer.PersonId.PersonGuarantorPhone, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonGuarantorPhone")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonStaySince">
                                Mulai tinggal di kota ini :</label>
                        </td>
                        <td>
                            <%= Html.TextBox("PersonGuarantorStaySince", Model.Customer.PersonId.PersonGuarantorStaySince.HasValue ? Model.Customer.PersonId.PersonGuarantorStaySince.Value.ToString(CommonHelper.DateFormat) : null, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonGuarantorStaySince")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="PersonGuarantorHouseOwnerStatus">
                                Status Tempat Tinggal Penjamin :</label>
                        </td>
                        <td>
                            <select>
                              <option value="Milik Sendiri">Milik Sendiri</option>
                              <option value="SewaKontrak">Sewa/Kontrak</option>
                              <option value="Keluarga">Keluarga</option>
                              <option value="KostAsrama">Kost/Asrama</option>
                            </select>
                            <%-- <%= Html.DropDownList("AddressStatusOwner")%>
                            <%= Html.ValidationMessage("AddressStatusOwner")%> --%>
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
                            <select>
                              <option value="Baik">Baik</option>
                              <option value="KurangBaik">Kurang Baik</option>
                            </select>
                            <%-- <%= Html.DropDownList("AddressStatusOwner")%>
                            <%= Html.ValidationMessage("AddressStatusOwner")%> --%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyNeighborCharacter">
                                Character :</label>
                        </td>
                        <td>
                            <input type="text" name="SurveyNeighborCharacter" style="width:300px" />
                            <%-- <%= Html.TextBox("SurveyNeighborCharacter", Model.LoanSurvey.SurveyNeighborCharacter, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("SurveyNeighborCharacter")%> --%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyNeighborConclusion">
                                Keterangan Tetangga :</label>
                        </td>
                        <td>
                            <input type="text" name="SurveyNeighborConclusion" style="width:300px" />
                            <%-- <%= Html.TextBox("SurveyNeighborConclusion", Model.LoanSurvey.SurveyNeighborConclusion, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("SurveyNeighborConclusion")%> --%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyAsset">
                                Accessories :</label>
                        </td>
                        <td>
                            <input type="checkbox" name="acc" value="CTV" />TV
                            <input type="checkbox" name="acc" value="SF" />Sofa
                            <input type="checkbox" name="acc" value="KG" />Kompor Gas
                            <input type="checkbox" name="acc" value="MC" />Mesin Cuci
                            <input type="checkbox" name="acc" value="LH" />Lemari Hias
                            <input type="checkbox" name="acc" value="PC" />Komputer
                            <input type="checkbox" name="acc" value="MTR" />Motor
                            <input type="checkbox" name="acc" value="MBL" />Mobil
                            <input type="checkbox" name="acc" value="AC" />AC
                            <%-- <%= Html.DropDownList("AddressStatusOwner")%>
                            <%= Html.ValidationMessage("AddressStatusOwner")%> --%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyHouseType">
                                Kode Rumah :</label>
                        </td>
                        <td>
                            <select>
                              <option value="Permanen">Permanen</option>
                              <option value="Semi Permanen">Semi Permanen</option>
                              <option value="Kayu">Kayu</option>
                            </select>
                            <%-- <%= Html.DropDownList("AddressStatusOwner")%>
                            <%= Html.ValidationMessage("AddressStatusOwner")%> --%>
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
                            <input type="text" name="SurveyUnitDeliverDate" style="width:300px" />
                            <%-- <%= Html.TextBox("PersonGuarantorStaySince", Model.Customer.PersonId.PersonGuarantorStaySince.HasValue ? Model.Customer.PersonId.PersonGuarantorStaySince.Value.ToString(CommonHelper.DateFormat) : null, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonGuarantorStaySince")%> --%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label for="SurveyUnitDeliverAddress">
                                Alamat Pengiriman Barang :</label>
                        </td>
                        <td>
                            <input type="text" name="SurveyUnitDeliverAddress" style="width:300px" />
                            <%-- <%= Html.TextBox("PersonGuarantorStaySince", Model.Customer.PersonId.PersonGuarantorStaySince.HasValue ? Model.Customer.PersonId.PersonGuarantorStaySince.Value.ToString(CommonHelper.DateFormat) : null, new { @style = "width:300px" })%>
                            <%= Html.ValidationMessage("PersonGuarantorStaySince")%> --%>
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
                $("#Save").attr('disabled', '');
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
                "Id": { required: "<img id='Iderror' src='" + errorimg + "' hovertext='No Pasien harus diisi' />"
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
        $("#Save").button();
        $("#PersonDob").datepicker({ dateFormat: "dd-M-yy" });
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
