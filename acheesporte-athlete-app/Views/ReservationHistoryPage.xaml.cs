using acheesporte_athlete_app.ViewModels;

namespace acheesporte_athlete_app.Views;

public partial class ReservationHistoryPage : ContentPage
{
    public ReservationHistoryPage(ReservationHistoryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var vm = (ReservationHistoryViewModel)BindingContext;
        if (!vm.History.Any())
            await vm.LoadCommand.ExecuteAsync(null);
    }
}
