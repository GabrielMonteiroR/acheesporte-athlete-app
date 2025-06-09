using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos.Venues;

public class VenueDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("capacity")]
    public int Capacity { get; set; }

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("rules")]
    public string Rules { get; set; }

    [JsonPropertyName("venue_type_id")]
    public int VenueTypeId { get; set; }

    [JsonPropertyName("venue_type_name")]
    public string VenueTypeName { get; set; }

    [JsonPropertyName("owner_id")]
    public int OwnerId { get; set; }

    [JsonPropertyName("owner_name")]
    public string OwnerName { get; set; }

    [JsonPropertyName("sports")]
    public List<string> Sports { get; set; } = new();

    [JsonPropertyName("image_urls")]
    public List<string>? ImageUrls { get; set; } = new();

    [JsonPropertyName("venue_avaliability_times")]
    public List<VenueAvailabilityDto> VenueAvailabilityTimes { get; set; } = new();
}
