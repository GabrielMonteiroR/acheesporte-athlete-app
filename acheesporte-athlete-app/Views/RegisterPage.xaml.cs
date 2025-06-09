namespace acheesporte_athlete_app.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetService<RegisterViewModel>();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        if (BindingContext is RegisterViewModel vm)
            await vm.ExecuteRegisterAsync();
    }
}