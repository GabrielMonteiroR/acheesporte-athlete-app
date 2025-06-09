using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public UserService(HttpClient httpClient, ApiSettings apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_apiSettings.LoginEndpoint, dto);
                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

                    if (loginResponse is null || string.IsNullOrEmpty(loginResponse.Token))
                    {
                        throw new Exception("Login failed. Invalid response from server.");
                    }
                    await SecureStorage.SetAsync("token", loginResponse.Token);
                    return loginResponse;
                }

                var backendError = await response.Content.ReadAsStringAsync();
                throw new Exception($"Login failed: {backendError}");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while logging in.", ex);
            }
        }
    }
}
