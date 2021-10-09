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
    /// Interaction logic for ConstructMapWin.xaml
    /// </summary>
    public partial class ConstructMapWin : ArcGIS.Desktop.Framework.Controls.ProWindow
    {
        ConstructMapVM vm = new ConstructMapVM();
        public ConstructMapWin()
        {
            InitializeComponent();
            this.DataContext = vm;
            Module1.Current.ConstructMapVM = vm;
        }
    }
}
