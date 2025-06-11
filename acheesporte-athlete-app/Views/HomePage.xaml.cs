using acheesporte_athlete_app.ViewModels;

namespace acheesporte_athlete_app.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is HomeViewModel vm)
            await vm.LoadUserAsync();
    }
}
