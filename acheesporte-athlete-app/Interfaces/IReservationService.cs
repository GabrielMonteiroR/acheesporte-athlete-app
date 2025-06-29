using acheesporte_athlete_app.Dtos.ReservationDtos;

namespace acheesporte_athlete_app.Interfaces;

public interface IReservationService
{
    Task<ReservationsByUserResponseDto> GetReservationsByUserAsync(int userId);
    Task<ReservationDto?> CreateReservationAsync(CreateReservationDto dto);
    Task<ReservationsByUserResponseDto> GetHistoryByUserAsync(int userId);
    Task<ReservationsByUserResponseDto> GetNextReservationByUserAsync(int userId);
    Task<StreakDto?> GetUserStreakAsync(int userId);
}

