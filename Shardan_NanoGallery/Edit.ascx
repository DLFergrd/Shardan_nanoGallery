<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="Shardan.Modules.Shardan_NanoGallery.Edit" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm dnnEditBasicSettings" id="dnnEditBasicSettings">
    <div class="dnnFormExpandContent dnnRight"><a href=""><%=LocalizeString("ExpandAll")%></a></div>

    <h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead dnnClear"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings")%></a></h2>
    <fieldset>
        <asp:Panel ID="pnlSingle" runat="server">
            <div class="divDisplayPhoto">
                <asp:Image CssClass="imgDisplayPhoto" ID="imgDisplayPhoto" ImageUrl="/" runat="server" />
            </div>
            <div class="divItems">
                <div class="dnnFormItem">
                    <dnn:label ID="lblTitle" runat="server" />
                    <asp:TextBox ID="txtTitle" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:label ID="lblFileName" runat="server" />
                    <asp:TextBox ID="txtFileName" runat="server" ReadOnly="true" />
                </div>
                <div class="dnnFormItem">
                    <dnn:label ID="lblDescription" runat="server" />
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="20" />
                </div>
                <div class="dnnFormItem">
                    <dnn:label ID="lblAlbumKind" runat="server" />
                    <asp:RadioButtonList ID="rblItemKind" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
                </div>
                <div class="dnnFormItem">
                    <dnn:label ID="lblAlbumID" runat="server" />
                    <asp:DropDownList ID="ddlAlbumID" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:label ID="lblThisPictureID" runat="server" />
                    <asp:TextBox ID="txtThisPictureID" runat="server" ReadOnly="True" />
                </div>
                
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlMulti" runat="server">
        <asp:ListView ID="lvGalleryList" runat="server" OnItemDeleting="lvGalleryList_ItemDeleting" OnItemCommand="lvGalleryList_ItemCommand" OnItemDataBound="lvGalleryList_ItemDataBound" GroupItemCount="10" GroupPlaceholderID="Placeholder1" ItemPlaceholderID="Placeholder2">
            <LayoutTemplate>
                <div runat="server" id="divtable1">
                    <div runat="server" id="Placeholder1"></div>
                </div>
            </LayoutTemplate>
            <GroupTemplate>
                <div runat="server" id="divtableRow">
                    <div runat="server" id="Placeholder2"></div>
                </div>
            </GroupTemplate>
            <ItemTemplate>
                <div class="divPhotolistcontent" id="divPhotolistcontent">
                    <div class="divImagArea" id="divImageArea">
                        <div id="divEditArea" class="divEditArea">
                            <div id="divCheckBox" class="divCheckBox">
                                <asp:CheckBox ID="chkSelectPicture" runat="server" />
                            </div>
                            <div id="editlink" class="divEditLink">
                                <asp:HyperLink ID="lnkEditPicture" Visible="false" Enabled="false" runat="server">
                                    <asp:Image ID="editLinkImage" ImageUrl="~/images/eip_edit.png" AlternateText="Edit" Visible="false" runat="server" />
                                </asp:HyperLink>
                            </div>
                            <div id="divDel" class="divDel">
                                <asp:ImageButton ID="lnkPictureDelete" runat="server" CssClass="lbPictureDelete" ImageUrl="~/images/delete.gif" CommandName="Delete" Visible="true" />
                            </div>
                        </div>
                        <div id="divImage" class="divImage">
                                <asp:Image CssClass="imgPhoto" ID="imgPhoto" ImageUrl="/" runat="server" />
                        </div>
                    </div>
                    <div id="divInfoArea" class="divInfoArea">
                        <div id="divPicName">
                            <div id="divPictureName" class="divPictureName">
                                <asp:label cssclass="lblPictureName" id="lblPictureName" runat="server" Font-Bold="True"></asp:label>
                                <asp:label CssClass="lblItemName" ID="lblItemName" runat="server" Columns="40"></asp:label>
                            </div>
                        </div>
                        <div id="divPicDesc">
                            <div class="divPictureDescription">
                                <asp:label cssclass="lblPictureDescription" id="lblPictureDescription" runat="server" Font-Bold="True"></asp:label>
                                <asp:label CssClass="lblItemDescription" ID="lblItemDescription" runat="server" ></asp:label>
                            </div>
                        </div>
                        <div id="divT">
                            <div id="divType" class="divType">
                                <asp:Label CssClass="lblType" ID="lblType" runat="server" Font-Bold="True"></asp:Label>
                                <asp:label ID="lblItemType" runat="server" ></asp:label>
                            </div>
                        </div>
                        <div id="divAID">
                            <div id="divAlbumID" class="divAlbumID">
                                <asp:Label CssClass="lblThisAlbumID" ID="lblThisAlbumID" runat="server" Font-Bold="True"></asp:Label>
                                <asp:Label ID="lblThisItemAlbumID" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div id="divTIID">
                            <div id="divThisItemID" class="divThisItemID">
                                <asp:Label CssClass="lblTIID" ID="lblTIID" runat="server" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblThisItemID" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
                <br /><hr /><br />
            </ItemTemplate>
        </asp:ListView>
            <asp:DataPager ID="dbPictureList" runat="server" PagedControlID="lvGalleryList" OnPreRender="dbPictureList_PreRender" ValidateRequestMode="Inherit" PageSize="4">
                <Fields>
                    <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="true" ShowPreviousPageButton="true" ShowLastPageButton="true" ShowNextPageButton ="true"/>
                    <asp:NumericPagerField ButtonType="Link" ButtonCount="20" />
                    <asp:TemplatePagerField>
                         <PagerTemplate>
                             <b>showing&nbsp;<asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1 %>" />&nbsp;to
                             <asp:Label runat="server" ID="ItemsThisPageLabel" Text="<%# Container.StartRowIndex+Container.PageSize > Container.TotalRowCount ? Container.TotalRowCount : Container.StartRowIndex+Container.PageSize %>" />&nbsp;( of&nbsp;
                             <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />&nbsp;records)
                             <br />
                             </b>
                         </PagerTemplate>
                     </asp:TemplatePagerField>
                </Fields>
            </asp:DataPager>
            <div id="divCommandArea" class="divCommandArea">
                <div id="divCommandDelete" class="divCommandDelete">
                    <asp:LinkButton ID="btnDeleteSelected" runat="server" resourcekey="btnDeleteSelected" OnClick="btnDeleteSelected_Click"  CssClass="dnnSecondaryAction" />
                </div>
                <div id="divCommandAdd" class="divCommandAdd">
                    <asp:LinkButton ID="btnAddToAlbum" runat="server" resourcekey="btnAddToAlbum" OnClick="btnAddToAlbum_Click" CssClass="dnnSecondaryAction" />
                </div>
                <div id="divCommandDDL" class="divCommandDDL">
                    <asp:DropDownList ID="ddlAlbumList" runat="server"></asp:DropDownList>
                </div>
            </div>
        </asp:Panel>
    </fieldset>
</div>


<script type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($, Sys) {
        function dnnEditBasicSettings() {
            $('#dnnEditBasicSettings').dnnPanels();
            $('#dnnEditBasicSettings .dnnFormExpandContent a').dnnExpandAll({ expandText: '<%=Localization.GetString("ExpandAll", LocalResourceFile)%>', collapseText: '<%=Localization.GetString("CollapseAll", LocalResourceFile)%>', targetArea: '#dnnEditBasicSettings' });
        }

        $(document).ready(function () {
            dnnEditBasicSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                dnnEditBasicSettings();
            });
        });

    }(jQuery, window.Sys));
</script>

<asp:LinkButton ID="btnSubmit" runat="server"
    OnClick="btnSubmit_Click" resourcekey="btnSubmit" CssClass="dnnPrimaryAction" />
<asp:LinkButton ID="btnCancel" runat="server"
    OnClick="btnCancel_Click" resourcekey="btnCancel" CssClass="dnnSecondaryAction" />
<asp:LinkButton ID="btnDone" runat="server"
    OnClick="btnDone_Click" resourcekey="btnDone" CssClass="dnnSecondaryAction" />


