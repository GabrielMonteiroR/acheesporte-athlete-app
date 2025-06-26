using acheesporte_athlete_app.Config;
using acheesporte_athlete_app.Dtos;
using acheesporte_athlete_app.Dtos.Users;
using acheesporte_athlete_app.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace acheesporte_locator_app.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public UserService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
    }

    public async Task<SignInResponseDto> SignInUserAsync(SignInRequestDto request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(_apiSettings.SignInEndpoint, request);
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<SignInResponseDto>();

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

    public async Task<SignUpResponseDto> SignUpUserAsync(SignUpRequestDto request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(_apiSettings.SignUpEndpoint, request);

            if (response.IsSuccessStatusCode)
            {
                var registerResponse = await response.Content.ReadFromJsonAsync<SignUpResponseDto>();
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

    public async Task<UserResponseDto> GetUserByIdAsync(int id)
    {
        var token = await SecureStorage.GetAsync("auth_token");
        if (string.IsNullOrWhiteSpace(token))
            throw new UnauthorizedAccessException("Token não encontrado. Faça login novamente.");

        var url = $"{_apiSettings.BaseUrl}{_apiSettings.GetUserById}{id}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UserResponseDto>()
               ?? throw new Exception("Resposta inesperada do servidor.");
    }


    public async Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserRequestDto dto)
    {
        var token = await SecureStorage.GetAsync("auth_token");
        if (string.IsNullOrWhiteSpace(token))
            throw new UnauthorizedAccessException("Token não encontrado. Faça login novamente.");

        var url = $"{_apiSettings.BaseUrl}{_apiSettings.UpdateUserInfoEndpoint}{id}";

        var request = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = JsonContent.Create(dto)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UserResponseDto>()
               ?? throw new Exception("Resposta inesperada do servidor.");
    }

    public async Task<UserResponseDto> UpdateUserProfileImageAsync(int userId, string imageUrl)
    {
        var token = await SecureStorage.GetAsync("auth_token");
        if (string.IsNullOrWhiteSpace(token))
            throw new UnauthorizedAccessException("Token não encontrado.");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var url = $"{_apiSettings.BaseUrl}{_apiSettings.UpdateUserProfilePictureBaseUrl}{userId}{_apiSettings.UpdateUserProfilePicturePatchUrl}";

        var dto = new UpdateUserProfileImageRequestDto { ImageUrl = imageUrl };
        var request = new HttpRequestMessage(HttpMethod.Patch, url)
        {
            Content = JsonContent.Create(dto)
        };

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var backendError = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro PATCH foto: {backendError}");
        }

        return await response.Content.ReadFromJsonAsync<UserResponseDto>()
               ?? throw new Exception("Resposta inesperada do servidor.");
    }


}