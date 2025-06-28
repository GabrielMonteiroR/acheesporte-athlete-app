using acheesporte_athlete_app.Dtos.ReservationDtos;
using acheesporte_athlete_app.Dtos.Venues;
using acheesporte_athlete_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VenueDto = acheesporte_athlete_app.Dtos.Venues.VenueDto;

namespace acheesporte_athlete_app.ViewModels;

[QueryProperty(nameof(Venue), "Venue")]
public partial class ReservationCreateViewModel : ObservableObject
{
    private readonly IReservationService _reservationService;
    private readonly IVenueService _venueService;      // ← novo

    public ReservationCreateViewModel(
        IReservationService reservationService,
        IVenueService venueService)                    // ← injeta
    {
        _reservationService = reservationService;
        _venueService = venueService;
    }

    // ────────────── propriedades bindadas ──────────────
    [ObservableProperty] private VenueDto venue;
    [ObservableProperty] private VenueAvailabilityDto? selectedAvailability;
    [ObservableProperty] private bool isSubmitting;
    [ObservableProperty] private string? errorMessage;

    public ObservableCollection<VenueAvailabilityDto> Availabilities { get; } = new();

    // chamado no code-behind (OnAppearing)
    public async Task InitializeAsync()
    {
        // Sempre puxa do endpoint dedicado
        var list = await _venueService.GetAvailableTimesByVenueIdAsync(Venue.Id);

        Availabilities.Clear();
        foreach (var t in list)
            Availabilities.Add(t);
    }

    // ────────────── confirmar reserva ──────────────
    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (IsSubmitting || SelectedAvailability is null) return;

        try
        {
            IsSubmitting = true;
            ErrorMessage = null;

            var userIdStr = await SecureStorage.GetAsync("user_id");
            if (!int.TryParse(userIdStr, out var userId))
            {
                ErrorMessage = "Erro ao obter ID do usuário.";
                return;
            }

            var dto = new CreateReservationDto
            {
                UserId = userId,
                VenueId = Venue.Id,
                VenueAvailabilityTimeId = SelectedAvailability.Id,
                PaymentMethodId = 1 //
            };

            var ok = await _reservationService.CreateReservationAsync(dto);

            if (ok)
            {
                await Shell.Current.DisplayAlert("Sucesso", "Reserva realizada!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                ErrorMessage = "Falha ao criar reserva.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro: {ex.Message}";
        }
        finally { IsSubmitting = false; }
    }
}
