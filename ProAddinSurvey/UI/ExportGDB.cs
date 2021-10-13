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
using ArcGIS.Desktop.Core.Geoprocessing;


namespace ProAddinSurvey.UI
{
    internal class ExportGDB : Button
    {

        public static string gdbFullName;
        protected override void OnClick()
        {
            string str_id = this.ID;
            FeatureLayer QALayer = Module1.flyr;
            string[] NoteTypeList;
            string ts = Module1.Current.GetTimeString().Replace(':', '_').Replace('/','_').Replace(' ','_');
            
            switch (str_id)
            {
                case "ProAddinSurvey_ExportGDB":
                    NoteTypeList = new string[2]{"真变化", "伪变化"};
                    ExportGDB_main(QALayer, Module1.NoteFieldName1, NoteTypeList, ts);
                    break;
                case "ProAddinSurvey_ExportGDB2":
                    NoteTypeList = new string[3]{ "真变化但不符合", "真变化且符合", "伪变化" };
                    ExportGDB_main(QALayer, Module1.NoteFieldName3, NoteTypeList, ts);
                    break;
                default:
                    MessageBox.Show("Error");
                    break;
            }
        }

        public void ExportGDB_main(FeatureLayer QALayer, string NoteFieldName, string[] NoteTypeList, string ts)
        {
            QueuedTask.Run(() => 
            {
                string featureName;
                string clause;
                
                string gdbNameEx = NoteTypeList.Length == 2 ? "真伪判断_" : "结果核实_";
                foreach (var NoteType in NoteTypeList)
                {
                    CreateGDB(Module1.outputFolder, gdbNameEx + NoteType);
                    featureName = gdbFullName + "\\" + NoteType + ts;
                    clause = NoteFieldName + " = '" + NoteType + "'";
                    Selection subSelection = QALayer.Select(new QueryFilter { WhereClause = clause });
                    ExportToGDB(QALayer, gdbFullName + "\\" + NoteType + ts);
                }
            
            });
        }


        public async static void CreateGDB(string outputFolder, string gdbName)
        {
            var parameters = Geoprocessing.MakeValueArray(outputFolder, gdbName);
            var gpResult = await Geoprocessing.ExecuteToolAsync("Management.CreateFileGDB", parameters);
            if(gpResult.IsFailed == true)
            {
                MessageBox.Show("CreateFileGDB Failed", "Error");
            }
            gdbFullName = outputFolder + "\\" + gdbName +".gdb";
        }

        public async static void ExportToGDB(Layer QALayer, string outputPath)
        {
            var parameters = Geoprocessing.MakeValueArray(QALayer.ToString(), outputPath);
            var gpResult = await Geoprocessing.ExecuteToolAsync("Management.CopyFeatures", parameters);
            if (gpResult.IsFailed == true)
            {
                MessageBox.Show("ExportToGDB Failed", "Error");
            }
        }


    }
}
