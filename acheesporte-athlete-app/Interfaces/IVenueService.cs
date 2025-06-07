using acheesporte_athlete_app.Dtos;
using acheesporte_athlete_app.Dtos.Venues;

namespace acheesporte_athlete_app.Interfaces;

public interface IVenueService
{
    Task<List<VenueDto>> GetVenuesAsync( int? venueTypeId = null, int? minCapacity = null, int? maxCapacity = null,
        string? name = null,string? address = null);

}
