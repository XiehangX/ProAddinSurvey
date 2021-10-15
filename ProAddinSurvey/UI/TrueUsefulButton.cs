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
    internal class TrueUsefulButton : Button
    {
        protected override void OnClick()
        {
            if (Module1.lyr == null || Module1.flyr == null) return;
            if (Module1.isClickedFwBw == false) return;

            string ts = Module1.Current.GetTimeString();
            Module1.Current.EditNoteFiledValue(Module1.NoteFieldName3, "真变化且符合", Module1.TimeFieldName3, ts);
        }
    }
}
