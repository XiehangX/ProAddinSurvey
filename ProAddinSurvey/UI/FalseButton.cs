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

namespace ProAddinSurvey.UI
{
    internal class FalseButton : Button
    {
        protected override void OnClick()
        {
            if (Module1.lyr == null || Module1.flyr == null) return;
            if (Module1.isClickedFwBw == false) return;

            string str_id = this.ID;
            string ts = Module1.Current.GetTimeString();

            switch (str_id)
            {
                case "ProAddinSurvey_UI_FalseButton":
                    Module1.Current.EditNoteFiledValue(Module1.NoteFieldName1, "伪变化", Module1.TimeFieldName1, ts);
                    break;
                case "ProAddinSurvey_UI_FalseButton2":
                    Module1.Current.EditNoteFiledValue(Module1.NoteFieldName3, "伪变化", Module1.TimeFieldName3, ts);
                    break;
                default:
                    break;
            }


        }
    }
}
