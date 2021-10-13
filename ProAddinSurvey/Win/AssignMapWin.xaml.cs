using ProAddinSurvey.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProAddinSurvey.Win
{
    /// <summary>
    /// Interaction logic for AssignMapWin.xaml
    /// </summary>
    public partial class AssignMapWin : ArcGIS.Desktop.Framework.Controls.ProWindow
    {
        AssignMapVM vm = new AssignMapVM();
        public AssignMapWin()
        {
            InitializeComponent();
            this.DataContext = vm;
            Module1.Current.AssignMapVM = vm;
        }
    }
}
