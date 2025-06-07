using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos.Venues;

public class VenueListResponseDto
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    public List<VenueDto> Data { get; set; }
}
