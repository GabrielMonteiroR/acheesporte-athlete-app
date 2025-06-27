using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Dtos.ReservationDtos;
using acheesporte_athlete_app.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace acheesporte_athlete_app.Services;

public class ReservationService : IReservationService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public ReservationService(HttpClient http, ApiSettings settings)
    {
        _httpClient = http;
        _apiSettings = settings;
    }

    public async Task<ReservationsByUserResponseDto> GetReservationsByUserAsync(int userId)
    {
        var url = $"{_apiSettings.BaseUrl}{_apiSettings.GetReservationsByUserIdEndpoint}{userId}";
        var token = await SecureStorage.GetAsync("auth_token");

        using var req = new HttpRequestMessage(HttpMethod.Get, url);
        if (!string.IsNullOrWhiteSpace(token))
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var res = await _httpClient.SendAsync(req);
        var body = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode)
            throw new Exception($"API error {(int)res.StatusCode}: {body}");

        return JsonSerializer.Deserialize<ReservationsByUserResponseDto>(
                   body,
                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
               ?? new ReservationsByUserResponseDto();
    }

    public async Task<bool> CreateReservationAsync(CreateReservationDto dto)
    {
        try
        {
            var url = _apiSettings.BaseUrl + _apiSettings.ReservationsEndpoint;
            var token = await SecureStorage.GetAsync("auth_token");

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(dto);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao criar reserva.", ex);
        }
    }

}