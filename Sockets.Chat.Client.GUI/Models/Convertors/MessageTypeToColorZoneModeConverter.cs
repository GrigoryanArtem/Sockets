using MaterialDesignThemes.Wpf;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Sockets.Chat.Client.GUI.Models.Convertors
{
    public class MessageTypeToColorZoneModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            var type = (MessageType)value;
            ColorZoneMode result;

            switch (type)
            {
                case MessageType.Default:
                    result = ColorZoneMode.PrimaryLight;
                    break;
                case MessageType.Mention:
                    result = ColorZoneMode.PrimaryDark;
                    break;
                case MessageType.Recipient:
                    result = ColorZoneMode.Accent;
                    break;
                default:
                    throw new NotSupportedException();                    
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
