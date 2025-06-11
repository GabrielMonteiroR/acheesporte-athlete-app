namespace acheesporte_athlete_app.Dtos.ReservationDtos;

public class ReservationRequestDto
{
    public int UserId { get; set; }
    public int? Status { get; set; } = new();
}
