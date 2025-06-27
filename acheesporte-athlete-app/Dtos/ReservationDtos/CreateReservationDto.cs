using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos.ReservationDtos;

public class CreateReservationDto
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("venueId")]
    public int VenueId { get; set; }

    [JsonPropertyName("venueAvailabilityTimeId")]
    public int VenueAvailabilityTimeId { get; set; }

    [JsonPropertyName("paymentMethodId")]
    public int PaymentMethodId { get; set; } = 1;
}
