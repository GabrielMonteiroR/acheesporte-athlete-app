using acheesporte_athlete_app.Dtos.ReservationDtos;
using acheesporte_athlete_app.Dtos.Venues;
using acheesporte_athlete_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VenueDto = acheesporte_athlete_app.Dtos.Venues.VenueDto;

namespace acheesporte_athlete_app.ViewModels;

[QueryProperty(nameof(Venue), "Venue")]
[QueryProperty(nameof(Availability), "Availability")]
public partial class ReservationCreateViewModel : ObservableObject
{
    private readonly IReservationService _reservationService;

    public ReservationCreateViewModel(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [ObservableProperty] private VenueDto venue;
    [ObservableProperty] private VenueAvailabilityDto availability;

    [ObservableProperty] private bool isSubmitting;
    [ObservableProperty] private string? errorMessage;

    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (IsSubmitting) return;

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
                VenueAvailabilityTimeId = Availability.Id,
                PaymentMethodId = 1 
            };

            var result = await _reservationService.CreateReservationAsync(dto);

            if (result)
            {
                await Shell.Current.DisplayAlert("Sucesso", "Reserva realizada com sucesso!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                ErrorMessage = "Erro ao criar reserva. Tente novamente.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro: {ex.Message}";
        }
        finally
        {
            IsSubmitting = false;
        }
    }
}
