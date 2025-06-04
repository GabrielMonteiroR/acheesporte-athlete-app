using System.Threading.Tasks;
using System.Collections.Generic;
using acheesporte_athlete_app.Dtos.GooglePlaces;

namespace acheesporte_athlete_app.Interfaces
{
    public interface IGooglePlacesService
    {
        Task<List<Prediction>> GetAutocompleteSuggestionsAsync(string input);
        Task<PlaceLocation?> GetPlaceLocationAsync(string placeId);
    }
}
