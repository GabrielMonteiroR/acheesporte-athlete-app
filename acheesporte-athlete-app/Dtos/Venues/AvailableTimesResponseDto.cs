using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos.Venues;

public class AvailableTimesResponseDto
{
    [JsonPropertyName("data")]
    public List<VenueAvailabilityDto>? Data { get; set; }
}

