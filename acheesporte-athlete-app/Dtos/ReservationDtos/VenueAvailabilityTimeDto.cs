using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.Dtos.ReservationDtos;

public record VenueAvailabilityTimeDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("start_date")] DateTime StartDate,
    [property: JsonPropertyName("end_date")] DateTime EndDate,
    [property: JsonPropertyName("price")] decimal Price,
    [property: JsonPropertyName("is_reserved")] bool IsReserved,
    [property: JsonPropertyName("venue_id")] int VenueId
);
