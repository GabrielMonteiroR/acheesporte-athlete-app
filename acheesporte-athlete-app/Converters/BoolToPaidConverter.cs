using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace acheesporte_athlete_app.Converters;

public class BoolToPaidConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isPaid)
            return isPaid ? "Pago" : "Pendente";

        return "Desconhecido";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
