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
        bool? isReserved = false,
        DateTime? from = null,
        DateTime? to = null,
        List<int>? sportId = null
    )
    {
        try
        {
            var q = new List<string>();

            if (venueTypeId.HasValue) q.Add($"venueTypeId={venueTypeId}");
            if (minCapacity.HasValue) q.Add($"minCapacity={minCapacity}");
            if (maxCapacity.HasValue) q.Add($"maxCapacity={maxCapacity}");
            if (!string.IsNullOrWhiteSpace(name)) q.Add($"name={Uri.EscapeDataString(name)}");
            if (!string.IsNullOrWhiteSpace(address)) q.Add($"address={Uri.EscapeDataString(address)}");
            if (isReserved.HasValue) q.Add($"isReserved={isReserved.Value.ToString().ToLower()}");

            if (sportId is { Count: > 0 })
                q.AddRange(sportId.Select(id => $"sportId={id}"));

            if (from.HasValue) q.Add($"from={from.Value:o}");
            if (to.HasValue) q.Add($"to={to.Value:o}");

            var queryString = q.Count > 0 ? "?" + string.Join("&", q) : string.Empty;
            var url = _apiSettings.BaseUrl + _apiSettings.VenuesEndpoint + queryString;

            var token = await SecureStorage.GetAsync("auth_token");
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<VenueListResponseDto>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                       ?? new VenueListResponseDto();

            return dto.Data ?? [];
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Erro de rede ao buscar locais.", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception("Erro ao interpretar dados dos locais.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado.", ex);
        }
    }

    public async Task<IReadOnlyList<VenueAvailabilityDto>> GetAvailableTimesByVenueIdAsync(int venueId)
    {
        try
        {
            var url = $"{_apiSettings.BaseUrl}{_apiSettings.GetAvailableTimesByVenueIdEndpoint}{venueId}";
            var token = await SecureStorage.GetAsync("auth_token");

            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var res = await _httpClient.SendAsync(req);
            res.EnsureSuccessStatusCode();

            var json = await res.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<AvailableTimesResponseDto>(json, (JsonSerializerOptions?)new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return dto?.Data?.Where(t => !t.IsReserved).OrderBy(t => t.StartDate).ToList()
                   ?? new List<VenueAvailabilityDto>();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao buscar horários disponíveis.", ex);
        }
    }
}