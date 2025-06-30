using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Dtos;
using acheesporte_athlete_app.Dtos.Sports;
using acheesporte_athlete_app.Helpers;
using acheesporte_athlete_app.Interfaces;
using System.Net.Http.Json;

public class SportService : ISportService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public SportService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
    }

    public async Task<List<SportDto>> GetSportsAsync()
    {
        var response = await _httpClient.GetAsync(_apiSettings.GetSportsEndpoint);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<SportDto>>>();
        return result?.Data ?? new List<SportDto>();
    }
}
