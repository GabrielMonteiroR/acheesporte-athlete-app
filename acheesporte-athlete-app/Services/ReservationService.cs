using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Dtos.ReservationDtos;
using acheesporte_athlete_app.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace acheesporte_athlete_app.Services;

public class ReservationService : IReservationService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public ReservationService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
    }

 public async Task<ReservationResponseDto> GetReservationsByUserAsync(ReservationRequestDto requestDto)
    {
        try
        {
            var requestUrl = $"{_apiSettings.UpdateUserProfilePicturePatchUrl}/{requestDto.UserId}";

            if (requestDto.Status.HasValue)
            {
                requestUrl += $"?status={requestDto.Status.Value}";
            }

            var token = await SecureStorage.GetAsync("auth_token");

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var reservationResponse = JsonSerializer.Deserialize<ReservationResponseDto>(json, options);

                return reservationResponse ?? new ReservationResponseDto
                {
                    Message = "No reservations found.",
                    Reservations = new List<ReservationDto>()
                };
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to fetch reservations: {response.StatusCode} - {errorContent}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while fetching reservations.", ex);
        }
    }
}