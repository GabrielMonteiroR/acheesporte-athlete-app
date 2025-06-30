using acheesporte_athlete_app.ViewModels;

namespace acheesporte_athlete_app.Views;

public partial class LoginPage : ContentPage
{
    private bool isLoginSelected = true;

    public LoginPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetService<LoginViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        isLoginSelected = true;
    }
}