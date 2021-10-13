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

        private string _layerPath;
        /// <summary>
        /// 监测图斑图层
        /// </summary>
        public string LayerPath
        {
            get { return _layerPath; }
            set { SetProperty(ref _layerPath, value, () => LayerPath); }
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
        /// 选择图层
        /// </summary>
        public ICommand BrowseLayerCommand => new RelayCommand((param) =>
        {
            IEnumerable<Item> items = FileAccessHelper.BrowsePolygonLayerInFgdb();
            if (items == null)
                return;
            foreach (Item selectedItem in items)
            {
                LayerPath = selectedItem.Path;
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
                break;
            }
        }, () => true);

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
            SurveyLayer = LayerFactory.Instance.CreateFeatureLayer(new Uri(LayerPath), MapView.Active.Map);
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

            if (!Directory.Exists(TargetPath))
            {
                MessageBox.Show($@"{TargetPath} 目标GDB不存在或无法访问");
                return false;
            }

            return true;
        }

        private async void SaveChanges()
        {
            ClearMessage(); ;
            await QueuedTask.Run(async () =>
            {
                if (!CheckParams())
                    return;

                try
                {
                    int rowHeader = 3;
                    using (DataTable dt = ExcelHelper.LoadTableFirst(FilePath, rowHeader))
                    {
                        List<AttributeTableEntity> list = ExcelHelper.DataTableToList<AttributeTableEntity>(dt);

                        foreach (AttributeTableEntity field in list)
                        {
                            //List<object> arguments = field.ToAddFieldDesc();
                            //await GPToolHelper.ExecuteAddFieldToolAsync(SurveyLayer, arguments);
                        }
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                    Message += $"执行异常。{exp.Message} \n";
                }
            });
        }

        private bool CanSaveChanges() => !string.IsNullOrEmpty(LayerPath) && !string.IsNullOrEmpty(FilePath) && !string.IsNullOrEmpty(TargetPath);

    }
}
