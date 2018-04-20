using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Firma.Converters
{
    public class DecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((decimal)value).ToString("0.00");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return decimal.Parse(value as string);
        }
    }
}
