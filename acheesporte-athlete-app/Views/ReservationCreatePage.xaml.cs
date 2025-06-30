using acheesporte_athlete_app.Dtos.Venues;
using acheesporte_athlete_app.ViewModels;

namespace acheesporte_athlete_app.Views;

public partial class ReservationCreatePage : ContentPage
{
    public ReservationCreatePage(ReservationCreateViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ReservationCreateViewModel vm)
            await vm.InitializeAsync();  
    }
}

