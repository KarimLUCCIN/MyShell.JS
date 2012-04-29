using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace MyShell.Converters
{
    public class BooleanVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var param = parameter as string;
                bool invert = !String.IsNullOrEmpty(param) && param.Equals("invert", StringComparison.OrdinalIgnoreCase);

                var val = (bool)value;

                if (invert)
                    return val ? Visibility.Collapsed : Visibility.Visible;
                else
                    return val ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
