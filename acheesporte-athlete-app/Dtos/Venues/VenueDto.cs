using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos;

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

    [JsonPropertyName("allowLocalPayment")]
    public bool AllowLocalPayment { get; set; }

    [JsonPropertyName("imageUrls")]
    public List<string> ImageUrls { get; set; }

    [JsonPropertyName("venueTypeId")]
    public int VenueTypeId { get; set; }

    [JsonPropertyName("rules")]
    public string Rules { get; set; }

    [JsonPropertyName("ownerId")]
    public int OwnerId { get; set; }

    [JsonPropertyName("venueAvaliabilityId")]
    public int VenueAvaliabilityId { get; set; }
}
