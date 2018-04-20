using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Firma.Converters
{
    public class TvrtkaToTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string tip = value.ToString();
            return value.ToString().Equals(Constants.TvrtkaTip);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
            {
                return Constants.TvrtkaTip;
            }
            else
            {
                return Constants.OsobaTip;
            }
        }
    }
}
