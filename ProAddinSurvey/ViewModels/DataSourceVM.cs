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


            //ConnectionString = Module1.ConnectionString;
            //UserName = Module1.UserName;
            //Password = Module1.Password;
            //WarningValue = Module1.WarningValue;
        }

        //public string ConnectionString => Module1.ConnectionString;

        //private string _connectionString;
        //public string ConnectionString
        //{
        //    get { return _connectionString; }
        //    set
        //    {
        //        SetProperty(ref _connectionString, value, () => ConnectionString);
        //    }
        //}
        //private string _userName;
        //public string UserName
        //{
        //    get { return _userName; }
        //    set
        //    {
        //        SetProperty(ref _userName, value, () => UserName);
        //    }
        //}

        //private string _password;
        //public string Password
        //{
        //    get { return _password; }
        //    set
        //    {
        //        SetProperty(ref _password, value, () => Password);
        //    }
        //}
        //private string _warningValue;
        //public string WarningValue
        //{
        //    get { return _warningValue; }
        //    set
        //    {
        //        SetProperty(ref _warningValue, value, () => WarningValue);
        //    }
        //}



        #region Commands

        public ICommand CmdOk => new RelayCommand((proWindow) =>
        {
            //ResultList.Clear();
            //List<DiskRecord> list = new List<DiskRecord>();
            //bool isLocal = ConnectionString.ToLower() == "localhost" || ConnectionString == "127.0.0.1";
            //try
            //{
            //    list = isLocal ? Utility.LocalDisk() : Utility.RemoteDisk(ConnectionString, UserName, Password);

            //    foreach (DiskRecord each in list)
            //    {
            //        each.IsWarning = each.Free <= Math.Round(Convert.ToDouble(WarningValue) / 1024, 1);
            //        each.Status = each.IsWarning ? "警告" : "正常";
            //        ResultList.Add(each);
            //    }

            //    Module1.ConnectionString = ConnectionString;
            //    Module1.UserName = UserName;
            //    Module1.Password = Password;
            //    Module1.WarningValue = WarningValue;
            //}
            //catch (Exception exp)
            //{
            //    ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(exp.Message);
            //}


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
            // TODO: set dialog result and close the window
            //(proWindow as ProWindow).DialogResult = true;
            //(proWindow as ProWindow).Close();
        }, () => true);


        #endregion
    }
}
