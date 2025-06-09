using acheesporte_athlete_app.Configuration;

namespace acheesporte_athlete_app.Services;

public class ImageService
{
    private HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public ImageService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
    }


}
