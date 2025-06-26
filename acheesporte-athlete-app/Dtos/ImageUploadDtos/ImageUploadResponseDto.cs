using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos.ImageUploadDtos;

public class ImageUploadResponseDto
{
    [JsonPropertyName("image")]
    public string Image { get; set; }
}
