using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.Dtos.ReservationDtos;

public class ReservationsByUserResponseDto
{
    [JsonPropertyName("message")] public string Message { get; set; } = "";
    [JsonPropertyName("reservations")] public List<ReservationDto> Reservations { get; set; } = [];
}
