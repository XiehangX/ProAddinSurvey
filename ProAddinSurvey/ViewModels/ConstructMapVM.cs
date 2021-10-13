using ArcGIS.Core.Data;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using Microsoft.Win32;
using ProAddinSurvey.Common;
using ProAddinSurvey.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProAddinSurvey.ViewModels
{
    /// <summary>
    /// 图斑构建
    /// </summary>
    public class ConstructMapVM : PropertyChangedBase
    {
        private ObservableCollection<AttributeFileItem> _attributeFiles = new ObservableCollection<AttributeFileItem>();
        /// <summary>
        /// 属性表文件集合
        /// </summary>
        public ObservableCollection<AttributeFileItem> AttributeFiles
        {
            get { return _attributeFiles; }

        }

        private AttributeFileItem _selectedAttributeFileItem;
        /// <summary>
        /// 选中的属性表文件项
        /// </summary>
        public AttributeFileItem SelectedAttributeFileItem
        {

            get { return _selectedAttributeFileItem; }
            set
            {
                SetProperty(ref _selectedAttributeFileItem, value, () => SelectedAttributeFileItem);
            }
        }


        private ICommand _selectAttributeFileCommand;
        /// <summary>
        /// 选中属性表文件命令
        /// </summary>
        public ICommand SelectAttributeFileCommand
        {
            get
            {
                _selectAttributeFileCommand = new RelayCommand(() => SelectAttributeFileX());
                return _selectAttributeFileCommand;
            }
        }


        private RelayCommand _applyCommand;
        /// <summary>
        /// 确定
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


        private string _itemPath;
        public string ItemPath
        {
            get { return _itemPath; }
            set { SetProperty(ref _itemPath, value, () => ItemPath); }
        }

        private RelayCommand _browseCommand;
        public ICommand BrowseCommand
        {
            get
            {
                if (_browseCommand == null)
                    _browseCommand = new RelayCommand(BrowseImpl, () => true);
                return _browseCommand;
            }
        }

        private void BrowseImpl(object param)
        {
            if (param == null)
                return;

            IEnumerable<Item> items = FileAccessHelper.BrowsePolygonLayerInFgdb();
            if (items == null)
                return;
            foreach (Item selectedItem in items)
            {
                switch (param.ToString())
                {
                    case "ItemPath":
                        ItemPath = selectedItem.Path;
                        break;
                }
            }
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

        private async void SaveChanges()
        {
            //if (!File.Exists(ItemPath))
            //    throw new Exception($@"{ItemPath} 图层不存在或无法访问");
            await QueuedTask.Run(async () =>
            {
                ClearMessage();
                var layerParams = new FeatureLayerCreationParams(new Uri(ItemPath));
                var layer = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, MapView.Active.Map);
                //var layer = LayerFactory.Instance.CreateFeatureLayer(new Uri(ItemPath), MapView.Active.Map);
                if (layer == null)
                {
                    MessageBox.Show($@"{ItemPath} 图层不存在或无法访问");
                    return;
                }
                else
                {
                    //var dataSource = await GPToolHelper.GetDataSource(layer);
                    //Message += $@"{dataSource} 图层正常访问";

                    foreach (AttributeFileItem item in _attributeFiles)
                    {
                        Message += $"解析属性表文件 {item.FileName} \n";
                        if (!File.Exists(item.FilePath))
                        {
                            Message += $"{item.FileName} 文件不存在\n";
                            continue;
                        }

                        try
                        {
                            int rowHeader = 3;
                            using (DataTable dt = ExcelHelper.LoadTableFirst(item.FilePath, rowHeader))
                            {
                                List<AttributeTableEntity> list = ExcelHelper.DataTableToList<AttributeTableEntity>(dt);

                                string newLayerName = $"{layer.Name}_{Path.GetFileNameWithoutExtension(item.FileName)}".Trim();
                                //string newLayerPath = await GPToolHelper.ExecuteCopyToolAsync(layer, newLayerName); 
                                string newLayerPath = await GPToolHelper.ExecuteFeatureClassToFeatureClassToolAsync(layer, newLayerName);
                                var newLayer = LayerFactory.Instance.CreateFeatureLayer(new Uri(newLayerPath), MapView.Active.Map);
                                if (newLayer == null)
                                {
                                    Message += $"{newLayerName} 图层创建失败\n";
                                    continue;
                                }
                                Message += $"已解析待添加属性字段 {list.Count} 个\n";
                                //foreach (AttributeTableEntity field in list)
                                //{
                                //    List<object> arguments = field.ToAddFieldDesc();
                                //    await GPToolHelper.ExecuteAddFieldToolAsync(newLayer, arguments);
                                //}
                                List<object> fieldArgumentsList = new List<object>();
                                foreach (AttributeTableEntity field in list)
                                {
                                    string arguments = field.ToAddFieldsDesc();
                                    fieldArgumentsList.Add(arguments);
                                }
                                await GPToolHelper.ExecuteAddFieldsToolAsync(newLayer, fieldArgumentsList);
                                Message += $"{newLayerName} 图层属性字段批量添加完成\n";
                            }
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show(exp.Message);
                            Message += $"执行异常。{exp.Message} \n";
                        }
                    }
                }
            });
        }

        private bool CanSaveChanges() => !string.IsNullOrEmpty(ItemPath) && _attributeFiles.Count > 0;

        private void SelectAttributeFileX()
        {

            ClearMessage();

            IEnumerable<Item> items = FileAccessHelper.BrowseExcel();
            if (items == null)
                return;

            string message = string.Empty;

            foreach (Item item in items)
            {
                if (_attributeFiles.FirstOrDefault(i => i.FilePath == item.Path) == null)
                    _attributeFiles.Add(new AttributeFileItem(item));
                else
                    message += $"{item.Name} 已存在;\n";
            }
            ShowMessage(message);
        }

    }
}
