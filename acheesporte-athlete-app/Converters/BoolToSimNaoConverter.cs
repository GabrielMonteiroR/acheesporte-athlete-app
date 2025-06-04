using System;
using System.Globalization;

namespace acheesporte_athlete_app.Converters
{
    public class BoolToSimNaoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b ? (b ? "Sim" : "Não") : "Não";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
