using System.Globalization;

namespace acheesporte_athlete_app.Converters;

public class EmptyFieldToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string? input = value as string;
        return string.IsNullOrWhiteSpace(input) ? Colors.Red : Color.FromArgb("#200937");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
