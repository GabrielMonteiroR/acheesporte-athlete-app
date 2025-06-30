using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace acheesporte_athlete_app.ViewModels;

[QueryProperty(nameof(ReservationId), "reservationId")]
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
        await Shell.Current.GoToAsync("//HomePage");
    }


    public async Task InitializeAsync()
    {
        PixCode = $"00020126360014BR.GOV.BCB.PIX0114acheesporte.com-res-5204000053039865406100.005802BR5921AcheEsporte Ltda6011São Paulo6304ABCD-{ReservationId}";
        await Task.CompletedTask;
    }
}
