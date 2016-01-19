<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="Shardan.Modules.Shardan_NanoGallery.Settings" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<div class="dnnForm dnnEditBasicSettings" id="dnnEditBasicSettings">
    <div class="dnnFormExpandContent dnnLeft"><a href=""><%=LocalizeString("ExpandAll")%></a></div>
        
	<h2 id="dnnSitePanel-GallerySettings" class="dnnFormSectionHead dnnClear"><a href="" class="dnnSectionExpanded"><%=LocalizeString("GallerySettings")%></a></h2>
	<fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblGName" runat="server" /> 
            <asp:TextBox ID="txtGName" runat="server" width="150"/>
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblGDescription" runat="server" />
            <asp:TextBox ID="txtGDescription" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:label ID="lblBaseFolderPath" runat="server" />
            <asp:TextBox ID="txtBaseFolderPath" runat="server" ReadOnly="True" />
        </div>
    </fieldset>
    <h2 id="dnnSitePanel-ThumbNailSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("ThumbNailSettings")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblThumbnailWidth" runat="server" /> 
            <asp:TextBox ID="txtThumbnailWidth" runat="server" width="150"/>
            <asp:RangeValidator ID="rvThumbnailWidth" runat="server" ErrorMessage="" ControlToValidate="txtThumbnailWidth" MaximumValue="400" MinimumValue="50" SetFocusOnError="True" Type="Integer" Display="Dynamic" ForeColor="#CC0000"></asp:RangeValidator>
            <asp:RequiredFieldValidator ID="rfvThumbnailWidth" runat="server" ErrorMessage="" ControlToValidate="txtThumbnailWidth"  ForeColor="#CC0000" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblThumbnailHeight" runat="server" /> 
            <asp:TextBox ID="txtThumbnailHeight" runat="server" width="150"/>
            <asp:RangeValidator ID="rvThumbnailHeight" runat="server" ErrorMessage="" ControlToValidate="txtThumbnailHeight" MaximumValue="400" MinimumValue="50" SetFocusOnError="True" Type="Integer" Display="Dynamic" ForeColor="#CC0000"></asp:RangeValidator>
            <asp:RequiredFieldValidator ID="rfvThumbnailHeight" runat="server" ErrorMessage="" ControlToValidate="txtThumbnailHeight"  ForeColor="#CC0000" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblThumbnailHoverEffect" runat="server" /> 
            <asp:DropDownList ID="ddlTHE1" runat="server" Width="150"></asp:DropDownList>
            <asp:DropDownList ID="ddlTHE2" runat="server" Width="150"></asp:DropDownList>
            <asp:DropDownList ID="ddlTHE3" runat="server" Width="150"></asp:DropDownList>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblTheme" runat="server" /> 
            <asp:DropDownList ID="ddlTheme" runat="server" Width="150"></asp:DropDownList>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblColorScheme" runat="server" /> 
            <asp:DropDownList ID="ddlColorScheme" runat="server" Width="150"></asp:DropDownList>
        </div>
    </fieldset>
    <h2 id="dnnSitePanel-OtherSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("OtherSettings")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblMaxItemsPerLine" runat="server" /> 
            <asp:TextBox ID="txtMaxItemsPerLine" runat="server" width="150" />
            <asp:RangeValidator ID="rvMaxItemsPerLine" runat="server" ErrorMessage="" ControlToValidate="txtMaxItemsPerLine" MaximumValue="20" MinimumValue="1" SetFocusOnError="True" Type="Integer" Display="Dynamic" ForeColor="#CC0000"></asp:RangeValidator>
            <asp:RequiredFieldValidator ID="rfvMaxItemsPerLine" runat="server" ErrorMessage="" ControlToValidate="txtMaxItemsPerLine"  ForeColor="#CC0000" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPaginationMaxLinesPerPage" runat="server" /> 
            <asp:TextBox ID="txtPaginationMaxLinesPerPage" runat="server" width="150"/>
            <asp:RangeValidator ID="rvPaginationMaxLinesPerPage" runat="server" ErrorMessage="" ControlToValidate="txtPaginationMaxLinesPerPage" MaximumValue="1000" MinimumValue="1" SetFocusOnError="True" Type="Integer" Display="Dynamic" ForeColor="#CC0000"></asp:RangeValidator>
            <asp:RequiredFieldValidator ID="rfvPaginationMaxLinesPerPage" runat="server" ErrorMessage="" ControlToValidate="txtPaginationMaxLinesPerPage"  ForeColor="#CC0000" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
         </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblThumbnailGutterWidth" runat="server" /> 
            <asp:TextBox ID="txtThumbnailGutterWidth" runat="server" width="150"/>
            <asp:RangeValidator ID="rvThumbnailGutterWidth" runat="server" ErrorMessage="" ControlToValidate="txtThumbnailGutterWidth" MaximumValue="600" MinimumValue="0" SetFocusOnError="True" Type="Integer" Display="Dynamic" ForeColor="#CC0000"></asp:RangeValidator>
            <asp:RequiredFieldValidator ID="rfvThumbnailGutterWidth" runat="server" ErrorMessage="" ControlToValidate="txtThumbnailGutterWidth"  ForeColor="#CC0000" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblThumbnailGutterHeight" runat="server" /> 
            <asp:TextBox ID="txtThumbnailGutterHeight" runat="server" width="150"/>
            <asp:RangeValidator ID="rvThumbnailGutterHeight" runat="server" ErrorMessage="" ControlToValidate="txtThumbnailGutterHeight" MaximumValue="600" MinimumValue="0" SetFocusOnError="True" Type="Integer" Display="Dynamic" ForeColor="#CC0000"></asp:RangeValidator>
            <asp:RequiredFieldValidator ID="rfvThumbnailGutterHeight" runat="server" ErrorMessage="" ControlToValidate="txtThumbnailGutterHeight"  ForeColor="#CC0000" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
        <%--<div class="dnnFormItem">
            <dnn:Label ID="lblCountDisplay" runat="server" /> 
            <asp:DropDownList ID="ddlCountDisplay" runat="server" Width="150"></asp:DropDownList>
        </div>--%>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPaginationDots" runat="server" /> 
            <asp:RadioButtonList ID="rblPaginationDots" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblLocationHash" runat="server" /> 
            <asp:RadioButtonList ID="rblLocationHash" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblBreadcrumbAutoHideTopLevel" runat="server" /> 
            <asp:RadioButtonList ID="rblBreadcrumbAutoHideTopLevel" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
        </div>
    </fieldset>
    <h2 id="dnnSitePanel-EditWindowSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("EditWindowSettings")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblLinesPerPage" runat="server" /> 
            <asp:TextBox ID="txtLinesPerPage" runat="server" width="150" />
            <asp:RangeValidator ID="rvLinesPerPage" runat="server" ErrorMessage="" ControlToValidate="txtLinesPerPage" MaximumValue="20000" MinimumValue="3" SetFocusOnError="True" Type="Integer" Display="Dynamic" ForeColor="#CC0000"></asp:RangeValidator>
            <asp:RequiredFieldValidator ID="rfvLinesPerPage" runat="server" ErrorMessage="" ControlToValidate="txtLinesPerPage"  ForeColor="#CC0000" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
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

