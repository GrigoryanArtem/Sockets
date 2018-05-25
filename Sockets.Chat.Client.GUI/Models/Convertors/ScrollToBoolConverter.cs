using System;
using System.Globalization;
using System.Windows.Data;

namespace Sockets.Chat.Client.GUI.Models.Convertors
{
    public class ScrollToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double x = (double)values[0];
            double y = (double)values[1];

            return x == y;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
