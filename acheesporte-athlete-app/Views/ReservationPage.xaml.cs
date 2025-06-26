using acheesporte_athlete_app.ViewModels;

namespace acheesporte_athlete_app.Views;

public partial class ReservationPage : ContentPage
{
    public ReservationPage(ReservationViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ReservationViewModel vm && vm.Reservations.Count == 0)
        {
            await vm.LoadReservationsCommand.ExecuteAsync(null);
        }
    }
}
