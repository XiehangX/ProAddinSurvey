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
    internal class BrowserButton : Button
    {
        
        protected async override void OnClick()
        {
            OpenItemDialog pathDialog = new OpenItemDialog()
            {
                Title = "选择图层",
                MultiSelect = false,
                Filter = ItemFilters.composite_addToMap
            };
            bool? ok = pathDialog.ShowDialog();

            if (ok == true)
            {
                IEnumerable<Item> selectedItems = pathDialog.Items;
                foreach (Item selectedItem in selectedItems)
                {
                    MessageBox.Show(selectedItem.Path);
                    Module1.url = selectedItem.Path.ToString();
                }
            }
            Module1.mapPreorder = MapView.Active.Map;

            Module1.lyr = await AddLayer(Module1.url, Module1.mapPreorder);
            Module1.flyr = await GetFeatureLayer(Module1.lyr);

            //this.Topmost = true;
        }

        public Task<Layer> AddLayer(string uri, Map map)
        {
            return QueuedTask.Run(() =>
            {
                return LayerFactory.Instance.CreateLayer(new Uri(uri), map);
            });
        }

        public Task<FeatureLayer> GetFeatureLayer(Layer lyr)
        {
            return QueuedTask.Run(() =>
            {
                if (lyr is ServiceLayer) return null;
                if (lyr is ILayerContainer)
                    return ((ILayerContainer)lyr).GetLayersAsFlattenedList()[0] as FeatureLayer;
                return lyr as FeatureLayer;
            });
        }

    }
}
