using acheesporte_athlete_app.Dtos.ReservationDtos;
using acheesporte_athlete_app.Interfaces;
using acheesporte_athlete_app.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace acheesporte_athlete_app.ViewModels;

public partial class ReservationViewModel : ObservableObject
{
    private readonly IReservationService _reservationService;

    public ReservationViewModel(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [ObservableProperty]
    private ObservableCollection<ReservationDto> reservations = new();

    [ObservableProperty]
    private bool isBusy;

    public bool IsNotBusy => !IsBusy;

    [RelayCommand]
    private async Task LoadReservationsAsync()
    {
        if (IsBusy || UserSession.CurrentUser == null)
            return;

        try
        {
            IsBusy = true;
            Reservations.Clear();

            int userId = UserSession.CurrentUser.Id;

            var response = await _reservationService.GetReservationsByUserAsync(new ReservationRequestDto
            {
                UserId = userId,
                Status = null
            });

            if (response?.Reservations == null || response.Reservations.Count == 0)
                return;

            var filtered = response.Reservations
                .Where(r => r.Status >= 1 && r.Status <= 3)
                .OrderBy(r => r.StartDate)
                .ToList();

            foreach (var r in filtered)
                Reservations.Add(r);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsNotBusy)); 
        }
    }
}
