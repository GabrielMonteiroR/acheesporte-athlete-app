using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.Dtos.ReservationDtos;
    public record ReservationUserDto(
        [property: JsonPropertyName("first_name")] string FirstName,
        [property: JsonPropertyName("last_name")] string LastName,
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("phone")] string Phone,
        [property: JsonPropertyName("profile_image_url")] string ProfileImageUrl
    );

