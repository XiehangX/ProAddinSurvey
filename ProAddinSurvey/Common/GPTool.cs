using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProAddinSurvey.Common
{
    public class GPTool
    {

        public enum EnumFeatureClassType
        {
            POINT,
            MULTIPOINT,
            POLYLINE,
            POLYGON
        }

        /// <summary>
        /// Create a feature class in the default geodatabase of the project.
        /// </summary>
        /// <param name="featureclassName">Name of the feature class to be created.</param>
        /// <param name="featureclassType">Type of feature class to be created. Options are:
        /// <list type="bullet">
        /// <item>POINT</item>
        /// <item>MULTIPOINT</item>
        /// <item>POLYLINE</item>
        /// <item>POLYGON</item></list></param>
        /// <returns></returns>
        public static async Task CreateFeatureClass(string featureclassName, EnumFeatureClassType featureclassType)
        {
            List<object> arguments = new List<object>
              {
                // store the results in the default geodatabase
                CoreModule.CurrentProject.DefaultGeodatabasePath,
                // name of the feature class
                featureclassName,
                // type of geometry
                featureclassType.ToString(),
                // no template
                "",
                // no z values
                "DISABLED",
                // no m values
                "DISABLED"
              };
            await QueuedTask.Run(() =>
            {
                // spatial reference
                arguments.Add(SpatialReferenceBuilder.CreateSpatialReference(3857));
            });
            IGPResult result = await Geoprocessing.ExecuteToolAsync("CreateFeatureclass_management", Geoprocessing.MakeValueArray(arguments.ToArray()));
        }

        public static async Task<string> GetDataSource(BasicFeatureLayer theLayer)
        {
            try
            {
                return await QueuedTask.Run(() =>
                {
                    var inTable = theLayer.Name;
                    var table = theLayer.GetTable();
                    var dataStore = table.GetDatastore();
                    var workspaceNameDef = dataStore.GetConnectionString();
                    var workspaceName = workspaceNameDef.Split('=')[1];
                    var fullSpec = System.IO.Path.Combine(workspaceName, inTable);
                    return fullSpec;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return string.Empty;
            }
        }

        public static async Task<string> ExecuteAddFieldToolAsync(
          BasicFeatureLayer theLayer,
          KeyValuePair<string, string> field,
          string fieldType, int? fieldLength = null,
          bool isNullable = true)
        {
            return await QueuedTask.Run(() =>
            {
                try
                {
                    var inTable = theLayer.Name;
                    var table = theLayer.GetTable();
                    var dataStore = table.GetDatastore();
                    var workspaceNameDef = dataStore.GetConnectionString();
                    var workspaceName = workspaceNameDef.Split('=')[1];

                    var fullSpec = System.IO.Path.Combine(workspaceName, inTable);
                    System.Diagnostics.Debug.WriteLine($@"Add {field.Key} from {fullSpec}");

                    var parameters = Geoprocessing.MakeValueArray(fullSpec, field.Key, fieldType.ToUpper(), null, null,
                          fieldLength, field.Value, isNullable ? "NULABLE" : "NON_NULLABLE");
                    var cts = new CancellationTokenSource();
                    var results = Geoprocessing.ExecuteToolAsync("management.AddField", parameters, null, cts.Token,
                          (eventName, o) =>
                          {
                              System.Diagnostics.Debug.WriteLine($@"GP event: {eventName}");
                          });
                    var isFailure = results.Result.IsFailed || results.Result.IsCanceled;
                    return !isFailure ? "Failed" : "Ok";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return ex.ToString();
                }
            });
        }

        public static Task<bool> FeatureClassExistsAsync(string fcName)
        {
            return QueuedTask.Run(() =>
            {
                try
                {
                    using (Geodatabase projectGDB = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri(Project.Current.DefaultGeodatabasePath))))
                    {
                        using (FeatureClass fc = projectGDB.OpenDataset<FeatureClass>(fcName))
                        {
                            return fc != null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($@"FeatureClassExists Error: {ex.ToString()}");
                    return false;
                }
            });
        }

    }
}
