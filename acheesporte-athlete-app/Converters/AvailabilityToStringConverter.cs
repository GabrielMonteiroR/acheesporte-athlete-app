using acheesporte_athlete_app.Dtos.Venues;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.Converters;
public class AvailabilityToStringConverter : IValueConverter
{
    public object Convert(object value, Type t, object p, CultureInfo c)
    {
        if (value is VenueAvailabilityDto a)
            return $"{a.StartDate:HH:mm} – R$ {a.Price:F2}";
        return string.Empty;
    }

    public object ConvertBack(object v, Type t, object p, CultureInfo c) => throw new NotImplementedException();
}
