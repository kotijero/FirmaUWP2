using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Firma.Converters
{
    class IsPositiveToForegroundColorConverter : IValueConverter
    {
        public Brush PositiveColorBrush { get; set; }
        public Brush NegativeColorBrush { get; set; }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value) return PositiveColorBrush;
            else return NegativeColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
