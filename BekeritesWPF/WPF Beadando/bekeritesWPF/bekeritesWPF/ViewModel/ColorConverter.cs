using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Enumeration;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Color = System.Drawing.Color;
using RetardColor = System.Windows.Media.Color;

namespace bekeritesWPF.ViewModel {
    internal class ColorConverter {
        

        public static RetardColor ToRetardColor(Color color) {
            return RetardColor.FromArgb(color.A, color.R, color.G, color.B);
        }
        public static SolidColorBrush ConvertToBrush(Color value) {
            if (value is System.Drawing.Color color) {
                return new SolidColorBrush(ToRetardColor(value));
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
