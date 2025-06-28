using acheesporte_athlete_app.ViewModels;
using System.Security.Cryptography.X509Certificates;

namespace acheesporte_athlete_app.Views;

public partial class PixPaymentPage : ContentPage
{
    public PixPaymentPage(PixPaymentViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        async void OnAppearing(object sender, EventArgs e)
        {
            var viewModel = (PixPaymentViewModel)BindingContext;
            await viewModel.InitializeAsync();
        }

    }
}
