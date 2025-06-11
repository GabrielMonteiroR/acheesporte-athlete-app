using acheesporte_athlete_app.Interfaces;
using AcheesporteAppAthlete.Helpers;
using Microsoft.Maui.Controls;

namespace acheesporte_athlete_app.Views;

public partial class SplashPage : ContentPage
{
    private readonly IUserService _userService;

    public SplashPage(IUserService userService)
    {
        InitializeComponent();
        _userService = userService;

        LoadGif();
    }

    private void LoadGif()
    {
        string html = @"
<html>
  <body style='margin:0;padding:0;background:transparent;display:flex;justify-content:center;align-items:center;height:100vh;'>
    <img src='file:///android_asset/loading.gif' width='200' height='200'/>
  </body>
</html>";
        GifWebView.Source = new HtmlWebViewSource { Html = html };

    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var user = await _userService.GetCurrentUserAsync();

            if (user != null)
            {
                UserSession.CurrentUser = user;
                Application.Current.MainPage = App.Services.GetService<AppShell>();
                await Shell.Current.GoToAsync("//MainApp/HomePage");
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
        catch
        {
            await Task.Delay(1500); 
            Application.Current.MainPage = App.Services.GetService<LoginPage>();
        }
    }
}
