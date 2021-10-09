﻿using ArcGIS.Core.CIM;
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
    internal class ShowAssignMapWin : Button
    {

        private AssignMapWin _assignmapwin = null;

        protected override void OnClick()
        {
            //already open?
            if (_assignmapwin != null)
                return;
            _assignmapwin = new AssignMapWin();
            _assignmapwin.Owner = FrameworkApplication.Current.MainWindow;
            _assignmapwin.Closed += (o, e) => { _assignmapwin = null; };
            _assignmapwin.Show();
            //uncomment for modal
            //_assignmapwin.ShowDialog();
        }

    }
}
