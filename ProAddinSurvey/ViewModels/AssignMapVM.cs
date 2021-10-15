using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using ProAddinSurvey.Common;
using ProAddinSurvey.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProAddinSurvey.ViewModels
{
    /// <summary>
    /// 监测图斑赋值
    /// </summary>
    public class AssignMapVM : PropertyChangedBase
    {
        private ObservableCollection<DataSourceLayerItem> _dataSourceLayers = new ObservableCollection<DataSourceLayerItem>();
        /// <summary>
        /// 数据源图层集合
        /// </summary>
        public ObservableCollection<DataSourceLayerItem> DataSourceLayers
        {
            get { return _dataSourceLayers; }
        }

        private AttributeFileItem _selectedDataSourceLayerItem;
        /// <summary>
        /// 选中的属性表文件项
        /// </summary>
        public AttributeFileItem SelectedDataSourceLayerItem
        {

            get { return _selectedDataSourceLayerItem; }
            set
            {
                SetProperty(ref _selectedDataSourceLayerItem, value, () => SelectedDataSourceLayerItem);
            }
        }

        private ObservableCollection<AttributeTableEntity> _attributeFields = new ObservableCollection<AttributeTableEntity>();
        /// <summary>
        /// 属性字段集合
        /// </summary>
        public ObservableCollection<AttributeTableEntity> AttributeFields
        {
            get { return _attributeFields; }
            set
            {
                SetProperty(ref _attributeFields, value, () => AttributeFields);
            }
        }

        private string _layerPath;
        /// <summary>
        /// 监测图斑图层
        /// </summary>
        public string LayerPath
        {
            get { return _layerPath; }
            set
            {
                SetProperty(ref _layerPath, value, () => LayerPath);
                //NotifyPropertyChanged(nameof(LayerPath));
            }
        }
        private string _layerName;
        /// <summary>
        /// 监测图斑图层
        /// </summary>
        public string LayerName
        {
            get { return _layerName; }
            set
            {
                SetProperty(ref _layerName, value, () => LayerName);
                //NotifyPropertyChanged(nameof(LayerPath));
            }
        }
        private string _targetPath;
        /// <summary>
        /// 输出GDB路径
        /// </summary>
        public string TargetPath
        {
            get { return _targetPath; }
            set { SetProperty(ref _targetPath, value, () => TargetPath); }
        }
        private string _filePath;
        /// <summary>
        /// 属性表文件路径
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value, () => FilePath); }
        }
        /// <summary>
        /// 选择监测图层
        /// </summary>
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
        /// <summary>
        /// 选择输出GDB
        /// </summary>
        public ICommand BrowseGdbCommand => new RelayCommand((param) =>
        {
            IEnumerable<Item> items = FileAccessHelper.BrowseFgdb();
            if (items == null)
                return;
            foreach (Item selectedItem in items)
            {
                TargetPath = selectedItem.Path;
                break;
            }
        }, () => true);
        /// <summary>
        /// 选择属性文件
        /// </summary>
        public ICommand BrowseFileCommand => new RelayCommand((param) =>
        {
            IEnumerable<Item> items = FileAccessHelper.BrowseExcel();
            if (items == null)
                return;
            foreach (Item selectedItem in items)
            {
                FilePath = selectedItem.Path;
                InitializeDataSourceUI(FilePath);
                break;
            }
        }, () => true);


        private void InitializeDataSourceUI(string filePath)
        {
            ClearMessage();
            try
            {
                int rowHeader = 3;
                DataSourceLayers.Clear();
                using (DataTable dt = ExcelHelper.LoadTableFirst(filePath, rowHeader))
                {
                    AttributeFields = new ObservableCollection<AttributeTableEntity>(ExcelHelper.DataTableToList<AttributeTableEntity>(dt));

                    foreach (AttributeTableEntity field in AttributeFields)
                    {

                        if (string.IsNullOrEmpty(field.模式) || string.IsNullOrEmpty(field.数据来源) || field.模式 != "A")
                            continue;

                        if (DataSourceLayers.FirstOrDefault(i => i.Label == field.数据来源) == null)
                            DataSourceLayers.Add(new DataSourceLayerItem(field.数据来源, string.Empty, string.Empty));
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
                Message += $"执行异常。{exp.Message} \n";
            }
            //ShowMessage(message);
        }

        #region Message
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                SetProperty(ref _message, value);
                NotifyPropertyChanged(nameof(HasMessage));
            }
        }

        public bool HasMessage => !string.IsNullOrEmpty(_message);

        public void ShowMessage(string msg)
        {
            Message = msg;
        }

        public void ClearMessage()
        {
            Message = "";
        }
        #endregion

        private RelayCommand _applyCommand;
        /// <summary>
        /// 确定执行
        /// </summary>
        public ICommand ApplyCommand
        {
            get
            {
                if (_applyCommand == null)
                    _applyCommand = new RelayCommand(() => SaveChanges(), CanSaveChanges);

                return _applyCommand;
            }
        }

        /// <summary>
        /// 监测图斑图层
        /// </summary>
        private FeatureLayer SurveyLayer { get; set; } = null;

        private bool CheckParams()
        {
            var layer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().Where((l) => l.Name == LayerName).FirstOrDefault();
            if (layer == null || layer != SurveyLayer)
            {
                SurveyLayer = LayerFactory.Instance.CreateFeatureLayer(new Uri(LayerPath), MapView.Active.Map);
            }

            if (SurveyLayer == null)
            {
                MessageBox.Show($@"{LayerPath} 图层不存在或无法访问");
                return false;
            }

            if (!File.Exists(FilePath))
            {
                MessageBox.Show($@"{FilePath} 属性表文件不存在或无法访问");
                return false;
            }

            //if (!Directory.Exists(TargetPath))
            //{
            //    MessageBox.Show($@"{TargetPath} 目标GDB不存在或无法访问");
            //    return false;
            //}

            //DataSourceLayerItem item = DataSourceLayers.FirstOrDefault(i => string.IsNullOrEmpty(i.LayerPath));
            //if (item != null)
            //{
            //    MessageBox.Show($@"{item.Label} 的数据源未设置。");
            //    return false;
            //}

            return true;
        }


        private bool CanSaveChanges() => !string.IsNullOrEmpty(LayerPath) && !string.IsNullOrEmpty(FilePath); // && !string.IsNullOrEmpty(TargetPath)

        private async void SaveChanges()
        {
            ClearMessage(); ;
            await QueuedTask.Run(async () =>
            {
                if (!CheckParams())
                    return;

                MessageBox.Show("开发中");
                try
                {
                    //var groups = AttributeFields.Where(i => i.模式 == "A").GroupBy(i => i.数据来源);

                    //foreach (var group in groups)
                    //{
                    //    string datasourceLabel = group.Key;
                    //    List<AttributeTableEntity> fieldList = group.ToList();
                    //    //List<object> arguments = field.ToAddFieldDesc();
                    //    //await GPToolHelper.ExecuteAddFieldToolAsync(SurveyLayer, arguments);
                    //}
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                    Message += $"执行异常。{exp.Message} \n";
                }
            });
        }

        //private FeatureLayer GetDataSourceLayer(string label)
        //{
        //    DataSourceLayerItem layerItem = DataSourceLayers.FirstOrDefault(i => i.Label == label && !string.IsNullOrEmpty(i.LayerPath ));
        //    if (layerItem == null)
        //        return null;

        //    var layer = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().Where((l) => l.Name == layerItem.LayerName).FirstOrDefault();
        //    if (layer == null)
        //    {
        //        layer = LayerFactory.Instance.CreateFeatureLayer(new Uri(layerItem.LayerPath), MapView.Active.Map);
        //    }
        //    return layer;
        //}

        //private void UpdateFields()
        //{ 
            
        //}

    }
}
