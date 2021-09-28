using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
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
    internal class GpToolButton : Button
    {
        protected override void OnClick()
        {
            string str_id = this.ID;

            string prefix_gptool = "ProAddinSurvey_GpTool_";
            if (str_id.StartsWith(prefix_gptool))
            {
                string tool_name = str_id.Substring(prefix_gptool.Length);
                switch (tool_name)
                {
                    case "GeoTaggedPhotosToPoints":
                        string input_folder = "";
                        string output_feature_class = "";
                        var param_values = Geoprocessing.MakeValueArray(input_folder, output_feature_class);
                        Geoprocessing.OpenToolDialog("management.GeoTaggedPhotosToPoints", param_values);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
