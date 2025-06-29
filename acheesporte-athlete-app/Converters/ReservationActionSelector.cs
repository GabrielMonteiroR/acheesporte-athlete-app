using acheesporte_athlete_app.Dtos.ReservationDtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.Converters;

public class ReservationActionSelector : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is ReservationDto res)
        {
            return new Command(async () =>
            {
                if (res.IsPaid)
                {
                    var uri = new Uri($"https://www.google.com/maps/dir/?api=1&destination={res.Venue?.Latitude},{res.Venue?.Longitude}&travelmode=driving");
                    await Launcher.Default.OpenAsync(uri);
                }
                else
                {
                    await Shell.Current.GoToAsync($"pix-payment?reservationId={res.Id}");
                }
            });
        }

        return null!;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
