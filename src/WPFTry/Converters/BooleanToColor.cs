using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WPFTry
{
    [ValueConversion( typeof( bool ), typeof( SolidColorBrush ) )]
    public class BooleanToColor : IValueConverter
    {
        public bool IsReversed { get; set; }
        public bool UseHidden { get; set; }
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            var val = System.Convert.ToBoolean( value, CultureInfo.InvariantCulture );
            if( this.IsReversed )
            {
                val = !val;
            }
            if( val )
            {
                return new SolidColorBrush( Color.FromRgb( 255, 153, 0 ) );
            }
            return new SolidColorBrush( Color.FromRgb( 204, 204, 204 ) );
        }
        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}
