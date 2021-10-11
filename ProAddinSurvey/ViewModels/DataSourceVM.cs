using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Controls;
using ProAddinSurvey.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProAddinSurvey.ViewModels
{
    public class DataSourceVM : PropertyChangedBase
    {
        //public ObservableCollection<DataSourceSetting> ResultList { get; set; } = new ObservableCollection<DataSourceSetting>();

        public IObservable<DataSourceSetting> Setting { get; set; }
        public DataSourceVM()
        {
            //ResultList = new ObservableCollection<DiskRecord>();

            //new DataSourceSetting()
            AdminArea_City = Module1.Current.DataSourceVM?.AdminArea_City;
            AdminArea_Country = Module1.Current.DataSourceVM?.AdminArea_Country;
            AdminArea_Town = Module1.Current.DataSourceVM?.AdminArea_Town;
            DLTB = Module1.Current.DataSourceVM?.DLTB;
            JBNT = Module1.Current.DataSourceVM?.JBNT;
            STHX = Module1.Current.DataSourceVM?.STHX;
        }


        private string _adminArea_City;
        public string AdminArea_City
        {
            get { return _adminArea_City; }
            set { SetProperty(ref _adminArea_City, value, () => AdminArea_City); }
        }
        private string _adminArea_Country;
        public string AdminArea_Country
        {
            get { return _adminArea_Country; }
            set { SetProperty(ref _adminArea_Country, value, () => AdminArea_Country); }
        }
        private string _adminArea_Town;
        public string AdminArea_Town
        {
            get { return _adminArea_Town; }
            set { SetProperty(ref _adminArea_Town, value, () => AdminArea_Town); }
        }
        private string _DLTB;
        public string DLTB
        {
            get { return _DLTB; }
            set { SetProperty(ref _DLTB, value, () => DLTB); }
        }
        private string _JBNT;
        public string JBNT
        {
            get { return _JBNT; }
            set { SetProperty(ref _JBNT, value, () => JBNT); }
        }
        private string _STHX;
        public string STHX
        {
            get { return _STHX; }
            set { SetProperty(ref _STHX, value, () => STHX); }
        }

        #region Commands

        //public ICommand CmdOk => new RelayCommand((proWindow) =>
        //{
        //    OpenItemDialog pathDialog = new OpenItemDialog()
        //    {
        //        Title = "选择图层",
        //        //InitialLocation = @"C:\Data\",
        //        MultiSelect = false,
        //        Filter = ItemFilters.composite_addToMap
        //    };
        //    bool? ok = pathDialog.ShowDialog();

        //    if (ok == true)
        //    {
        //        IEnumerable<Item> selectedItems = pathDialog.Items;
        //        foreach (Item selectedItem in selectedItems)
        //        {
        //            //TxtUri.Text = selectedItem.Path;
        //            MessageBox.Show(selectedItem.Path);
        //        }
        //    }
        //}, () => true);

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
            OpenItemDialog pathDialog = new OpenItemDialog()
            {
                Title = "选择图层",
                //InitialLocation = @"C:\Data\",
                MultiSelect = false,
                Filter = ItemFilters.composite_addToMap
            };
            bool? ok = pathDialog.ShowDialog();

            if (ok == true)
            {
                IEnumerable<Item> selectedItems = pathDialog.Items;
                foreach (Item selectedItem in selectedItems)
                {
                    switch (param.ToString())
                    {
                        case "AdminArea_City":
                            AdminArea_City = selectedItem.Path;
                            break;
                        case "AdminArea_Country":
                            AdminArea_Country = selectedItem.Path;
                            break;
                        case "AdminArea_Town":
                            AdminArea_Town = selectedItem.Path;
                            break;
                        case "DLTB":
                            DLTB = selectedItem.Path;
                            break;
                        case "JBNT":
                            JBNT = selectedItem.Path;
                            break;
                        case "STHX":
                            STHX = selectedItem.Path;
                            break;
                    }
                }
            }
        }


        //private RelayCommand _applyCommand;
        ///// <summary>
        ///// 确定
        ///// </summary>
        //public ICommand ApplyCommand
        //{
        //    get
        //    {
        //        if (_applyCommand == null)
        //            _applyCommand = new RelayCommand(() => SaveChanges());

        //        return _applyCommand;
        //    }
        //}
        //private void SaveChanges()
        //{
        //    MessageBox.Show("SaveChanges", "SaveChanges", MessageBoxButton.OK, MessageBoxImage.Information);
        //}
        #endregion
    }
}
