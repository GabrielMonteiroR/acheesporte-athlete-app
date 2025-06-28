using acheesporte_athlete_app.Dtos.Venues;
using acheesporte_athlete_app.ViewModels;

namespace acheesporte_athlete_app.Views;

public partial class ReservationCreatePage : ContentPage
{
    public ReservationCreatePage(ReservationCreateViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
    }
}
