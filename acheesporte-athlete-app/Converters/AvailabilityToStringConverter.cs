using acheesporte_athlete_app.Dtos.Venues;
using System.Globalization;

namespace acheesporte_athlete_app.Converters;

public class AvailabilityToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not VenueAvailabilityDto dto)
            return string.Empty;

        return $"{dto.StartDate:HH:mm} - {dto.EndDate:HH:mm} | R$ {dto.Price:F2}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
