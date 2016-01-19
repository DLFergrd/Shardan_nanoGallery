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
//using System.Web.UI.WebControls;
using Shardan.Modules.Shardan_NanoGallery.Components;
using DotNetNuke.Common;
//using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
//using DotNetNuke.UI.Utilities;
using DotNetNuke.Web.UI.WebControls;
using DotNetNuke.Web.Client.ClientResourceManagement;
using System.Text;
using System.Web.UI;
using DotNetNuke.UI.Skins;
using DotNetNuke.UI.Skins.Controls;
using System.Collections.Generic;
using System.Linq;


namespace Shardan.Modules.Shardan_NanoGallery
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from Shardan_NanoGalleryModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : Shardan_NanoGalleryModuleBase , IActionable
    {
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);

                fileUpload.ModuleId = ModuleId;
                fileUpload.Options.Parameters.Add("isHostPortal", "false");

                //Make sure folder exists - Module configured
                if (Settings.Contains("BaseFolderPath"))
                {
                    //Get default upload directory for this module
                    var folder = DotNetNuke.Services.FileSystem.FolderManager.Instance.GetFolder(PortalId, "Gallery/Nano/" + ModuleId.ToString());

                    if (fileUpload.Options.FolderPicker.InitialState == null)
                    {
                        //Set uploader directory
                        DnnDropDownListState myDDLState = new DnnDropDownListState();
                        myDDLState.SelectedItem = new SerializableKeyValuePair<string, string>(folder.FolderID.ToString(), folder.FolderName);
                        fileUpload.Options.FolderPicker.InitialState = myDDLState;
                    }
                    //Set allowed extensions
                    List<string> extensionList = new List<string>();
                    extensionList.Add("jpg");
                    extensionList.Add("gif");
                    extensionList.Add("bmp");
                    extensionList.Add("png");
                    extensionList.Add("zip");
                    fileUpload.Options.Extensions = extensionList;

                    //Set other options
                    fileUpload.Options.FolderPath = folder.MappedPath + "/";
                    fileUpload.Options.FolderPicker.Disabled = true;
                    fileUpload.Options.Width = 900;
                    fileUpload.Options.Height = 700;
                    fileUpload.ShowOnStartup = true;

                    fileUpload.DataBind();
                }
                // Add some security
                ServicesFramework.Instance.RequestAjaxScriptSupport();
                ServicesFramework.Instance.RequestAjaxAntiForgerySupport();

                //Add action event handler
                AddActionHandler(new ActionEventHandler(MyActions_Click));
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // make sure file upload is not visible
            fileUpload.Visible = false;
            try
            {
                // Load the nanoGallery plugin client script on every page load
                ClientResourceManager.RegisterScript(Page, string.Concat(ControlPath, "js/jquery.nanogallery.min.js"));

                // Load the nanoGallery plugin CSS
                ClientResourceManager.RegisterStyleSheet(Page, string.Concat(ControlPath, "js/css/nanogallery.min.css"));
                ClientResourceManager.RegisterStyleSheet(Page, string.Concat(ControlPath, "js/css/themes/clean/nanogallery_clean.min.css"));
                ClientResourceManager.RegisterStyleSheet(Page, string.Concat(ControlPath, "js/css/themes/light/nanogallery_light.min.css"));

                // check to see if this module has been configured
                if (Settings.Contains("BaseFolderPath"))
                {
                    lblGalleryName.Text = Settings["GalleryName"].ToString();
                    lblViewDescription.Text = Settings["GalleryDescription"].ToString();
                    WriteOutnanoGalleryConstructors();
                }
                else
                {
                    //No settings yet so set message
                    Skin.AddModuleMessage(this, GetLocalizedString("Error.NoAlbums"), ModuleMessage.ModuleMessageType.BlueInfo);
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void WriteOutnanoGalleryConstructors()
        {
            //Create the javascript
            var sb = new StringBuilder();

            sb.Append("<script>");
            sb.Append(Environment.NewLine);
            sb.Append("$(document).ready(function (){ ");
            sb.Append(Environment.NewLine);
            string contentNameID = "contentGalleryMLN" + ModuleId.ToString();
            sb.Append("var " + contentNameID + "=[");
            sb.Append(Environment.NewLine);

            // get items for this module from db
            var tc = new ItemController();
            IEnumerable<Item> dblist = tc.GetItems(ModuleId);   // Get all the items for this module from DB

            int _items = dblist.Count();                       //get number of pictures
            if (_items > 0)                               //If there are any process them
            {
                for (int ii = 0; ii < _items; ii++)
                {
                    Item _picture = new Item();
                    //get a single item
                    _picture = dblist.ElementAt(ii);               
                    sb.Append("{src:'" + _picture.ItemFileName + "',");
                    sb.Append("srct:'thm_" + _picture.ItemFileName + "',");
                    sb.Append("title: '" + _picture.ItemTitle + "',");
                    sb.Append("description: '" + _picture.ItemDescription + "',");
                    sb.Append("ID: " + _picture.ItemId.ToString() + ",");
                    if (_picture.AlbumID != 0) sb.Append("albumID: " + _picture.AlbumID.ToString() + ",");
                    if (_picture.ItemKind == "album") sb.Append("kind: '" + _picture.ItemKind + "'");
                    sb.Append("},"); //close item definition  //add a comma after the } to add more items
                    sb.Append(Environment.NewLine);
                }
            }
            //close array list
            sb.Append("];");                     
            sb.Append(Environment.NewLine);

            //Add gallery settings
            string divNameID = "\"#nanoGalleryMLN" + ModuleId.ToString() + "\"";
            sb.Append("jQuery(" + divNameID + ").nanoGallery({");
            sb.Append(Environment.NewLine);
            sb.Append("items: " + contentNameID + ",");
            if (Settings.Contains("ThumbnailWidth")) sb.Append("thumbnailWidth: " + Settings["ThumbnailWidth"].ToString() + ",");
            if (Settings.Contains("ThumbnailHeight")) sb.Append("thumbnailHeight: " + Settings["ThumbnailHeight"].ToString() + ",");
            if (Settings.Contains("MaxItemsPerLine")) sb.Append("maxItemsPerLine: " + Settings["MaxItemsPerLine"].ToString() + ",");
            if (Settings.Contains("PaginationMaxLinesPerPage")) sb.Append("paginationMaxLinesPerPage: " + Settings["PaginationMaxLinesPerPage"].ToString() + ",");
            if (Settings.Contains("Theme")) sb.Append("theme: " + Settings["Theme"].ToString() + ",");
            if (Settings.Contains("BreadcrumbAutoHideTopLevel")) sb.Append("breadcrumbAutoHideTopLevel: " + Settings["BreadcrumbAutoHideTopLevel"].ToString() + ",");
            if (Settings.Contains("ThumbnailHoverEffect1"))
            {
                if (Settings["ThumbnailHoverEffect1"].ToString() != "none")
                {
                    sb.Append("thumbnailHoverEffect: '");

                    if (Settings["ThumbnailHoverEffect1"].ToString() != "none")
                    {
                        sb.Append(Settings["ThumbnailHoverEffect1"].ToString());
                    }
                    if (Settings["ThumbnailHoverEffect2"].ToString() != "none" && Settings["ThumbnailHoverEffect1"].ToString() != "none")
                    {
                        sb.Append("," + Settings["ThumbnailHoverEffect2"].ToString());
                    }
                    if (Settings["ThumbnailHoverEffect3"].ToString() != "none" && Settings["ThumbnailHoverEffect2"].ToString() != "none" && Settings["ThumbnailHoverEffect1"].ToString() != "none")
                    {
                        sb.Append("," + Settings["ThumbnailHoverEffect3"].ToString());
                    }
                    sb.Append("',");
                }
            }
            //sb.Append("paginationDots: true,");
            if (Settings.Contains("PaginationDots")) sb.Append("paginationDots: " + Settings["PaginationDots"].ToString() + ",");

            //colorScheme
            if (Settings.Contains("ColorScheme")) sb.Append("colorScheme: " + Settings["ColorScheme"].ToString() + ",");

            //locationHash
            if (Settings.Contains("LocationHash")) sb.Append("locationHash: " + Settings["LocationHash"].ToString() + ",");
            //thumbnailGutterWidth    thumbnailGutterHeight
            if (Settings.Contains("ThumbnailGutterWidth")) sb.Append("thumbnailGutterWidth: " + Settings["ThumbnailGutterWidth"].ToString() + ",");
            if (Settings.Contains("ThumbnailGutterHeight")) sb.Append("thumbnailGutterHeight: " + Settings["ThumbnailGutterHeight"].ToString() + ",");

            //thumbnailLabel.itemsCount
            //this is a more advanced option todo later
            //if (Settings.Contains("CountDisplay")) sb.Append("thumbnailLabel.itemsCount: " + Settings["CountDisplay"].ToString() + ",");

            sb.Append("viewerDisplayLogo: false,");
            //sb.AppendFormat("'changeSpeed': {0}, ", objSetting.ChangeSpeed);   ////example
            sb.AppendFormat("itemsBaseURL: '/{0}',", Settings["BaseFolderPath"].ToString());
            //add comma to add more items
            //Last line does not have a comma!!!
            sb.Append("useTags: false");
            sb.Append("})");

            sb.Append(Environment.NewLine);
            sb.Append("});");
            sb.Append(Environment.NewLine);
            sb.Append("</script>");

            phScript.Controls.Add(new LiteralControl(sb.ToString()));
        }
        protected void RescanDirectory()
        {
            //Increase timeout
            Server.ScriptTimeout = 600;

            //get list of files
            IEnumerable<FileInfo> filelist = FileUtility.GetSafeFileList(PicturePath, GetExcludedFiles(), GetSortOrder());

            int filelistcount = filelist.Count();
            //If there are any process them
            if (filelistcount > 0)
            {
                //get list from db
                var tc = new ItemController();
                IEnumerable<Item> dblist = tc.GetItems(ModuleId);

                foreach (FileInfo file in filelist)
                {
                    bool isinDB = false;

                    //see if this file is in dm
                    foreach (Item picture in dblist)
                    {
                        if (file.FileName.Contains("thm_") || file.FileName == picture.ItemFileName)
                        {
                            isinDB = true;
                            break;
                        }
                    }
                    if (!isinDB)  //picture is not in db so add it and create thumbnail
                    {
                        Item addPic = new Item();
                        //check for bad filename
                        string goodFileName = RemoveCharactersFromString(file.FileName," '&#<>");
                        if(goodFileName != file.FileName)
                        {
                            //rename the file and use goodfilename instead of file.filename

                            string myPath = Server.MapPath("portals/" + PortalId + "/Gallery/Nano/" + ModuleId + "/");
                            string myOldPath = myPath + file.FileName;
                            string myNewPath = myPath + goodFileName;

                            System.IO.FileInfo f = new System.IO.FileInfo(myOldPath);

                            f.CopyTo(myNewPath);

                            f.Delete();

                        }
                        addPic.ItemFileName = goodFileName;
                        addPic.ItemTitle = goodFileName;
                        addPic.ItemKind = "";
                        addPic.AlbumID = 0;
                        addPic.ItemDescription = "New Picture";
                        addPic.CreatedOnDate = DateTime.Now;
                        addPic.LastModifiedOnDate = DateTime.Now;
                        addPic.LastModifiedByUserId = UserId;
                        addPic.ModuleId = ModuleId;

                        //add to db
                        var tc1 = new ItemController();
                        tc1.CreateItem(addPic);

                        // Get image
                        System.Drawing.Image image = System.Drawing.Image.FromFile(PicturePath + "\\" + goodFileName);

                        float x = image.Width;
                        float y = image.Height;
                        float scale = x / y;
                        int newx = 120;
                        int newy = Convert.ToInt32(120 / scale);
                        if (newy > 120)
                        {
                            newy = 120;
                            newx = Convert.ToInt32(120 * scale);
                        }
                        //create thumbnail
                        System.Drawing.Image thumb = image.GetThumbnailImage(newx, newy, () => false, IntPtr.Zero);
                        thumb.Save(PicturePath + "\\thm_" + goodFileName);
                    }
                }
            }
            //Reload page
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }
        protected void RecreateThumbnails()
        {
            //Get all items from db
            var tc = new ItemController();
            IEnumerable<Item> dblist = tc.GetItems(ModuleId);
            foreach (Item picture in dblist)
            {
                string thumbName = "thm_" + picture.ItemFileName;
                //replace old thumbnail with new thumbnail
                // Get image
                System.Drawing.Image image = System.Drawing.Image.FromFile(PicturePath + "\\"+ picture.ItemFileName);

                //Delete thumbnail




                float x = image.Width;
                float y = image.Height;
                float scale = x / y;
                int newx = 120;
                int newy = Convert.ToInt32(120 / scale);
                if (newy > 120)
                {
                    newy = 120;
                    newx = Convert.ToInt32(120 * scale);
                }
                //create thumbnail
                System.Drawing.Image thumb = image.GetThumbnailImage(newx, newy, () => false, IntPtr.Zero);
                thumb.Save(PicturePath + "\\thm_" + picture.ItemFileName);



            }
            

        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                //Upload Command
                var uploadaction = new ModuleAction(ModuleContext.GetNextActionID());
                uploadaction.Title = "Upload File";
                uploadaction.CommandName = "Upload";
                uploadaction.CommandArgument = "true";
                //jsaction.ClientScript = "<script type=\"text/javascript\"> alert('Isnt this cool!');</script>";
                uploadaction.Secure = SecurityAccessLevel.Edit;
                uploadaction.Visible = GalleryConfigured;
                uploadaction.UseActionEvent = true;
                actions.Add(uploadaction);

                //Rescan Picture Directory Command
                var rescanaction = new ModuleAction(ModuleContext.GetNextActionID());
                rescanaction.Title = "Rescan Picture Directory";
                rescanaction.CommandName = "Rescan";
                rescanaction.CommandArgument = "true";
                //jsaction.ClientScript = "<script type=\"text/javascript\"> alert('Isnt this cool!');</script>";
                rescanaction.Secure = SecurityAccessLevel.Edit;
                rescanaction.Visible = GalleryConfigured;
                rescanaction.UseActionEvent = true;
                actions.Add(rescanaction);

                //Recreate Thumbnails
                var recreateaction = new ModuleAction(ModuleContext.GetNextActionID());
                recreateaction.Title = "Recreate Thumbnails";
                recreateaction.CommandName = "Recreate";
                recreateaction.CommandArgument = "true";
                //jsaction.ClientScript = "<script type=\"text/javascript\"> alert('Isnt this cool!');</script>";
                recreateaction.Secure = SecurityAccessLevel.Edit;
                recreateaction.Visible = GalleryConfigured;
                recreateaction.UseActionEvent = true;
                actions.Add(recreateaction);

                return actions;
            }
        }
        private void MyActions_Click(object sender, DotNetNuke.Entities.Modules.Actions.ActionEventArgs e)
        {
            //My event handler for the custom events added to the ModuleActionCollection
            switch (e.Action.CommandName.ToUpper())
            {
                case "UPLOAD":
                    if (e.Action.CommandArgument.ToUpper() != "CANCEL")
                    {
                        fileUpload.Visible = true;
                    }
                    else
                    {
                        fileUpload.Visible = false;
                    }
                    break;
                case "RESCAN":
                    if (e.Action.CommandArgument.ToUpper() != "CANCEL")
                    {
                        RescanDirectory();
                    }
                    break;
                case "RECREATE":
                    {
                        RecreateThumbnails();
                        break;
                    }
            }
        }

    }
}