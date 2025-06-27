using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos.ReservationDtos;

public class ReservationDto
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("userId")] public int UserId { get; set; }
    [JsonPropertyName("venueId")] public int VenueId { get; set; }

    [JsonPropertyName("venueAvailabilityTimeId")]
    public int VenueAvailabilityTimeId { get; set; }

    [JsonPropertyName("venueAvailabilityTime")]
    public VenueAvailabilityTimeDto VenueAvailabilityTime { get; set; } = null!;

    [JsonPropertyName("locator")]
    public LocatorDto Locator { get; set; }

    [JsonPropertyName("venue")]
    public VenueDto? Venue { get; set; }                   

    [JsonPropertyName("paymentMethodId")] public int PaymentMethodId { get; set; }
    [JsonPropertyName("isPaid")] public bool IsPaid { get; set; }
}
