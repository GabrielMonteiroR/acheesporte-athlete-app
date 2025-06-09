using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Dtos.Venues;
using System.Text.Json;


namespace acheesporte_athlete_app.Services
{
    public class VenueTypeService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public VenueTypeService(HttpClient httpClient, ApiSettings apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings;
        }

        public async Task<VenueTypeResponseDto> GetVenueTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiSettings.BaseUrl + _apiSettings.VenueTypeEndpoint);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var venueTypeResponse = JsonSerializer.Deserialize<VenueTypeResponseDto>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return venueTypeResponse ?? new VenueTypeResponseDto
                {
                    Message = "No data available",
                    VenueTypesList = new List<VenueTypeDto>()
                };
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while fetching venue types.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("An error occurred while parsing venue types.", ex);
            }
        }
    }
}
