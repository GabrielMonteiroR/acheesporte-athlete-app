using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Dtos.ReservationDtos;
using acheesporte_athlete_app.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace acheesporte_athlete_app.Services;

public class ReservationService : IReservationService
{
    private readonly HttpClient _http;
    private readonly ApiSettings _settings;

    public ReservationService(HttpClient http, ApiSettings settings)
    {
        _http = http;
        _settings = settings;
    }

    public async Task<ReservationsByUserResponseDto> GetReservationsByUserAsync(int userId)
    {
        var url = $"{_settings.BaseUrl}{_settings.GetReservationsByUserIdEndpoint}{userId}";
        var token = await SecureStorage.GetAsync("auth_token");

        using var req = new HttpRequestMessage(HttpMethod.Get, url);
        if (!string.IsNullOrWhiteSpace(token))
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var res = await _http.SendAsync(req);
        var body = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode)
            throw new Exception($"API error {(int)res.StatusCode}: {body}");

        return JsonSerializer.Deserialize<ReservationsByUserResponseDto>(
                   body,
                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
               ?? new ReservationsByUserResponseDto();
    }
}