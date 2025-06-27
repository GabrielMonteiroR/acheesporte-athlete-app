using acheesporte_athlete_app.ViewModels;

namespace acheesporte_athlete_app.Views;

public partial class ReservationPage : ContentPage
{
    public ReservationPage(MyReservationsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var vm = (MyReservationsViewModel)BindingContext;
        if (!vm.Reservations.Any())
            await vm.LoadCommand.ExecuteAsync(null);
    }
}
