using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;

namespace ARPAVTemporali.Converters
{
    public class StringToDoubleConverter: IValueConverter
    {
        public StringToDoubleConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			
            return double.Parse(value.ToString(), CultureInfo.InvariantCulture);
		}

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
