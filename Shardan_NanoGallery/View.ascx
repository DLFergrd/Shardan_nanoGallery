<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Shardan.Modules.Shardan_NanoGallery.View" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<asp:PlaceHolder ID="phScript" runat="server" />
<div id="galleryContainer" class="divgalleryContainer">
    <asp:Label ID="lblGalleryName" runat="server"/>
    <div id="divGalleryView" class="divGalleryView">
        <div id="nanoGalleryMLN<%= ModuleId %>">&nbsp;</div>
        <div id="divViewDescription" class="divViewDescription">
            <asp:Label ID="lblViewDescription" CssClass="lblViewDescription" runat="server"></asp:Label>
        </div>
    </div>
</div>
<dnnweb:DnnFileUpload ID="fileUpload" runat="server"/>
