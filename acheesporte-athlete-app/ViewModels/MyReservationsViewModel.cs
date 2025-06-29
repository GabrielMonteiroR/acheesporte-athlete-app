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

    /* ───────── carregar reservas ───────── */
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

            foreach (var r in dto.Reservations)
                Reservations.Add(r);
        }
        finally { IsLoading = false; }
    }

    [RelayCommand]
    private async Task OpenRouteAsync(ReservationDto? res)
    {
        if (res?.Venue is null) return;

        var uri = new Uri(
            $"https://www.google.com/maps/dir/?api=1&destination={res.Venue.Latitude},{res.Venue.Longitude}&travelmode=driving");

        await Launcher.Default.OpenAsync(uri);
    }

    [RelayCommand]
    private async Task ReservationActionAsync(ReservationDto? reservation)
    {
        if (reservation is null) return;

        if (reservation.IsPaid)
        {
            // Ação: Abrir rota no mapa
            var lat = reservation.Venue?.Latitude;
            var lng = reservation.Venue?.Longitude;

            if (lat != null && lng != null)
            {
                var uri = new Uri($"https://www.google.com/maps/dir/?api=1&destination={lat},{lng}&travelmode=driving");
                await Launcher.Default.OpenAsync(uri);
            }
        }
        else
        {
            // Ação: Navegar para tela de pagamento PIX
            await Shell.Current.GoToAsync($"pix-payment?reservationId={reservation.Id}");
        }
    }


    /* ───────── seguir para pagamento PIX ───────── */
    [RelayCommand]
    private Task PayAsync(ReservationDto? res) =>
        res is null || res.IsPaid
            ? Task.CompletedTask
            : Shell.Current.GoToAsync($"pix-payment?reservationId={res.Id}");
}
