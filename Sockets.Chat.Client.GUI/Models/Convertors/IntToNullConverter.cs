﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Sockets.Chat.Client.GUI.Models.Convertors
{
    public class IntToNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            return ((int)value) > 0 ? value : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
