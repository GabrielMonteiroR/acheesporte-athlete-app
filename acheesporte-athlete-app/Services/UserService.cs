using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Dtos.User;
using acheesporte_athlete_app.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace acheesporte_athlete_app.Services
{
    public class UserService : IUserService
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
                var response = await _httpClient.PostAsJsonAsync(_apiSettings.SignInEndpoint, dto);
                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

                    if (loginResponse is null || string.IsNullOrEmpty(loginResponse.Token))
                    {
                        throw new Exception("Login failed. Invalid response from server.");
                    }
                    await SecureStorage.SetAsync("auth_token", loginResponse.Token);
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
                var response = await _httpClient.PostAsJsonAsync(_apiSettings.SignInUpEndpoint, dto);

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

    public async Task<CurrentUserDto> GetCurrentUserAsync()
        {
            try
            {
                var token = await SecureStorage.GetAsync("auth_token");

                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("Token não encontrado no SecureStorage.");

                var request = new HttpRequestMessage(HttpMethod.Get, _apiSettings.CurrentUserEndpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var statusCode = (int)response.StatusCode;

                    throw new Exception($"Erro ao buscar usuário logado ({statusCode}): {content}");
                }

                var user = await response.Content.ReadFromJsonAsync<CurrentUserDto>();
                return user!;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the current user.", ex);
            }
        }
    }
}