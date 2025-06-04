using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos.GooglePlaces
{
    public class AutocompleteResponse
    {
        [JsonPropertyName("predictions")]
        public List<Prediction> Predictions { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class Prediction
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; }
    }
}
