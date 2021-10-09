using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProAddinSurvey.Win
{
    internal class ShowDataSourceWin : Button
    {

        private DataSourceWin _datasourcewin = null;

        protected override void OnClick()
        {
            //already open?
            if (_datasourcewin != null)
                return;
            _datasourcewin = new DataSourceWin();
            //if (string.IsNullOrEmpty(Module1.ConnectionString))
            //    Module1.ConnectionString = $@"localhost";
            _datasourcewin.Owner = FrameworkApplication.Current.MainWindow;
            _datasourcewin.Closed += (o, e) => { _datasourcewin = null; };
            _datasourcewin.Show();
            //uncomment for modal
            //_datasourcewin.ShowDialog();
        }

    }
}
