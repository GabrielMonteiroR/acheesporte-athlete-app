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

    /* ─── dados do usuário ─── */
    [ObservableProperty] private string userName = "";
    [ObservableProperty] private string profileImageUrl = "profile.png";

    /* ─── próxima reserva ─── */
    [ObservableProperty] private ReservationDto? nextReservation;
    [ObservableProperty] private bool isLoadingNextReservation;

    /* ─── navegação para o mapa ─── */
    [RelayCommand]
    private Task NavigateToMapAsync() =>
        Shell.Current.GoToAsync(nameof(SelectVenueMapPage));

    /* ─── carregar dados do usuário + próxima reserva ─── */
    [RelayCommand]
    public async Task LoadDataAsync()
    {
        try
        {
            /* usuário */
            var user = await _userService.GetCurrentUserAsync();
            UserSession.CurrentUser = user;

            UserName = user.FirstName;
            ProfileImageUrl = string.IsNullOrWhiteSpace(user.ProfileImage) ? "profile.png"
                                                                            : user.ProfileImage;

            /* próxima reserva  */
            IsLoadingNextReservation = true;
            var dto = await _reservationService.GetNextReservationByUserAsync(user.Id);

            NextReservation = dto.Reservations.FirstOrDefault();
        }
        catch
        {
            // fallback simples
            UserName = "Atleta";
            ProfileImageUrl = "profile.png";
            NextReservation = null;
        }
        finally
        {
            IsLoadingNextReservation = false;
        }
    }
}
