using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Dtos;
using acheesporte_athlete_app.Dtos.Venues;
using acheesporte_athlete_app.Interfaces;
using System.Text.Json;

namespace acheesporte_athlete_app.Services
{
    public class VenueService : IVenueService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public VenueService(HttpClient httpClient, ApiSettings apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings;
        }

        public async Task<List<VenueDto>> GetVenuesAsync(
            int? venueTypeId = null,
            int? minCapacity = null,
            int? maxCapacity = null,
            string? name = null,
            string? address = null)
        {
            try
            {
                var queryParams = new List<string>();

                if (venueTypeId.HasValue)
                    queryParams.Add($"venueTypeId={venueTypeId.Value}");
                if (minCapacity.HasValue)
                    queryParams.Add($"minCapacity={minCapacity.Value}");
                if (maxCapacity.HasValue)
                    queryParams.Add($"maxCapacity={maxCapacity.Value}");
                if (!string.IsNullOrWhiteSpace(name))
                    queryParams.Add($"name={Uri.EscapeDataString(name)}");
                if (!string.IsNullOrWhiteSpace(address))
                    queryParams.Add($"address={Uri.EscapeDataString(address)}");

                var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
                var fullUrl = _apiSettings.BaseUrl + _apiSettings.VenueEndpoint + queryString;

                var response = await _httpClient.GetAsync(fullUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var venueResponse = JsonSerializer.Deserialize<VenueListResponseDto>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return venueResponse?.Data ?? new List<VenueDto>();
            }
            catch (HttpRequestException ex)
            { 
                throw new Exception("An error occurred while fetching venues.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("An error occurred while processing the venue data.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }
    }
}
