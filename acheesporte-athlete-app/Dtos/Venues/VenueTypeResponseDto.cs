using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos.Venues
{
    public class VenueTypeResponseDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("venueTypesList")]
        public List<VenueTypeDto> VenueTypesList { get; set; }
    }
}
