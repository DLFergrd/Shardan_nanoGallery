/*
' Copyright (c) 2016  Shardan
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;
using System.Web.UI.WebControls;

namespace Shardan.Modules.Shardan_NanoGallery
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Settings class manages Module Settings
    /// 
    /// Typically your settings control would be used to manage settings for your module.
    /// There are two types of settings, ModuleSettings, and TabModuleSettings.
    /// 
    /// ModuleSettings apply to all "copies" of a module on a site, no matter which page the module is on. 
    /// 
    /// TabModuleSettings apply only to the current module on the current page, if you copy that module to
    /// another page the settings are not transferred.
    /// 
    /// If you happen to save both TabModuleSettings and ModuleSettings, TabModuleSettings overrides ModuleSettings.
    /// 
    /// Below we have some examples of how to access these settings but you will need to uncomment to use.
    /// 
    /// Because the control inherits from Shardan_NanoGallerySettingsBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Settings : Shardan_NanoGalleryModuleSettingsBase
    {
        #region Base Method Implementations

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// LoadSettings loads the settings from the Database and displays them
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void LoadSettings()
        {
            try
            {
                //Set text
                rvThumbnailWidth.ErrorMessage = GetLocalizedString("Error.ThumbnailWidth");
                rvThumbnailHeight.ErrorMessage = GetLocalizedString("Error.ThumbnailHeight");
                rvMaxItemsPerLine.ErrorMessage = GetLocalizedString("Error.ItemsPerLine");
                rvPaginationMaxLinesPerPage.ErrorMessage = GetLocalizedString("Error.LinesPerPage");
                rvThumbnailGutterWidth.ErrorMessage = GetLocalizedString("Error.GutterWidth");
                rvThumbnailGutterHeight.ErrorMessage = GetLocalizedString("Error.GutterHeight");
                rfvThumbnailWidth.ErrorMessage = GetLocalizedString("Error.Number");
                rfvThumbnailHeight.ErrorMessage = GetLocalizedString("Error.Number");
                rfvMaxItemsPerLine.ErrorMessage = GetLocalizedString("Error.Number");
                rfvPaginationMaxLinesPerPage.ErrorMessage = GetLocalizedString("Error.Number");
                rfvThumbnailGutterWidth.ErrorMessage = GetLocalizedString("Error.Number");
                rfvThumbnailGutterHeight.ErrorMessage = GetLocalizedString("Error.Number");
                rvLinesPerPage.ErrorMessage = GetLocalizedString("Error.LinesPerEditPage");
                rfvLinesPerPage.ErrorMessage = GetLocalizedString("Error.Number");

                if (!Page.IsPostBack)
                {
                    //Check for existing settings and use those on this page
                    if (Settings.Contains("GalleryName")) txtGName.Text = Settings["GalleryName"].ToString();
                    if (Settings.Contains("GalleryDescription")) txtGDescription.Text = Settings["GalleryDescription"].ToString();
                    if (Settings.Contains("BaseFolderPath"))
                    {
                        txtBaseFolderPath.Text = Settings["BaseFolderPath"].ToString();
                        txtBaseFolderPath.ReadOnly = true;
                    }
                    else
                    {
                        //this gallery has not been configured
                        //make sure directories exists
                        txtBaseFolderPath.Text = "portals/" + PortalId.ToString() + "/Gallery/Nano/" + ModuleId.ToString();

                        var folderMapping = FolderMappingController.Instance.GetDefaultFolderMapping(PortalId);
                        if (!FolderManager.Instance.FolderExists(PortalId, "Gallery"))
                        {
                            
                            //create the "Gallery" folder
                            FolderManager.Instance.AddFolder(folderMapping, "Gallery");
                        }

                        if (!FolderManager.Instance.FolderExists(PortalId, "Gallery/Nano"))
                        {
                            //create the "Nano" folder
                            FolderManager.Instance.AddFolder(folderMapping, "Gallery/Nano");
                        }
                        if (!FolderManager.Instance.FolderExists(PortalId, "Gallery/Nano/" + ModuleId.ToString()))
                        {
                            // create the ModuleID folder
                            FolderManager.Instance.AddFolder(folderMapping, "Gallery/Nano/" + ModuleId.ToString());
                        }
                    }

                    if (Settings.Contains("ThumbnailWidth")) txtThumbnailWidth.Text = Settings["ThumbnailWidth"].ToString();
                    else txtThumbnailWidth.Text = "120";

                    if (Settings.Contains("ThumbnailHeight")) txtThumbnailHeight.Text = Settings["ThumbnailHeight"].ToString();
                    else txtThumbnailHeight.Text = "120";

                    if (Settings.Contains("MaxItemsPerLine")) txtMaxItemsPerLine.Text = Settings["MaxItemsPerLine"].ToString();
                    else txtMaxItemsPerLine.Text = "4";

                    if (Settings.Contains("PaginationMaxLinesPerPage")) txtPaginationMaxLinesPerPage.Text = Settings["PaginationMaxLinesPerPage"].ToString();
                    else txtPaginationMaxLinesPerPage.Text = "4";
                    
                    FillDropDownLists();

                    if (Settings.Contains("ThumbnailHoverEffect1")) ddlTHE1.SelectedValue = Settings["ThumbnailHoverEffect1"].ToString();
                    if (Settings.Contains("ThumbnailHoverEffect2")) ddlTHE2.SelectedValue = Settings["ThumbnailHoverEffect2"].ToString();
                    if (Settings.Contains("ThumbnailHoverEffect3")) ddlTHE3.SelectedValue = Settings["ThumbnailHoverEffect3"].ToString();
                    if (Settings.Contains("Theme")) ddlTheme.SelectedValue = Settings["Theme"].ToString();
                    if (Settings.Contains("BreadcrumbAutoHideTopLevel")) rblBreadcrumbAutoHideTopLevel.SelectedValue = Settings["BreadcrumbAutoHideTopLevel"].ToString();
                    else rblBreadcrumbAutoHideTopLevel.SelectedValue = "true";
                    if (Settings.Contains("PaginationDots")) rblPaginationDots.SelectedValue = Settings["PaginationDots"].ToString();
                    else rblPaginationDots.SelectedValue = "true";
                    if (Settings.Contains("ColorScheme")) ddlColorScheme.SelectedValue = Settings["ColorScheme"].ToString();
                    if (Settings.Contains("LocationHash")) rblLocationHash.SelectedValue = Settings["LocationHash"].ToString();
                    else rblLocationHash.SelectedValue = "false";
                    if (Settings.Contains("ThumbnailGutterHeight")) txtThumbnailGutterHeight.Text = Settings["ThumbnailGutterHeight"].ToString();
                    else txtThumbnailGutterHeight.Text = "2";
                    if (Settings.Contains("ThumbnailGutterWidth")) txtThumbnailGutterWidth.Text = Settings["ThumbnailGutterWidth"].ToString();
                    else txtThumbnailGutterWidth.Text = "2";
                    if (Settings.Contains("EditLinesPerPage")) txtLinesPerPage.Text = Settings["EditLinesPerPage"].ToString();
                    else txtLinesPerPage.Text = "5";

                    //TODO Advanced Setting
                    //thumbnailLabel.itemsCount   string Displays the number of items per photo album.
                    //Possible values: 'none', 'title', 'description'
                    //if (Settings.Contains("CountDisplay")) ddlCountDisplay.SelectedValue = Settings["CountDisplay"].ToString();

                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UpdateSettings()
        {
            //make sure page is valid
            if (Page.IsValid)
            {
                try
                {
                    var modules = new ModuleController();
                    modules.UpdateTabModuleSetting(TabModuleId, "GalleryName", txtGName.Text);
                    modules.UpdateTabModuleSetting(TabModuleId, "GalleryDescription", txtGDescription.Text);
                    modules.UpdateTabModuleSetting(TabModuleId, "BaseFolderPath", txtBaseFolderPath.Text);
                    modules.UpdateTabModuleSetting(TabModuleId, "ThumbnailWidth", txtThumbnailWidth.Text);
                    modules.UpdateTabModuleSetting(TabModuleId, "ThumbnailHeight", txtThumbnailHeight.Text);
                    modules.UpdateTabModuleSetting(TabModuleId, "MaxItemsPerLine", txtMaxItemsPerLine.Text);
                    modules.UpdateTabModuleSetting(TabModuleId, "PaginationMaxLinesPerPage", txtPaginationMaxLinesPerPage.Text);
                    modules.UpdateTabModuleSetting(TabModuleId, "ThumbnailHoverEffect1", ddlTHE1.SelectedValue);
                    modules.UpdateTabModuleSetting(TabModuleId, "ThumbnailHoverEffect2", ddlTHE2.SelectedValue);
                    modules.UpdateTabModuleSetting(TabModuleId, "ThumbnailHoverEffect3", ddlTHE3.SelectedValue);
                    modules.UpdateTabModuleSetting(TabModuleId, "Theme", ddlTheme.SelectedValue);
                    modules.UpdateTabModuleSetting(TabModuleId, "BreadcrumbAutoHideTopLevel", rblBreadcrumbAutoHideTopLevel.SelectedValue);
                    modules.UpdateTabModuleSetting(TabModuleId, "PaginationDots", rblPaginationDots.SelectedValue);
                    modules.UpdateTabModuleSetting(TabModuleId, "ColorScheme", ddlColorScheme.SelectedValue);
                    modules.UpdateTabModuleSetting(TabModuleId, "LocationHash", rblLocationHash.SelectedValue);
                    modules.UpdateTabModuleSetting(TabModuleId, "ThumbnailGutterHeight", txtThumbnailGutterHeight.Text);
                    modules.UpdateTabModuleSetting(TabModuleId, "ThumbnailGutterWidth", txtThumbnailGutterWidth.Text);
                    modules.UpdateTabModuleSetting(TabModuleId, "EditLinesPerPage", txtLinesPerPage.Text);
                    //modules.UpdateTabModuleSetting(TabModuleId, "CountDisplay", ddlCountDisplay.SelectedValue);
                }
                catch (Exception exc) //Module failed to load
                {
                    Exceptions.ProcessModuleLoadException(this, exc);
                }
            }
        }

        #endregion
        public void FillDropDownLists()
        {
            // Create ddlist for the thumbnail effects
            
            ddlTHE1.Items.Add(new ListItem("none","none"));
            ddlTHE1.Items.Add(new ListItem("borderLighter", "borderLighter"));
            ddlTHE1.Items.Add(new ListItem("borderDarker", "borderDarker"));
            ddlTHE1.Items.Add(new ListItem("scale120", "scale120"));
            ddlTHE1.Items.Add(new ListItem("scaleLabelOverImage", "scaleLabelOverImage"));
            ddlTHE1.Items.Add(new ListItem("overScale", "overScale"));
            ddlTHE1.Items.Add(new ListItem("overScaleOutside", "overScaleOutside"));
            ddlTHE1.Items.Add(new ListItem("slideUp", "slideUp"));
            ddlTHE1.Items.Add(new ListItem("slideDown", "slideDown"));
            ddlTHE1.Items.Add(new ListItem("slideRight", "slideRight"));
            ddlTHE1.Items.Add(new ListItem("slideLeft", "slideLeft"));
            ddlTHE1.Items.Add(new ListItem("rotateCornerBL", "rotateCornerBL"));
            ddlTHE1.Items.Add(new ListItem("rotateCornerBR", "rotateCornerBR"));
            ddlTHE1.Items.Add(new ListItem("imageScale150", "imageScale150"));
            ddlTHE1.Items.Add(new ListItem("imageScaleIn80", "imageScaleIn80"));
            ddlTHE1.Items.Add(new ListItem("imageScale150Outside", "imageScale150Outside"));
            ddlTHE1.Items.Add(new ListItem("imageSplit4", "imageSplit4"));
            ddlTHE1.Items.Add(new ListItem("imageSplitVert", "imageSplitVert"));
            ddlTHE1.Items.Add(new ListItem("imageSlideUp", "imageSlideUp"));
            ddlTHE1.Items.Add(new ListItem("imageSlideDown", "imageSlideDown"));
            ddlTHE1.Items.Add(new ListItem("imageSlideRight", "imageSlideRight"));
            ddlTHE1.Items.Add(new ListItem("imageSlideLeft", "imageSlideLeft"));
            ddlTHE1.Items.Add(new ListItem("imageRotateCornerBL", "imageRotateCornerBL"));
            ddlTHE1.Items.Add(new ListItem("imageRotateCornerBR", "imageRotateCornerBR"));
            ddlTHE1.Items.Add(new ListItem("imageFlipHorizontal", "imageFlipHorizontal"));
            ddlTHE1.Items.Add(new ListItem("imageFlipVertical", "imageFlipVertical"));
            ddlTHE1.Items.Add(new ListItem("labelAppear", "labelAppear"));
            ddlTHE1.Items.Add(new ListItem("labelAppear75", "labelAppear75"));
            ddlTHE1.Items.Add(new ListItem("labelOpacity50", "labelOpacity50"));
            ddlTHE1.Items.Add(new ListItem("descriptionAppear", "descriptionAppear"));
            ddlTHE1.Items.Add(new ListItem("descriptionSlideUp", "descriptionSlideUp"));
            ddlTHE1.Items.Add(new ListItem("labelSlideUpTop", "labelSlideUpTop"));
            ddlTHE1.Items.Add(new ListItem("labelSlideUp", "labelSlideUp"));
            ddlTHE1.Items.Add(new ListItem("labelSlideDown", "labelSlideDown"));
            ddlTHE1.Items.Add(new ListItem("labelSplit4", "labelSplit4"));
            ddlTHE1.Items.Add(new ListItem("labelSplitVert", "labelSplitVert"));
            ddlTHE1.Items.Add(new ListItem("labelAppearSplit4", "labelAppearSplit4"));
            ddlTHE1.Items.Add(new ListItem("labelAppearSplitVert", "labelAppearSplitVert"));

            foreach (ListItem li in ddlTHE1.Items)
            {
                ddlTHE2.Items.Add(new ListItem(li.Text,li.Value));
                ddlTHE3.Items.Add(new ListItem(li.Text,li.Value));
            }

            ddlTheme.Items.Add(new ListItem("clean","'clean'"));
            ddlTheme.Items.Add(new ListItem("lignt","'light'"));

            rblBreadcrumbAutoHideTopLevel.Items.Add(new ListItem("True", "true"));
            rblBreadcrumbAutoHideTopLevel.Items.Add(new ListItem("False", "false"));

            rblPaginationDots.Items.Add(new ListItem("True", "true"));
            rblPaginationDots.Items.Add(new ListItem("False", "false"));

            rblLocationHash.Items.Add(new ListItem("True", "true"));
            rblLocationHash.Items.Add(new ListItem("False", "false"));

            //'none', 'dark','darkRed', 'darkGreen', 'darkBlue', 'darkOrange', 'light', 'lightBackground'
            ddlColorScheme.Items.Add(new ListItem("None", "'none'"));
            ddlColorScheme.Items.Add(new ListItem("Dark", "'dark'"));
            ddlColorScheme.Items.Add(new ListItem("Dark Red", "'darkRed'"));
            ddlColorScheme.Items.Add(new ListItem("Dark Green", "'darkGreen'"));
            ddlColorScheme.Items.Add(new ListItem("Dark Blue", "'darkBlue'"));
            ddlColorScheme.Items.Add(new ListItem("Dark Orange", "'darkOrange'"));
            ddlColorScheme.Items.Add(new ListItem("Light", "'light'"));
            ddlColorScheme.Items.Add(new ListItem("Light Background", "'lightBackground'"));

            ////Possible values: 'none', 'title', 'description'
            //ddlCountDisplay.Items.Add(new ListItem("None", "'none'"));
            //ddlCountDisplay.Items.Add(new ListItem("Title", "'title'"));
            //ddlCountDisplay.Items.Add(new ListItem("Description", "'description'"));

        }
    }
}