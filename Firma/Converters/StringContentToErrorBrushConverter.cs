using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Firma.Converters
{
    public class StringContentToErrorBrushConverter : IValueConverter
    {
        public Brush NormalBrush { get; set; }
        public Brush ErrorBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return NormalBrush;
            string errorMessage = (string)value;
            if (string.IsNullOrEmpty(errorMessage))
            {
                return NormalBrush;
            }
            else
            {
                return ErrorBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
