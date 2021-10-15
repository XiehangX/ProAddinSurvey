using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProAddinSurvey.UI
{
    internal class OutputFolderButton : Button
    {
        protected override void OnClick()
        {
            if (Module1.isClickedFwBw == false) return;

            OpenItemDialog pathDialog = new OpenItemDialog()
            {
                Title = "选择导出文件夹",
                MultiSelect = false,
                Filter = ItemFilters.folders
            };
            bool? ok = pathDialog.ShowDialog();
            if (ok == true)
            {
                IEnumerable<Item> selectedItems = pathDialog.Items;
                foreach(Item selectedItem in selectedItems)
                {
                    Module1.outputFolder = selectedItem.Path;
                    MessageBox.Show(Module1.outputFolder);
                }
            }
        }
    }
}
