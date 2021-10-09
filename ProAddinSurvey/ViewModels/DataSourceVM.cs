using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
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

        }



        #region Commands

        public ICommand CmdOk => new RelayCommand((proWindow) =>
        {

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
                    //TxtUri.Text = selectedItem.Path;
                    MessageBox.Show(selectedItem.Path);
                }
            }
        }, () => true);


        #endregion
    }
}
