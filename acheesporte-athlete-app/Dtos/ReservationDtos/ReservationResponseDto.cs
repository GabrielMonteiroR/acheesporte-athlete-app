using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos.ReservationDtos;

public class ReservationResponseDto
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("reservations")]
    public List<ReservationDto> Reservations { get; set; }
}
