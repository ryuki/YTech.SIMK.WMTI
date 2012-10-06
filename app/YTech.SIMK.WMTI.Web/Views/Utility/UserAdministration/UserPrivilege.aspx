<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPopup.master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<MMenu>>" %>

<%@ Import Namespace="YTech.SIMK.WMTI.Core.Master" %>
<asp:Content ContentPlaceHolderID="title" runat="server">
    Administrasi Pengguna
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">

<% if (false)
   { %>
<script src="../../../Scripts/jquery-1.6.3-vsdoc.js" type="text/javascript"></script>
<script src="../../../Scripts/jquery.validate-vsdoc.js" type="text/javascript"></script>
<% } %>

    <% //using (Ajax.BeginForm(new AjaxOptions
        //                                 {
        //                                     //UpdateTargetId = "status",
        //                                     //InsertionMode = InsertionMode.Replace,
        //                                     OnSuccess = "onSavedSuccess",
        //                                     LoadingElementId = "progress"
        //                                 }

        //    ))
        // {%>
    <%--    <% using (Html.BeginForm())
               { %>
    <%=Html.AntiForgeryToken()%>

 <% List<TreeViewItem> checkedNodes = ViewData["TreeView1_checkedNodes"] as List<TreeViewItem>; %>

     <span id="toolbar" class="ui-widget-header ui-corner-all">
                    <button id="Save" type="submit">
                        Simpan</button>
                </span>
    selectted nodes : <%=ViewData["message"]%>

    <%= Html.Telerik().TreeView()
            .Name("TreeView1")
            .ShowLines(true)
            .ShowCheckBox(true)
            .BindTo(Model, mappings =>
            {
                mappings.For<MMenu>(binding => binding
                        .ItemDataBound((item, menu) =>
                        {
                            item.Text = menu.MenuName;
                            item.Value = menu.Id;
                            //item.Checked = true;
                            if (checkedNodes != null)
                            {
                                var checkedNode = checkedNodes
                                                    .Where(e => e.Value.Equals(menu.Id))
                                                    .FirstOrDefault();

                                item.Checked = checkedNode != null ? true : false;
                            }
                            
                            item.Expanded = true;
                        })
                        .Children(parent => parent.MenuChildren));
                mappings.For<MMenu>(binding => binding
                        .ItemDataBound((item, menu) =>
                        {
                            item.Text = menu.MenuName;
                            item.Value = menu.Id;
                            //item.Checked = true;

                            if (checkedNodes != null)
                            {
                                var checkedNode = checkedNodes
                                                    .Where(e => e.Value.Equals(menu.Id))
                                                    .FirstOrDefault();

                                item.Checked = checkedNode != null ? true : false;
                            }
                            item.Expanded = true;
                        }));
            })
    %>
    <%
       }%>--%>
   <%-- <% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
                                           //InsertionMode = InsertionMode.Replace,
                                           OnBegin = "generateHiddenFieldsForTree",
                                           OnSuccess = "onSavedSuccess",
                                           LoadingElementId = "progress"
                                       }
          ))
       {%>--%>
     <% using (Html.BeginForm())
       { %>
    <%=Html.AntiForgeryToken()%>
    <div id="demoTree" style="height: 350px;width:350px;overflow:auto;">
    </div>
    <br />
    <div>
        <input id="checkedId" name="checkedId" type="hidden" />
        <input type="submit" value="Simpan" id="btnSubmit" />
    </div>
    <% } %>

    <script type="text/javascript">
        function onSavedSuccess(e) {
            var json = e.get_response().get_object();
            var msg = json.Message;
            var status = json.Success;
            alert(msg);
            if (status) {
                $('#btnSubmit').attr('disabled', 'disabled');
            }
        }

         $(document).ready(function () {
            var username = '<%= Request.QueryString["userName"] %>';
            $("#demoTree").tree({
                ui: {
                    theme_name: "checkbox"
                },
                data: {
                    type: "json",
                    opts: {
                        method: "POST",
                        url: "<%= ResolveUrl("~/Utility/UserAdministration/GetTreeData") %>?userName=" + username
                    }
                },
                plugins: {
                    checkbox: {}
                },
                callback: {
                    onload: function (tree) {
                            $.tree.focused().open_all(); 
                        $('li[selected=true]').each(function () {
                            $.tree.plugins.checkbox.check(this);
                        });
                    }
                }
            });

            //$("form").submit(function () { generateHiddenFieldsForTree(); });
        });

        //function generateHiddenFieldsForTree() {
        $("form").submit(function(event) {
       var treeId = "demoTree";
            if ($("#" + treeId).length === 0) {
                alert("invalid treeId for generateHiddenFieldsForTree");
                return;
            }

            var checkedId = "";
            $.tree.plugins.checkbox.get_checked($.tree.reference("#" + treeId)).each(function () {
                checkedId += this.id + ",";
                //$("<input>").attr("type", "text").attr("name", "checkedId").val(checkedId).appendTo("#" + treeId);
                //alert(checkedId);
            });
                $("#checkedId").val(checkedId);
           // return true;

             var form = $(this);
            var url = form.attr('action');
            var formData = form.serialize();
           
           var result = $.ajax({ 
               url: url, 
               data : formData,
               type: "POST",
               async: true, 
               cache: false, 
               success: function (data, result) { 
                    if (!result) alert('Failure to retrieve the Department.');  

                    var msg = data.Message;
                    var status = data.Success;
                    alert(msg);
                    if (status) {
                        $('#btnSubmit').attr('disabled', 'disabled');
                    }
               } 
           });
           return false;
        });
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
