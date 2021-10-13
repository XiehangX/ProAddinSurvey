using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ProAddinSurvey.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProAddinSurvey.ViewModels
{
    public class AssignMapVM : PropertyChangedBase
    {
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


        private string _layerPath;
        public string LayerPath
        {
            get { return _layerPath; }
            set { SetProperty(ref _layerPath, value, () => LayerPath); }
        }
        private string _targetPath;
        public string TargetPath
        {
            get { return _targetPath; }
            set { SetProperty(ref _targetPath, value, () => TargetPath); }
        }
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value, () => FilePath); }
        }

        private RelayCommand _browseLayerCommand;
        public ICommand BrowseLayerCommand
        {
            get
            {
                if (_browseLayerCommand == null)
                    _browseLayerCommand = new RelayCommand(BrowseLayerImpl, () => true);
                return _browseLayerCommand;
            }
        }

        private void BrowseLayerImpl(object param)
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
                    case "LayerPath":
                        LayerPath = selectedItem.Path;
                        break;
                }
            }
        }

        private RelayCommand _browseGdbCommand;
        public ICommand BrowseGdbCommand
        {
            get
            {
                if (_browseGdbCommand == null)
                    _browseGdbCommand = new RelayCommand(BrowseGdbImpl, () => true);
                return _browseGdbCommand;
            }
        }
        private void BrowseGdbImpl(object param)
        {
            if (param == null)
                return;

            IEnumerable<Item> items = FileAccessHelper.BrowseFgdb();
            if (items == null)
                return;
            foreach (Item selectedItem in items)
            {
                switch (param.ToString())
                {
                    case "TargetPath":
                        TargetPath = selectedItem.Path;
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
                //var layerParams = new FeatureLayerCreationParams(new Uri(ItemPath));
                //var layer = LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, MapView.Active.Map);
                ////var layer = LayerFactory.Instance.CreateFeatureLayer(new Uri(ItemPath), MapView.Active.Map);
                //if (layer == null)
                //{
                //    MessageBox.Show($@"{ItemPath} 图层不存在或无法访问");
                //    return;
                //}
                //else
                //{
                //    //var dataSource = await GPToolHelper.GetDataSource(layer);
                //    //Message += $@"{dataSource} 图层正常访问";

                //    foreach (AttributeFileItem item in _attributeFiles)
                //    {
                //        Message += $"解析属性表文件 {item.FileName} \n";
                //        if (!File.Exists(item.FilePath))
                //        {
                //            Message += $"{item.FileName} 文件不存在\n";
                //            continue;
                //        }

                //        try
                //        {
                //            int rowHeader = 3;
                //            using (DataTable dt = ExcelHelper.LoadTableFirst(item.FilePath, rowHeader))
                //            {
                //                List<AttributeTableEntity> list = ExcelHelper.DataTableToList<AttributeTableEntity>(dt);

                //                string newLayerName = $"{layer.Name}_{Path.GetFileNameWithoutExtension(item.FileName)}".Trim();
                //                //string newLayerPath = await GPToolHelper.ExecuteCopyToolAsync(layer, newLayerName); 
                //                string newLayerPath = await GPToolHelper.ExecuteFeatureClassToFeatureClassToolAsync(layer, newLayerName);
                //                var newLayer = LayerFactory.Instance.CreateFeatureLayer(new Uri(newLayerPath), MapView.Active.Map);
                //                if (newLayer == null)
                //                {
                //                    Message += $"{newLayerName} 图层创建失败\n";
                //                    continue;
                //                }
                //                Message += $"已解析待添加属性字段 {list.Count} 个\n";
                //                //foreach (AttributeTableEntity field in list)
                //                //{
                //                //    List<object> arguments = field.ToAddFieldDesc();
                //                //    await GPToolHelper.ExecuteAddFieldToolAsync(newLayer, arguments);
                //                //}
                //                List<object> fieldArgumentsList = new List<object>();
                //                foreach (AttributeTableEntity field in list)
                //                {
                //                    string arguments = field.ToAddFieldsDesc();
                //                    fieldArgumentsList.Add(arguments);
                //                }
                //                await GPToolHelper.ExecuteAddFieldsToolAsync(newLayer, fieldArgumentsList);
                //                Message += $"{newLayerName} 图层属性字段批量添加完成\n";
                //            }
                //        }
                //        catch (Exception exp)
                //        {
                //            MessageBox.Show(exp.Message);
                //            Message += $"执行异常。{exp.Message} \n";
                //        }
                //    }
                //}
            });
        }

        private bool CanSaveChanges() => !string.IsNullOrEmpty(LayerPath) && !string.IsNullOrEmpty(LayerPath) && !string.IsNullOrEmpty(LayerPath);

        private void SelectAttributeFileX()
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
                //InitialLocation = @"C:\Data\",
                MultiSelect = true,
                BrowseFilter = bpf
            };
            bool? ok = dlg.ShowDialog();

            if (ok == true)
            {
                ClearMessage();
                string message = string.Empty;

                IEnumerable<Item> selectedItems = dlg.Items;
                foreach (Item item in selectedItems)
                {
                    //if (_attributeFiles.FirstOrDefault(i => i.FilePath == item.Path) == null)
                    //    _attributeFiles.Add(new AttributeFileItem(item));
                    //else
                    //    message += $"{item.Name} 已存在;\n";
                }
                ShowMessage(message);
            }
        }
    }
}
