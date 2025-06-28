using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace acheesporte_athlete_app.ViewModels;

[QueryProperty(nameof(ReservationId), "ReservationId")]
public partial class PixPaymentViewModel : ObservableObject
{
    [ObservableProperty]
    private int reservationId;

    [ObservableProperty]
    private string pixCode = string.Empty;

    public PixPaymentViewModel()
    {
    }

    [RelayCommand]
    private async Task CopyCodeAsync()
    {
        if (!string.IsNullOrWhiteSpace(PixCode))
        {
            await Clipboard.SetTextAsync(PixCode);
            await Shell.Current.DisplayAlert("PIX", "Código copiado para a área de transferência!", "OK");
        }
    }

    [RelayCommand]
    private async Task NavigationBackAsync()
    {
        await Shell.Current.GoToAsync("//ReservationPage");
    }


    public async Task InitializeAsync()
    {
        PixCode = $"PIX-RESERVA-{ReservationId}";
        await Task.CompletedTask;
    }
}
