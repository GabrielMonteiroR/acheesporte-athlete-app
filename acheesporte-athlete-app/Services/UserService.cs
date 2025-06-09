using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Dtos.User;
using acheesporte_athlete_app.Interfaces;
using System.Net.Http.Json;

namespace acheesporte_athlete_app.Services
{
    public class UserService : IUser
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public UserService(HttpClient httpClient, ApiSettings apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings;
        }

        public async Task<LoginResponseDto> SignInUserAsync(LoginRequestDto dto)
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

    public async Task<RegisterResponseDto> SignInUpUserAsync(RegisterRequestDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_apiSettings.RegisterEndpoint, dto);

                if (response.IsSuccessStatusCode)
                {
                    var registerResponse = await response.Content.ReadFromJsonAsync<RegisterResponseDto>();
                    if (registerResponse is null || string.IsNullOrEmpty(registerResponse.Token))
                        throw new Exception("Invalid registration response.");

                    await SecureStorage.SetAsync("auth_token", registerResponse.Token);

                    return registerResponse;
                }

                var statusCode = (int)response.StatusCode;
                var errorContent = await response.Content.ReadAsStringAsync();

                throw new Exception($"Backend Error ({statusCode}): {errorContent}");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while registering the user.", ex);
            }
        }
    }
}
