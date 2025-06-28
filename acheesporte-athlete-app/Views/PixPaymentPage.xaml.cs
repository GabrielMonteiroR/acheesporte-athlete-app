using acheesporte_athlete_app.ViewModels;

namespace acheesporte_athlete_app.Views;

public partial class PixPaymentPage : ContentPage
{
    public PixPaymentPage(PixPaymentViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PixPaymentViewModel vm)
            await vm.InitializeAsync();
    }
}
