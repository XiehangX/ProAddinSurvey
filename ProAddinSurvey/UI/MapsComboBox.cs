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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProAddinSurvey.UI
{
    /// <summary>
    /// Represents the ComboBox
    /// </summary>
    internal class MapsComboBox : ComboBox
    {

        private bool _isInitialized;

        /// <summary>
        /// Combo Box constructor
        /// </summary>
        public MapsComboBox()
        {
            _ = UpdateComboAsync();
        }

        /// <summary>
        /// Updates the combo box with all the items.
        /// </summary>

        private async Task UpdateComboAsync()
        {
            if (_isInitialized)
                SelectedItem = ItemCollection.FirstOrDefault(); 
            
            if (!_isInitialized)
            {
                Clear();

                List<Map> maps = await GetMaps();
                foreach (Map map in maps)
                {
                    Add(new ComboBoxItem(map.Name));
                }
                //_isInitialized = true;
            }

            Enabled = true; 
            SelectedItem = ItemCollection.FirstOrDefault(); 

        }

        private async Task<List<Map>> GetMaps()
        {
            var projectMapItems = Project.Current.GetItems<MapProjectItem>();
            if (projectMapItems == null)
                return null;

            var projectMaps = await QueuedTask.Run(() => { List<Map> maps = new List<Map>(); foreach (var item in projectMapItems) { maps.Add(item.GetMap()); } return maps; });

            return projectMaps;
        }

        /// <summary>
        /// The on comboBox selection change event. 
        /// </summary>
        /// <param name="item">The newly selected combo box item</param>
        protected override void OnSelectionChange(ComboBoxItem item)
        {

            if (item == null)
                return;

            if (string.IsNullOrEmpty(item.Text))
                return;

        }

    }
}
