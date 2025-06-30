using acheesporte_athlete_app.Dtos.ReservationDtos;
using acheesporte_athlete_app.Helpers;
using acheesporte_athlete_app.Interfaces;
using acheesporte_athlete_app.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class HomeViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly IReservationService _reservationService;

    public HomeViewModel(IUserService userService,
                         IReservationService reservationService)
    {
        _userService = userService;
        _reservationService = reservationService;
    }

    /* ─── próxima reserva ─── */
    [ObservableProperty] private ReservationDto? nextReservation;
    [ObservableProperty] private bool isLoadingNextReservation;

    /* ─── streak ─── */
    [ObservableProperty] private string streakMessage = "Sem streak ativo no momento.";

    /* ─── navegação para o mapa ─── */
    [RelayCommand]
    private Task NavigateToMapAsync() =>
        Shell.Current.GoToAsync(nameof(SelectVenueMapPage));

    /* ─── carregar dados do usuário + próxima reserva + streak ─── */
    [RelayCommand]
    public async Task LoadDataAsync()
    {
        try
        {
            var user = await _userService.GetCurrentUserAsync();
            UserSession.CurrentUser = user;

            IsLoadingNextReservation = true;

            var dto = await _reservationService.GetNextReservationByUserAsync(user.Id);
            NextReservation = dto.Reservations.FirstOrDefault();

            var streak = await _reservationService.GetUserStreakAsync(user.Id);
            StreakMessage = streak?.Message ?? "Sem streak ativo no momento.";
        }
        catch
        {
            NextReservation = null;
            StreakMessage = "Sem streak ativo no momento.";
        }
        finally
        {
            IsLoadingNextReservation = false;
        }
    }
}
