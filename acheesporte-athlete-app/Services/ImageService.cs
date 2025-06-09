using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Dtos.ImageDtos;
using acheesporte_athlete_app.Interfaces;
using System.Text.Json;

namespace acheesporte_athlete_app.Services;

public class ImageService : IImageService
{
    private HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public ImageService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
    }

    public async Task<ImageUploadResponseDto> UploadProfileImageAsync(FileResult file)
    {
        try
        {
            if (file is null || string.IsNullOrWhiteSpace(file.FileName))
            {
                throw new ArgumentNullException(nameof(file), "Arquivo não encontrado");
            }

            var alowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!alowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Formato de arquivo inválido. Apenas JPG, JPEG e PNG são permitidos.");
            }

            using var stream = await file.OpenReadAsync();
            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(stream), "image", file.FileName);

            var url = $"{_apiSettings.BaseUrl}{_apiSettings.ImageUploadEndpoint}";
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao fazer upload da imagem: {response.ReasonPhrase}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<JsonElement>(json);
            return new ImageUploadResponseDto
            {
                Image = obj.GetProperty("imageUrl").GetProperty("Image").GetString()
            };

        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao fazer upload da imagem de perfil", ex);
        }
    }
}
