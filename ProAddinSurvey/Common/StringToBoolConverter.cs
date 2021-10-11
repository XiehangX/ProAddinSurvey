using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProAddinSurvey.Common
{
    public class StringToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Converts OnlineUri query string length to a bool. A query string length &gt; 0 returns true
        /// </summary>
        /// <returns>Pass "true" as the parameter to flip (reverse) the returned bool (i.e. !true or !false)</returns>
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {

            string content = value == null ? "" : (string)value;
            var enable = content.Length > 0;

            if (parameter != null)
            {
                if (parameter.ToString().ToLower().CompareTo("true") == 0)
                {
                    enable = !enable;
                }
            }

            return enable;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Converter cannot convert back.");
        }
    }
}
