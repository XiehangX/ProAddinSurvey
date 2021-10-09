using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ProAddinSurvey.Common
{
    internal class BoolToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException("The target must be of type Visibility");

            var boolValue = System.Convert.ToBoolean(value);
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be of type bool");

            if (value == null)
                return true;

            Visibility visiblity = (Visibility)System.Convert.ToInt32(value);

            return visiblity == Visibility.Visible ? true : false;
        }
    }
}
