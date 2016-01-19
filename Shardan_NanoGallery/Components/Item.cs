/*
' Copyright (c) 2016 Shardan
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
using System.Web.Caching;
using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;

namespace Shardan.Modules.Shardan_NanoGallery.Components
{
    [TableName("Shardan_NanoGallery_Items")]
    //setup the primary key for table
    [PrimaryKey("ItemId", AutoIncrement = true)]
    //configure caching using PetaPoco
    [Cacheable("Items", CacheItemPriority.Default, 20)]
    //scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    [Scope("ModuleId")]
    class Item
    {
        ///<summary>
        /// The ID of your object with the name of the ItemName
        ///</summary>
        public int ItemId { get; set; }

        ///<summary>
        /// Kind of gallery (Album or Picture)
        /// </summary>
        public string ItemKind { get; set; }

        ///<summary>
        /// A string with the name of the Item (The actual filename of the picture)
        ///</summary>
        public string ItemFileName { get; set; }

        ///<summary>
        /// A string with the Title of the Item
        ///</summary>
        public string ItemTitle { get; set; }

        ///<summary>
        /// A string with the Title of the Item
        ///</summary>
        public string ItemDescription { get; set; }

        ///<summary>
        /// AlbumID that this item is assigned to
        /// </summary>
        public int AlbumID { get; set; }

        ///<summary>
        /// The ModuleId of where the object was created and gets displayed
        ///</summary>
        public int ModuleId { get; set; }

        ///<summary>
        /// An integer for the user id of the user who created the object
        ///</summary>
        public int CreatedByUserId { get; set; }

        ///<summary>
        /// An integer for the user id of the user who last updated the object
        ///</summary>
        public int LastModifiedByUserId { get; set; }

        ///<summary>
        /// The date the object was created
        ///</summary>
        public DateTime CreatedOnDate { get; set; }

        ///<summary>
        /// The date the object was updated
        ///</summary>
        public DateTime LastModifiedOnDate { get; set; }
    }
}
