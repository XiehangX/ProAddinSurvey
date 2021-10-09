using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProAddinSurvey.ViewModels
{
    public class AttributeFileItem : PropertyChangedBase
    {
        private string _fileName;
        private string _filePath;

        public AttributeFileItem(Item item)
        {
            _fileName = item.Name;
            _filePath = item.Path;
        }
        public AttributeFileItem(string name, string path)
        {
            _fileName = name;
            _filePath = path;
        }

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                SetProperty(ref _filePath, value, () => FilePath);
            }
        }
        public string FileName
        {
            get { return _fileName; }
            set
            {
                SetProperty(ref _fileName, value, () => FileName);
            }
        }

        private ICommand _deleteFileCommand;
        public ICommand DeleteFileCommand
        {
            get
            {
                _deleteFileCommand = new RelayCommand(() => RemoveFile());
                return _deleteFileCommand;
            }
        }

        private void RemoveFile()
        {
            if (this != null)
            {
                Module1.Current.ConstructMapVM?.AttributeFiles?.Remove(this);
            }
        }

    }
}
