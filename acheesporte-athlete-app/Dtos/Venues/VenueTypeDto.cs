using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.Dtos.Venues
{
    public class VenueTypeDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            return obj is VenueTypeDto dto && dto.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
