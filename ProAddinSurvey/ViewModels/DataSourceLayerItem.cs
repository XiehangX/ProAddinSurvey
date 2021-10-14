using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ProAddinSurvey.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProAddinSurvey.ViewModels
{
    public class DataSourceLayerItem : PropertyChangedBase
    {
        private string _label;
        private string _layerName;
        private string _layerPath;

        public DataSourceLayerItem(string label, string name, string path)
        {
            _label = label;
            _layerName = name;
            _layerPath = path;
        }
        public string Label
        {
            get { return _label; }
            set
            {
                SetProperty(ref _label, value, () => Label);
            }
        }

        public string LayerPath
        {
            get { return _layerPath; }
            set
            {
                SetProperty(ref _layerPath, value, () => LayerPath);
            }
        }
        public string LayerName
        {
            get { return _layerName; }
            set
            {
                SetProperty(ref _layerName, value, () => LayerName);
            }
        }
        public ICommand BrowseLayerCommand => new RelayCommand((param) =>
        {
            IEnumerable<Item> items = FileAccessHelper.BrowsePolygonLayerInFgdb();
            if (items == null)
                return;
            foreach (Item selectedItem in items)
            {
                LayerPath = selectedItem.Path;
                LayerName = selectedItem.Name;
                break;
            }
        }, () => true);
    }
}
