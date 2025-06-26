using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Dtos.Venues;
using acheesporte_athlete_app.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace acheesporte_athlete_app.Services;

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
        string? address = null,
        bool? isReserved = null
        )
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
            if (isReserved.HasValue)
                queryParams.Add($"isReserved={isReserved.Value.ToString().ToLower()}");

            //TDDO: var fullUrl = _apiSettings.BaseUrl + _apiSettings.VenueEndpoint + queryString;
            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
            var fullUrl = _apiSettings.BaseUrl + queryString;

            var token = await SecureStorage.GetAsync("auth_token");

            var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var venueResponse = JsonSerializer.Deserialize<VenueListResponseDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new VenueListResponseDto
            {
                Data = new List<VenueDto>()
            };

            return venueResponse.Data ?? new List<VenueDto>();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Erro ao buscar locais.", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception("Erro ao processar os dados dos locais.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado.", ex);
        }
    }


}
