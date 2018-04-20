using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Firma.Converters
{
    public class OsobaToTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString().Equals(Constants.OsobaTip);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
            {
                return Constants.OsobaTip;
            }
            else
            {
                return Constants.TvrtkaTip;
            }
        }
    }
}
