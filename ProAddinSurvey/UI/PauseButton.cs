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

namespace ProAddinSurvey.UI
{
    internal class PauseButton : Button
    {
        protected override void OnClick()
        {
            if (Module1.lyr == null || Module1.flyr == null) return;

            Module1.Current.SaveEdits();
            //MessageBox.Show(ID);
            //FrameworkApplication.State.Deactivate("state1");
            //FrameworkApplication.State.Activate("state0");
            //Module1.Current.ResetState();
        }
    }
}
