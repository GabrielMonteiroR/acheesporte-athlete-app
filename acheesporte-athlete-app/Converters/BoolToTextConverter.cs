using System.Globalization;

namespace acheesporte_athlete_app.Converters;

public class BoolToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isPaid)
            return isPaid ? "Ver no Mapa" : "Pagar com PIX";
        return "Desconhecido";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
