using acheesporte_athlete_app.Dtos.ReservationDtos;
using acheesporte_athlete_app.Dtos.Venues;
using acheesporte_athlete_app.Helpers;
using acheesporte_athlete_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VenueDto = acheesporte_athlete_app.Dtos.Venues.VenueDto;

namespace acheesporte_athlete_app.ViewModels;

[QueryProperty(nameof(Venue), "Venue")]
public partial class ReservationCreateViewModel : ObservableObject
{
    // ──────────────────── DI ────────────────────
    private readonly IReservationService _reservationService;
    private readonly IVenueService _venueService;

    public ReservationCreateViewModel(
        IReservationService reservationService,
        IVenueService venueService)
    {
        _reservationService = reservationService;
        _venueService = venueService;
    }

    // ──────────────────── Bindables ────────────────────
    [ObservableProperty] private VenueDto venue;

    /* ⇢ Etapa 1 – Datas ------------------------------------------------ */
    public ObservableCollection<DateTime> AvailableDates { get; } = new();
    [ObservableProperty] private DateTime? selectedDate;

    /* ⇢ Etapa 2 – Horários -------------------------------------------- */
    public ObservableCollection<VenueAvailabilityDto> DayAvailabilities { get; } = new();
    [ObservableProperty] private VenueAvailabilityDto? selectedAvailability;

    /* ⇢ Submissão ------------------------------------------------------ */
    [ObservableProperty] private bool isSubmitting;
    [ObservableProperty] private string? errorMessage;

    private IReadOnlyList<VenueAvailabilityDto> _allTimes = Array.Empty<VenueAvailabilityDto>();

    // ──────────────────── Lifecycle ────────────────────
    public async Task InitializeAsync()
    {
        // Busca todos os horários (somente não-reservados)
        _allTimes = await _venueService.GetAvailableTimesByVenueIdAsync(Venue.Id);

        // Popula o Picker de datas
        var distinctDays = _allTimes
                           .Where(t => !t.IsReserved)
                           .Select(t => t.StartDate.Date)
                           .Distinct()
                           .OrderBy(d => d);

        AvailableDates.Clear();
        foreach (var d in distinctDays)
            AvailableDates.Add(d);

        // Se houver apenas um dia, já o seleciona para agilizar
        if (AvailableDates.Count == 1)
            SelectedDate = AvailableDates[0];
    }

    /* Toda vez que o usuário escolhe uma data,
       filtramos os horários daquela data.                                  */
    partial void OnSelectedDateChanged(DateTime? value)
    {
        DayAvailabilities.Clear();
        SelectedAvailability = null;

        if (value is null) return;

        foreach (var t in _allTimes
                         .Where(t => t.StartDate.Date == value.Value.Date && !t.IsReserved)
                         .OrderBy(t => t.StartDate))
        {
            DayAvailabilities.Add(t);
        }
    }

    // ──────────────────── Command de Submit ────────────────────
    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (IsSubmitting || SelectedAvailability is null) return;

        try
        {
            IsSubmitting = true;
            ErrorMessage = null;

            var user = UserSession.CurrentUser;
            if (user == null)
            {
                ErrorMessage = "Usuário não autenticado.";
                return;
            }

            var dto = new CreateReservationDto
            {
                UserId = user.Id,
                VenueId = Venue.Id,
                VenueAvailabilityTimeId = SelectedAvailability.Id,
                PaymentMethodId = 1
            };

            var created = await _reservationService.CreateReservationAsync(dto);

            if (created != null)
            {
                await Shell.Current.DisplayAlert("Sucesso", "Reserva realizada!", "OK");
                await Shell.Current.GoToAsync($"pix-payment?reservationId={created.Id}");
            }
            else
            {
                ErrorMessage = "Falha ao criar reserva.";
            }
        }

        catch (Exception ex) { ErrorMessage = ex.Message; }
        finally { IsSubmitting = false; }
    }
}
