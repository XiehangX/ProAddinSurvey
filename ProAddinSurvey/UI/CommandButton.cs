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
using System.Windows.Input;

namespace ProAddinSurvey.UI
{
    internal class CommandButton : Button
    {
        protected override void OnClick()
        {
            string str_id = this.ID;

            if (str_id.StartsWith("ProAddinSurvey_CommandButton_"))
            {
                string commandId = str_id.Replace("ProAddinSurvey_CommandButton_", "esri_");
                var iCommand = FrameworkApplication.GetPlugInWrapper(commandId) as ICommand;
                if (iCommand != null)
                {
                    if (iCommand.CanExecute(null)) iCommand.Execute(null);
                }
            }
            //else if (str_id.StartsWith("SR_"))
            //{
            //    int srNum = Convert.ToInt32(str_id.Replace("SR_", ""));
            //    var map = MapView.Active?.Map;
            //    SpatialReference s = SpatialReferenceBuilder.CreateSpatialReference(srNum);
            //    await QueuedTask.Run(() => map.SetSpatialReference(s));
            //}
        }
    }
}
