using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
                _selectAttributeFileCommand = new RelayCommand(() => SelectAttributeFile());
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

        private void SaveChanges()
        {
            MessageBox.Show("SaveChanges", "SaveChanges", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

            foreach (AttributeFileItem item in _attributeFiles)
            { 
                
            }
        }
        private bool CanSaveChanges() => _attributeFiles.Count > 0;

        private void SelectAttributeFile()
        {
            //OpenItemDialog pathDialog = new OpenItemDialog()
            //{
            //    Title = "选择属性表文件",
            //    //InitialLocation = @"C:\Data\",
            //    MultiSelect = true,
            //    //Filter = ItemFilters.files_all
            //    Filter = "Csv (.csv)|*.csv|Excel (.xls)|*.xls|Excel (.xlsx)|*.xlsx|All files (*.*)|*.*"
            //};
            //bool? ok = pathDialog.ShowDialog();

            //if (ok == true)
            //{
            //    IEnumerable<Item> selectedItems = pathDialog.Items;
            //    foreach (Item selectedItem in selectedItems)
            //    {
            //        //MessageBox.Show(selectedItem.Path);
            //        _attributeFiles.Add(new AttributeFileItem(selectedItem));
            //    }
            //}

            OpenFileDialog dlg = new OpenFileDialog()
            {
                Title = "选择属性表文件",
                DefaultExt = ".xls",
                Multiselect = true,
                Filter = "Csv (.csv)|*.csv|Excel (.xls)|*.xls|Excel (.xlsx)|*.xlsx|All files (*.*)|*.*"
            };
            bool? ok = dlg.ShowDialog();

            if (ok == true)
            {
                ClearMessage();
                string message = string.Empty;

                string[] items = dlg.FileNames;
                foreach (string item in items)
                {
                    FileInfo info = new FileInfo(item);
                    if (_attributeFiles.FirstOrDefault(i => i.FilePath == info.FullName) == null)
                        _attributeFiles.Add(new AttributeFileItem(info.Name, info.FullName));
                    else
                        message += $"{info.Name} 已存在;\n";
                }
                ShowMessage(message);
            }
        }
    }
}
