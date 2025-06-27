using acheesporte_athlete_app.Dtos.ReservationDtos;
using acheesporte_athlete_app.Helpers;
using acheesporte_athlete_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using System.Collections.ObjectModel;

namespace acheesporte_athlete_app.ViewModels;

public partial class MyReservationsViewModel : ObservableObject
{
    private readonly IReservationService _service;

    public ObservableCollection<ReservationDto> Reservations { get; } = [];

    [ObservableProperty] private bool isLoading;

    public MyReservationsViewModel(IReservationService service) => _service = service;

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Reservations.Clear();

            var userId = UserSession.CurrentUser!.Id;
            var dto = await _service.GetReservationsByUserAsync(userId);

            foreach (var res in dto.Reservations)
                Reservations.Add(res);
        }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task OpenRouteAsync(ReservationDto? reservation)
    {
        if (reservation?.Venue is null) return;

        var lat = reservation.Venue.Latitude;
        var lng = reservation.Venue.Longitude;
        var uri = new Uri($"https://www.google.com/maps/dir/?api=1&destination={lat},{lng}&travelmode=driving");

        await Launcher.Default.OpenAsync(uri);
    }
}
