﻿using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Dtos;
using acheesporte_athlete_app.Dtos.ReservationDtos;
using acheesporte_athlete_app.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

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

    public async Task<ReservationDto?> CreateReservationAsync(CreateReservationDto dto)
    {
        try
        {
            var url = _apiSettings.BaseUrl + _apiSettings.ReservationsEndpoint;
            var token = await SecureStorage.GetAsync("auth_token");

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(dto);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ReservationDto>();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao criar reserva.", ex);
        }
    }

    public async Task<ReservationsByUserResponseDto> GetHistoryByUserAsync(int userId)
    {
        var url = $"{_apiSettings.BaseUrl}{_apiSettings.GetHistoryByUserIdEndpoint}{userId}";
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

    public async Task<ReservationsByUserResponseDto> GetNextReservationByUserAsync(int userId)
    {
        var url = $"{_apiSettings.BaseUrl}{_apiSettings.GetNextReservationByUserIdEndpoint}{userId}";
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

    public async Task<StreakDto?> GetUserStreakAsync(int userId)
    {
        try
        {
            var url = $"{_apiSettings.BaseUrl}{_apiSettings.GetUserCurrentStreakEndpoint}{userId}";
            var token = await SecureStorage.GetAsync("auth_token");

            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            if (!string.IsNullOrWhiteSpace(token))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var res = await _httpClient.SendAsync(req);
            var content = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
                throw new Exception($"API error {(int)res.StatusCode}: {content}");

            return JsonSerializer.Deserialize<StreakDto>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            return null;
        }
    }

}