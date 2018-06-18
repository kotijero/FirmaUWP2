using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Firma.Converters
{
    public class EditModeToWidthConverter : IValueConverter
    {
        public double EditModeWidth { get; set; }
        public double NotInEditModeWidth { get; set; }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
            {
                return EditModeWidth;
            }
            else
            {
                return NotInEditModeWidth;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
