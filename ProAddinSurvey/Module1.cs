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
using ProAddinSurvey.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using ArcGIS.Desktop.Mapping.Events;
using ArcGIS.Desktop.Core.Geoprocessing;
using ArcGIS.Desktop.Editing.Attributes;


namespace ProAddinSurvey
{
    internal class Module1 : Module
    {
        private static Module1 _this = null;
        public ConstructMapVM ConstructMapVM;
        public DataSourceVM DataSourceVM;
        public AssignMapVM AssignMapVM;

        /// <summary>
        /// Retrieve the singleton instance to this module here
        /// </summary>
        public static Module1 Current
        {
            get
            {
                return _this ?? (_this = (Module1)FrameworkApplication.FindModule("ProAddinSurvey_Module"));
            }
        }

        #region Event Listening
        public Module1()
        {
            ActiveMapViewChangedEvent.Subscribe(OnActiveMapViewChanged);

        }


        private MapView _currentActiveMapView = null;

        private void OnActiveMapViewChanged(ActiveMapViewChangedEventArgs activeMapViewChangedEventArgs)
        {
            try
            {
                // We are only interested in the current mapview, when focus changes, we do not refresh if we are still on the current active mapview
                if (activeMapViewChangedEventArgs.IncomingView == null) return;
                if (activeMapViewChangedEventArgs.IncomingView == _currentActiveMapView) return;
                _currentActiveMapView = activeMapViewChangedEventArgs.IncomingView;

                //RefreshLayerComboBox();
                //ResetTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in OnActiveMapViewChanged:  " + ex.ToString(), "Error");
            }


        }

        #endregion Event Listening


        #region Business Logic w

        public static string url;
        public static Map mapPreorder;
        public static Map mapPostorder;
        public static Layer lyr;
        public static FeatureLayer flyr;
        public static string indexField;
        public static IReadOnlyList<long> selectionSet;
        public static int selectionIndex = -1;
        public static string NoteFieldName1 = "QA_Note_addin1";
        public static string TimeFieldName1 = "NoteTime_addin1";
        public static string NoteFieldName3 = "QA_Note_addin3";
        public static string TimeFieldName3 = "NoteTime_addin3";
        public static string outputFolder;
        public static int[] countList = new int[6] { 0, 0, 0, 0, 0, 0 };
        public static Selection QALayerSelection;
        public static bool isClickedFwBw = false;
        public static bool isClickedStatr = false;
        public static bool isSaveEdit = false;





        //public void ResetState()
        //{
        //    FrameworkApplication.State.Deactivate("state1");
        //    FrameworkApplication.State.Activate("state0");
        //}
        public string GetTimeString() => DateTime.Now.ToLocalTime().ToString();

        public int[] GetFeatureCount(FeatureLayer flyr, string fieldName,string tab)
        {
            countList = new int[6] { 0, 0, 0, 0, 0, 0 };
            //int[] countList = new int[6] { 0, 0, 0, 0, 0, 0 };
            // [0]总计、[1]真变化、[2]伪变化、[3]真变化符合、[4]真变化不符合、[5]未判断
            try
            {                
                return QueuedTask.Run<int[]>(() =>
                {
                    RowCursor rows = flyr.Search();
                    string fieldValue;
                    object rowCurrent;
                    while (rows.MoveNext())
                    {                        
                        countList[0]++;
                        rowCurrent = rows.Current[fieldName];
                        fieldValue = rowCurrent == null ? " " : rowCurrent.ToString();

                        switch (fieldValue)
                        {
                            case "真变化":
                                countList[1]++;
                                break;
                            case "伪变化":
                                countList[2]++;
                                break;
                            case "真变化且符合":
                                countList[3]++;
                                break;
                            case "真变化但不符合":
                                countList[4]++;
                                break;
                            default:
                                countList[5]++;
                                break;
                        }
                    }
                    switch (tab)
                    {
                        case "tab1":
                            MessageBox.Show(
                                String.Format(
                                    "已完成判断\n" +
                                    "真变化：{0}\n" +
                                    "伪变化：{1}\n" +
                                    "待判断：{2}\n" +
                                    "总计：{3}\n" +
                                    "继续工程", 
                                    countList[1], countList[2], countList[5], countList[0]));
                            break;
                        case "tab3":
                            MessageBox.Show(
                                String.Format(
                                    "已完成判断\n" +
                                    "真变化且符合：{0}\n" +
                                    "真变化但不符合：{1}\n" +
                                    "伪变化：{2}\n" +
                                    "待判断：{3}\n" +
                                    "总计：{4}\n" +
                                    "继续工程",
                                    countList[3], countList[4], countList[2], countList[5], countList[0]));
                            break;
                        default:
                            break;
                    }
                    return countList;
                }).Result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GetFeatureCount:  " + ex.ToString(), "Error");
                return countList;
                throw;
            }

        }
        
        
        public async static void AddField(FeatureLayer flyr, string fieldName)
        {
            var parameters = Geoprocessing.MakeValueArray(flyr.GetTable(), fieldName, "TEXT");
            var gpResult = await Geoprocessing.ExecuteToolAsync("Management.AddField", parameters);
            if (gpResult.IsFailed == true)
            {
                MessageBox.Show("AddField failed.", "ERROR");
            }
        }

        public void SaveEdits()
        {
            try
            {
                isSaveEdit = true;
                var cmdSaveEdits = FrameworkApplication.GetPlugInWrapper("esri_editing_SaveEditsBtn") as ICommand;
                if (cmdSaveEdits != null)
                {
                    if (cmdSaveEdits.CanExecute(null))
                    {
                        cmdSaveEdits.Execute(null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SaveEdits:  " + ex.ToString(), "Error");
                throw;
            }
        }

        //TODO 点击启动，同时打开属性表
        public void OpenAttributeTable(FeatureLayer QALayer)
        {
            FrameworkExtender.OpenTablePane(FrameworkApplication.Panes, QALayer, TableViewMode.eAllRecords);
        }

        public void InitSelection(string tab, string NoteFieldName, string TimeFieldName)
        {
            try
            {
                QueuedTask.Run( () =>
                {
                    FeatureLayer QALayer = flyr;
                    var FieldsList = QALayer.GetTable().GetDefinition().GetFields();
                    bool isNoteField = false;
                    bool isTimeField = false;
                    //string NoteFieldName = "QA_Note_addin";
                    //string TimeFieldName = "NoteTime_addin";
                    foreach (var fld in FieldsList)
                    {
                        var fldNameString = fld.Name;
                        if (fldNameString.Contains("OBJECTID"))
                        {
                            indexField = fldNameString;
                        }
                        if (fldNameString.Equals(NoteFieldName)) isNoteField = true;
                        if (fldNameString.Equals(TimeFieldName)) isTimeField = true;
                    }
                    // 无字段的图层添加标记  字段
                    if (isNoteField == false) AddField(QALayer, NoteFieldName);

                    if (isTimeField == false) AddField(QALayer, TimeFieldName);

                    var qf = new QueryFilter
                    {
                        WhereClause = NoteFieldName + " IS NULL"
                    };

                    QALayerSelection = QALayer.Select(qf);
                    var ChangedToJudge = QALayerSelection.GetCount();
                    selectionSet = QALayerSelection.GetObjectIDs();
                    //MessageBox.Show(QALayerSelection.GetCount().ToString());
                    if (ChangedToJudge > 0)
                    {
                        MapView.Active.ZoomTo(QALayer, selectionSet, TimeSpan.FromSeconds(0));
                        MapView.Active.ZoomOutFixed(TimeSpan.FromSeconds(0));
                    }
                    _ = GetFeatureCount(Module1.flyr, NoteFieldName, tab);
                    selectionIndex = -1;
                    isClickedStatr = true;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("EditNoteFiledValue failed：" + ex.ToString(), "Error");
                throw;
            }

        }

        public void SelectNextNull(bool Direction)
        {
            try
            {
                int lastindex = selectionSet.Count - 1;
                if(Direction == true) // 下一个
                {
                    if (selectionIndex < lastindex)
                        selectionIndex++;
                    else
                    {
                        MessageBox.Show("已全部遍历完成");
                        return;
                    }
                }
                else // 上一个
                {
                    if (selectionIndex > 0)
                        selectionIndex--;
                    else
                    {
                        MessageBox.Show("已到本轮工作首个图斑");
                    }
                }
                QueuedTask.Run(() =>
                {
                    FeatureLayer QALayer = flyr;
                    var value = selectionSet[selectionIndex];
                    QueryFilter qf = new QueryFilter
                    {
                        WhereClause = indexField + " = " + value
                    };
                    QALayer.Select(qf);
                    MapView.Active.ZoomTo(QALayer, value, TimeSpan.FromSeconds(0));
                    MapView.Active.ZoomOutFixed(TimeSpan.FromSeconds(0));
                    isClickedFwBw = true;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SelectNextNull:  " + ex.ToString(), "Error");
                throw;
            }
            
        }

        public void EditNoteFiledValue(string NoteFieldName,string NoteValue, string TimeFieldName, string ts)
        {
            try
            {
                QueuedTask.Run(() =>
                {
                    FeatureLayer QALayer = flyr;
                    var qaFieldIndex = selectionIndex;
                    Selection QASelection = QALayer.GetSelection();
                    var selectionSet = QASelection.GetObjectIDs();
                    var inspector = new Inspector();
                    inspector.Load(QALayer, selectionSet);
                    inspector[NoteFieldName] = NoteValue;
                    inspector[TimeFieldName] = ts;
                    var editOp = new EditOperation();
                    editOp.Modify(inspector);
                    bool result = editOp.Execute();
                    if (!result)
                    {
                        MessageBox.Show("Operation was not added to undo stack.");
                    }
                    isSaveEdit = false;

                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in EditQANoteFieldValue:  " + ex.ToString(), "Error");
                throw;
            }
        }


        #endregion Business Logic w

        #region Overrides


        /// <summary>
        /// Called by Framework when ArcGIS Pro is closing
        /// </summary>
        /// <returns>False to prevent Pro from closing, otherwise True</returns>
        protected override bool CanUnload()
        {
            //TODO - add your business logic
            //return false to ~cancel~ Application close
            return true;
        }


        protected override Func<Task> ExecuteCommand(string id)
        {

            var command = FrameworkApplication.GetPlugInWrapper(id) as ICommand;
            if (command == null)
                return () => Task.FromResult(0);
            if (!command.CanExecute(null))
                return () => Task.FromResult(0);

            return () =>
            {
                command.Execute(null); // if it is a tool, execute will set current tool
                return Task.FromResult(0);
            };
        }

        #endregion Overrides

    }
}
