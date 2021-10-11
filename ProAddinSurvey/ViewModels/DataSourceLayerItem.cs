using ArcGIS.Desktop.Framework.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string FilePath
        {
            get { return _layerPath; }
            set
            {
                SetProperty(ref _layerPath, value, () => FilePath);
            }
        }
        public string FileName
        {
            get { return _layerName; }
            set
            {
                SetProperty(ref _layerName, value, () => FileName);
            }
        }
    }
}
