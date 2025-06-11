using acheesporte_athlete_app.ViewModels;

namespace acheesporte_athlete_app.Views;

public partial class LoginPage : ContentPage
{
    private bool isLoginSelected = true;

    public LoginPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetService<LoginViewModel>();
        SetToggleVisual();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        isLoginSelected = true;
        SetToggleVisual();
    }


    private void OnLoginToggleClicked(object sender, EventArgs e)
    {
        if (!isLoginSelected)
        {
            isLoginSelected = true;
            SetToggleVisual();
        }
    }

    private void SetToggleVisual()
    {
        if (isLoginSelected)
        {
            LoginToggleButton.BackgroundColor = Color.FromArgb("#200937");
            LoginToggleButton.TextColor = Colors.White;

            RegisterToggleButton.BackgroundColor = Colors.Transparent;
            RegisterToggleButton.TextColor = Color.FromArgb("#200937");
        }
        else
        {
            RegisterToggleButton.BackgroundColor = Color.FromArgb("#200937");
            RegisterToggleButton.TextColor = Colors.White;

            LoginToggleButton.BackgroundColor = Colors.Transparent;
            LoginToggleButton.TextColor = Color.FromArgb("#200937");
        }
    }

}