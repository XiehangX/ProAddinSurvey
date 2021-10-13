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
    internal class StartButton : Button
    {
        protected override void OnClick()
        {
            string str_id = this.ID;
            try
            {
                //Module1.mapPreorder = MapView.Active.Map;
                //Module1.mapPostorder = MapView.Active.Map;
                //Module1.lyr = await AddLayer(Module1.url, Module1.mapPreorder);
                //await AddLayer(Module1.url, Module1.mapPostorder);
                //Module1.flyr = await GetFeatureLayer(Module1.lyr);
                //Module1.Current.OpenAttributeTable(Module1.flyr);
                switch (str_id)
                {
                    case "ProAddinSurvey_UI_StartButton":
                        Module1.Current.InitSelection("tab1", Module1.NoteFieldName1, Module1.TimeFieldName1);
                        break;
                    case "ProAddinSurvey_UI_StartButton2":
                        Module1.Current.InitSelection("tab3", Module1.NoteFieldName3, Module1.TimeFieldName3);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to Init" + ex.ToString(), "Error");
            }
        }


        
    }
}
