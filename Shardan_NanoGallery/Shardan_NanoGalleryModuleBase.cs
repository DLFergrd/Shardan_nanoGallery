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
using DotNetNuke.Services.Localization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace Shardan.Modules.Shardan_NanoGallery
{
    public class Shardan_NanoGalleryModuleBase : PortalModuleBase
    {
        public int ItemId
        {
            get
            {
                var qs = Request.QueryString["tid"];
                if (qs != null)
                    return Convert.ToInt32(qs);
                return -1;
            }

        }
        protected string GetLocalizedString(string LocalizationKey)
        {
            if (!(string.IsNullOrEmpty(LocalizationKey)))
            {
                return Localization.GetString(LocalizationKey, this.LocalResourceFile);
            }
            else
            {
                return string.Empty;
            }
        }
        public string PicturePath
        {
            get
            {
                return Server.MapPath(Settings["BaseFolderPath"].ToString());
            }
        }
        /// <summary>
        /// Gets status of configuration.
        /// </summary>
        /// <returns>bool</returns>
        public bool GalleryConfigured
        {
            get
            {
                return DotNetNuke.Services.FileSystem.FolderManager.Instance.FolderExists(PortalId, "Gallery/Nano/" + ModuleId.ToString());
            }
        }

        public static string RemoveCharactersFromString(string testString, string removeChars)
        {
            //on picture filenames nanoGallery will replace _ with space
            string pattern = "[" + removeChars + "]";
            return Regex.Replace(testString, pattern, "_");
        }

        public string FixFileName(string filename)
        {
            //spaces and ' are not allowed
            filename = filename.Replace("'", "");
            filename = filename.Replace(" ", "_");
           




            return filename;
        }
        /// <summary>
        /// Gets the excluded files.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetExcludedFiles()
        {
            var settingValue = Settings["ExcludeFilter"];
            if (settingValue == null)
            {
                //Force default value in
                var modController = new ModuleController();
                var defaultFilter = ".cs,.vb,.template,.htmtemplate,.resources";
                modController.UpdateModuleSetting(this.ModuleId, "ExcludeFilter", defaultFilter);
                settingValue = defaultFilter;
            }
            var finalSetting = settingValue.ToString();
            return finalSetting.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Gets the sort order.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetSortOrder()
        {
            var value = Settings["SortOrder"];
            return value != null ? value.ToString() : "DA";
        }

    }
}