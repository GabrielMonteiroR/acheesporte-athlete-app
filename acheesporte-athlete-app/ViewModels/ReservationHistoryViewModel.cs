using acheesporte_athlete_app.Dtos.ReservationDtos;
using acheesporte_athlete_app.Helpers;
using acheesporte_athlete_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.ViewModels;

public partial class ReservationHistoryViewModel : ObservableObject
{
    private readonly IReservationService _service;

    public ObservableCollection<ReservationDto> History { get; } = [];
    [ObservableProperty] private bool isLoading;

    public ReservationHistoryViewModel(IReservationService service) => _service = service;

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            History.Clear();

            var userId = UserSession.CurrentUser!.Id;
            var dto = await _service.GetHistoryByUserAsync(userId);

            foreach (var res in dto.Reservations)
                History.Add(res);
        }
        finally { IsLoading = false; }
    }
}

