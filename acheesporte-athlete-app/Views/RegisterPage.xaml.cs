using acheesporte_athlete_app.ViewModels;

namespace acheesporte_athlete_app.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetService<RegisterViewModel>();
    }
}
    



