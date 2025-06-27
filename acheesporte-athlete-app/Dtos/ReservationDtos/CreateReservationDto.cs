namespace acheesporte_athlete_app.Dtos.ReservationDtos;

public class CreateReservationDto
{
    public int UserId { get; set; }
    public int VenueId { get; set; }
    public int VenueAvailabilityTimeId { get; set; }
    public int PaymentMethodId { get; set; } = 1;
}
