using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProAddinSurvey.Common
{
    public class FileAccessHelper
    {

        public static IEnumerable<Item> BrowsePolygonLayerInFgdb()
        {
            BrowseProjectFilter bf = new BrowseProjectFilter
            {
                Name = "面要素图层"
            };
            bf.AddCanBeTypeId("fgdb_fc_polygon");
            bf.AddDontBrowseIntoFlag(BrowseProjectFilter.FilterFlag.DontBrowseFiles);
            bf.AddDoBrowseIntoTypeId("database_fgdb");
            bf.Includes.Add("FolderConnection");
            bf.Includes.Add("GDB");
            bf.Excludes.Add("esri_browsePlaces_Online");

            OpenItemDialog dlg = new OpenItemDialog()
            {
                Title = "选择文件型地理数据库中的面要素图层",
                MultiSelect = false,
                BrowseFilter = bf
            };
            bool? ok = dlg.ShowDialog();

            if (ok == true)
            {
                return dlg.Items;
            }
            return null;
        }

        public static IEnumerable<Item> BrowseFgdb()
        {
            BrowseProjectFilter bf = new BrowseProjectFilter("esri_browseDialogFilters_geodatabases");
            OpenItemDialog dlg = new OpenItemDialog
            {
                Title = "选择文件型地理数据库",
                MultiSelect = false,
                BrowseFilter = bf
            };
            bool? ok = dlg.ShowDialog();

            if (ok == true)
            {
                return dlg.Items;
            }
            return null;
        }

        public static IEnumerable<Item> BrowseExcel()
        {

            var bpf = new BrowseProjectFilter("esri_browseDialogFilters_browseFiles")
            {
                FileExtension = "*.xls;*.xlsx;",//*.csv;
                BrowsingFilesMode = true,
                Name = "属性表模板 (*.xls;*.xlsx;)" //*.csv;
            };

            OpenItemDialog dlg = new OpenItemDialog()
            {
                Title = "选择属性表文件",
                MultiSelect = true,
                BrowseFilter = bpf
            };
            bool? ok = dlg.ShowDialog();

            if (ok == true)
            {
                return dlg.Items;
            }
            return null;
        }
    }
}
