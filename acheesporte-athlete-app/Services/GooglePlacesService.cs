using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Dtos.GooglePlaces;
using acheesporte_athlete_app.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using System.Web;

namespace acheesporte_athlete_app.Services;

public class GooglePlacesService : IGooglePlacesService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GooglePlacesService(HttpClient httpClient, IOptions<ApiSettings> options)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.PlacesApiKey;
    }

    public async Task<List<Prediction>> GetAutocompleteSuggestionsAsync(string input)
    {
        //TODO: PROTEGER ESTE CAMINHO VIA APP SETTINGS
        var encoded = HttpUtility.UrlEncode(input);
        var url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={encoded}&key={_apiKey}&language=pt-BR&components=country:br";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<AutocompleteResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result?.Predictions ?? new List<Prediction>();
    }

    public async Task<PlaceLocation?> GetPlaceLocationAsync(string placeId)
    {
        var url = $"https://maps.googleapis.com/maps/api/place/details/json?place_id={placeId}&key={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PlaceDetailsResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result?.Result?.Geometry?.Location;
    }
}
