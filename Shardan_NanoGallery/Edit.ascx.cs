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

using DotNetNuke.Entities.Users;
using Shardan.Modules.Shardan_NanoGallery.Components;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Client.ClientResourceManagement;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using DotNetNuke.Services.Localization;
using System.Collections;
using System.IO;
using DotNetNuke.UI.Utilities;

namespace Shardan.Modules.Shardan_NanoGallery
{
    /// -----------------------------------------------------------------------------
    /// <summary>   
    /// The Edit class is used to manage content
    /// 
    /// Typically your edit control would be used to create new content, or edit existing content within your module.
    /// The ControlKey for this control is "Edit", and is defined in the manifest (.dnn) file.
    /// 
    /// Because the control inherits from Shardan_NanoGalleryModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Edit : Shardan_NanoGalleryModuleBase
    {
        private string GetAlbumName(int picID)
        {
            var t = new Item();
            var tc = new ItemController();
            t = tc.GetItem(picID, ModuleId);
            return t.ItemTitle;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Add confirmation to Multiple Delete Button
                ClientAPI.AddButtonConfirm(btnDeleteSelected, Localization.GetString("ConfirmMultiDelete", LocalResourceFile));

                if (!Page.IsPostBack)
                {
                    //check if we have an ID passed in via a querystring parameter, if so, load that item to edit.
                    //ItemId is defined in the ItemModuleBase.cs file
                    if (ItemId > 0)
                    {
                        //Set multi panel view false
                        SetPanels(false);
                        LoadSingleView();

                        var tc = new ItemController();
                        //get 1 item
                        var t = tc.GetItem(ItemId, ModuleId);
                        if (t != null)
                        {
                            imgDisplayPhoto.ImageUrl = "~/" + Settings["BaseFolderPath"].ToString() + "/thm_" + t.ItemFileName;
                            txtTitle.Text = t.ItemTitle;
                            txtFileName.Text = t.ItemFileName;
                            txtDescription.Text = t.ItemDescription;

                            if (t.AlbumID != 0) ddlAlbumID.SelectedValue = t.AlbumID.ToString();
                            else ddlAlbumID.SelectedValue = "0";

                            if (t.ItemKind == "album") rblItemKind.SelectedIndex = 1;
                            else rblItemKind.SelectedIndex = 0;

                            txtThisPictureID.Text = t.ItemId.ToString();
                        }
                    }
                    else
                    {
                        //No ID so display all the pictures
                        LoadGalleryList();
                        LoadAlbumList();
                        SetPanels(true);
                    }
                }
                else
                {
                    //page is a postback.
                    //Cancel and Submit commands should come here.
                    //set panels and buttons
                    SetPanels(true);
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
        protected void LoadAlbumList()
        {
            //Fil ddl with available albums
            ddlAlbumList.Items.Add(new System.Web.UI.WebControls.ListItem("None", "0"));
            var tc = new ItemController();
            var Items = tc.GetItems(ModuleId);

            foreach (Item pc in Items)
            {
                if (pc.ItemKind == "album")
                {
                    ddlAlbumList.Items.Add(new System.Web.UI.WebControls.ListItem(pc.ItemTitle, pc.ItemId.ToString()));
                }
            }
        }
        private void LoadSingleView()
        {
            //set radio buttons
            System.Web.UI.WebControls.ListItem rb1 = new System.Web.UI.WebControls.ListItem("Picture", "");
            System.Web.UI.WebControls.ListItem rb2 = new System.Web.UI.WebControls.ListItem("Album", "album");

            rblItemKind.Items.Add(rb1);
            rblItemKind.Items.Add(rb2);

            //Fil ddl with available albums
            ddlAlbumID.Items.Add(new System.Web.UI.WebControls.ListItem("None", "0"));
            var tc = new ItemController();
            var Items = tc.GetItems(ModuleId);

            foreach (Item pc in Items)
            {
                if (pc.ItemKind == "album")
                {
                    ddlAlbumID.Items.Add(new System.Web.UI.WebControls.ListItem(pc.ItemTitle, pc.ItemId.ToString()));
                }
            }
        }
        private void LoadGalleryList()
        {

            //Load all pictures into a listview for editing
            var tc = new ItemController();
            var t = tc.GetItems(ModuleId);
            lvGalleryList.DataSource = t;
            //set lines per page
            if (Settings.Contains("EditLinesPerPage")) dbPictureList.PageSize = Convert.ToInt32(Settings["EditLinesPerPage"].ToString());
            lvGalleryList.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var t = new Item();
            var tc = new ItemController();

            if (ItemId > 0)
            {
                t = tc.GetItem(ItemId, ModuleId);
                t.ItemTitle = txtTitle.Text.Trim();
                t.ItemFileName = txtFileName.Text.Trim();
                t.ItemDescription = txtDescription.Text.Trim();
                t.ItemKind = rblItemKind.SelectedValue;

                if (ddlAlbumID.SelectedValue != "0") t.AlbumID = Convert.ToInt32(ddlAlbumID.SelectedValue);
                else t.AlbumID = 0;

                t.LastModifiedByUserId = UserId;
                t.LastModifiedOnDate = DateTime.Now;
            }
            else
            {
                //make sure there are no ' in title or description
                string newTitle = txtTitle.Text.Trim();
                string newDescription = txtDescription.Text.Trim();
                newTitle = newTitle.Replace("'", "");
                newDescription = newDescription.Replace("'", "");

                t = new Item()
                {
                    CreatedByUserId = UserId,
                    CreatedOnDate = DateTime.Now,

                    ItemTitle = newTitle,
                    ItemFileName = txtFileName.Text.Trim(),
                    ItemDescription = newDescription,
                    ItemKind = "",
                    AlbumID = 0
                };
            }
            t.LastModifiedOnDate = DateTime.Now;
            t.LastModifiedByUserId = UserId;
            t.ModuleId = ModuleId;
            if (t.ItemId > 0)
            {
                tc.UpdateItem(t);
            }
            else
            {
                tc.CreateItem(t);
            }
            //reload
            LoadGalleryList();
            LoadAlbumList();
            SetPanels(true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Do Nothing The page postback sets the panels
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }

        protected void lvGalleryList_ItemDataBound(object sender, System.Web.UI.WebControls.ListViewItemEventArgs e)
        {
            var editLinkImage = e.Item.FindControl("editLinkImage") as System.Web.UI.WebControls.Image;
            var lnkEditPicture = e.Item.FindControl("lnkEditPicture") as System.Web.UI.WebControls.HyperLink;
            var imgPhoto = e.Item.FindControl("imgPhoto") as System.Web.UI.WebControls.Image;
            var lblItemName = e.Item.FindControl("lblItemName") as System.Web.UI.WebControls.Label;
            var lblPictureName = e.Item.FindControl("lblPictureName") as System.Web.UI.WebControls.Label;
            var lblItemDescription = e.Item.FindControl("lblItemDescription") as System.Web.UI.WebControls.Label;
            var lblPictureDescription = e.Item.FindControl("lblPictureDescription") as System.Web.UI.WebControls.Label;
            var lblType = e.Item.FindControl("lblType") as System.Web.UI.WebControls.Label;
            var lblItemType = e.Item.FindControl("lblItemType") as System.Web.UI.WebControls.Label;
            var lblThisAlbumID = e.Item.FindControl("lblThisAlbumID") as System.Web.UI.WebControls.Label;
            var lblThisItemAlbumID = e.Item.FindControl("lblThisItemAlbumID") as System.Web.UI.WebControls.Label;
            var lblTIID = e.Item.FindControl("lblTIID") as System.Web.UI.WebControls.Label;
            var lblThisItemID = e.Item.FindControl("lblThisItemID") as System.Web.UI.WebControls.Label;
            var lnkPictureDelete = e.Item.FindControl("lnkPictureDelete") as System.Web.UI.WebControls.ImageButton;
            var chkSelectPicture = e.Item.FindControl("chkSelectPicture") as System.Web.UI.WebControls.CheckBox;
            //Get the current picture
            var curPhoto = (Item)e.Item.DataItem;

            imgPhoto.ImageUrl = "~/" + Settings["BaseFolderPath"].ToString() + "/thm_" + curPhoto.ItemFileName;
            lblPictureName.Text = Localization.GetString("lblPictureName", LocalResourceFile);
            lblItemName.Text = curPhoto.ItemTitle;
            lblPictureDescription.Text = Localization.GetString("lblPictureDescription", LocalResourceFile);
            lblItemDescription.Text = curPhoto.ItemDescription;

            lblType.Text = Localization.GetString("lblType", LocalResourceFile);
            if (curPhoto.ItemKind == "album")
            {
                lblItemType.Text = "Album";
            }
            else
            {
                lblItemType.Text = "Picture";
            }

            lblThisAlbumID.Text = Localization.GetString("lblThisAlbumID", LocalResourceFile);
            if (curPhoto.AlbumID != 0)
            {
                lblThisItemAlbumID.Text = GetAlbumName(curPhoto.AlbumID);//curPhoto.AlbumID.ToString();

            }
            else
            {
                lblThisItemAlbumID.Text = "";
            }
            lblTIID.Text = Localization.GetString("lblTIID", LocalResourceFile);
            lblThisItemID.Text = curPhoto.ItemId.ToString();
            chkSelectPicture.ToolTip = Localization.GetString("SelectThisPicture", LocalResourceFile);
            chkSelectPicture.Checked = false;

            //set links
            lnkEditPicture.NavigateUrl = EditUrl(string.Empty, string.Empty, "Edit", "tid=" + curPhoto.ItemId);
            lnkEditPicture.Visible = lnkEditPicture.Enabled = true;
            editLinkImage.Visible = true;
            editLinkImage.ToolTip = Localization.GetString("EditTip", LocalResourceFile);
            lnkPictureDelete.CommandArgument = curPhoto.ItemId.ToString();
            lnkPictureDelete.ToolTip = Localization.GetString("DeleteThisPicture", LocalResourceFile);
            ClientAPI.AddButtonConfirm(lnkPictureDelete, Localization.GetString("ConfirmDelete", LocalResourceFile));
        }

        protected void lvGalleryList_ItemCommand(object sender, System.Web.UI.WebControls.ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var tc = new ItemController();
                Item t = tc.GetItem(Convert.ToInt32(e.CommandArgument), ModuleId);
                //Delete from file system
                DeletePicThumb(t);

                //Delete from Database
                tc.DeleteItem(Convert.ToInt32(e.CommandArgument), ModuleId);

                //reload listview
                LoadGalleryList();
                SetPanels(true);
            }
        }
        private void DeletePicThumb(Item t)
        {
            string basePath = Server.MapPath(Settings["BaseFolderPath"].ToString());

            //Delete picture and thumbnail from directory
            File.Delete(basePath + "\\" + t.ItemFileName);
            File.Delete(basePath + "\\thm_" + t.ItemFileName);
        }
        protected void btnAddToAlbum_Click(object sender, EventArgs e)
        {
            if (ddlAlbumList.SelectedValue != "0")
            {
                List<string> addlist = new List<string>();
                GetCheckedList(addlist);

                //update list in db
                if (addlist.Count > 0)
                {
                    var tc = new ItemController();
                    foreach (string id in addlist)
                    {
                        var t = new Item();
                        t = tc.GetItem(Convert.ToInt32(id), ModuleId);
                        //make the AlbumID = what is in the ddList
                        t.AlbumID = Convert.ToInt32(ddlAlbumList.SelectedValue);
                        tc.UpdateItem(t);
                    }
                }
            }
        }
        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {

            List<string> deletelist = new List<string>();
            GetCheckedList(deletelist);

            if (deletelist.Count > 0)
            {
                var tc = new ItemController();
                foreach (string id in deletelist)
                {
                    //Delete File and thumnail
                    Item t = tc.GetItem(Convert.ToInt32(id), ModuleId);
                    //Delete from filesystem
                    DeletePicThumb(t);
                    
                    //Delete from db
                    tc.DeleteItem(Convert.ToInt32(id), ModuleId);
                }
            }
        }
        private List<string> GetCheckedList(List<string> checklist)
            {
            //Check each picture checkbox
            for (int i = 0; i < lvGalleryList.Items.Count; i++)
            {
                System.Web.UI.WebControls.CheckBox chb = lvGalleryList.Items[i].FindControl("chkSelectPicture") as System.Web.UI.WebControls.CheckBox;
                if (chb != null)
                {
                    if (chb.Checked)
                    {
                        //get picture id number of this checked picture
                        System.Web.UI.WebControls.Label pictureID = lvGalleryList.Items[i].FindControl("lblThisItemID") as System.Web.UI.WebControls.Label;
                        if (pictureID != null) checklist.Add(pictureID.Text);   //add to list of picture ids
                    }
                }
            }
            return checklist;




        }
        private void SetPanels(bool Multi)
        {

            if (Multi)
            {
                //multi panel = true
                pnlMulti.Visible = true;
                pnlSingle.Visible = false;
                btnSubmit.Visible = false;
                btnCancel.Visible = false;
                btnDone.Visible = true;
            }
            else
            {
                //multi panel = false
                pnlMulti.Visible = false;
                pnlSingle.Visible = true;
                btnSubmit.Visible = true;
                btnCancel.Visible = true;
                btnDone.Visible = false;
            }
        }
        protected void dbPictureList_PreRender(object sender, EventArgs e)
        {
            //this is required for paging to work properly
            LoadGalleryList();
        }

        protected void lvGalleryList_ItemDeleting(object sender, System.Web.UI.WebControls.ListViewDeleteEventArgs e)
        {
            //has to be here for delete to work
        }

        
    }
}