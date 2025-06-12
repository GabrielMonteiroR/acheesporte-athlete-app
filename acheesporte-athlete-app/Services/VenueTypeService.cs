using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Dtos.Venues;
using System.Net.Http.Headers;
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
                var token = await SecureStorage.GetAsync("auth_token");

                var request = new HttpRequestMessage(HttpMethod.Get, _apiSettings.BaseUrl + _apiSettings.VenueTypeEndpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);
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
                throw new Exception("Erro ao buscar tipos de local.", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Erro ao interpretar resposta dos tipos de local.", ex);
            }
        }
    }
}
