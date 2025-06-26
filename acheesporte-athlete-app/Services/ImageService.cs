using acheesporte_athlete_app.Interfaces;
using acheesporte_athlete_app.Dtos.ImageUploadDtos;
using System.Text.Json;
using acheesporte_athlete_app.Configuration;

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
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            content.Add(streamContent, "image", file.FileName);

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
                Image = obj.GetProperty("imageUrl").GetProperty("image").GetString()
            };

        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao fazer upload da imagem de perfil", ex);
        }
    }

    public async Task<List<ImageUploadResponseDto>> UploadVenuesImageAsync(List<FileResult> files)
    {
        try
        {
            if (files is null || files.Count == 0)
                throw new ArgumentNullException(nameof(files), "Nenhum arquivo encontrado");

            var content = new MultipartFormDataContent();

            foreach (var file in files)
            {
                if (file is null || string.IsNullOrWhiteSpace(file.FileName))
                    throw new ArgumentNullException(nameof(file), "Arquivo não encontrado");

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                    throw new ArgumentException("Formato de arquivo inválido. Apenas JPG, JPEG e PNG são permitidos.");

                var stream = await file.OpenReadAsync();
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                content.Add(streamContent, "images", file.FileName);
            }

            var url = $"{_apiSettings.BaseUrl}{_apiSettings.UploadVenueImagesEndpoint}";
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao fazer upload das imagens: {response.ReasonPhrase}");

            var json = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<JsonElement>(json);

            var result = new List<ImageUploadResponseDto>();
            foreach (var element in obj.GetProperty("imageUrls").EnumerateArray())
            {
                result.Add(new ImageUploadResponseDto
                {
                    Image = element.GetProperty("image").GetString()
                });
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao fazer upload das imagens do local", ex);
        }
    }


}