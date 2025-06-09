using acheesporte_athlete_app.Dtos.User;
using acheesporte_athlete_app.Interfaces;
using AcheesporteAppAthlete.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using acheesporte_athlete_app.Views;

namespace acheesporte_athlete_app.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IUserService _userService;

    public LoginViewModel(IUserService userService)
    {
        _userService = userService;
    }

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private bool isBusy;

    public bool IsNotBusy => !IsBusy;

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Atenção", "Preencha o e-mail e a senha.", "OK");
                return;
            }

            var dto = new LoginRequestDto
            {
                Email = Email,
                Password = Password
            };

            await _userService.SignInUserAsync(dto);

            var currentUser = await _userService.GetCurrentUserAsync();
            UserSession.CurrentUser = currentUser;

            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("loading?message=Entrando...&redirect=//MainApp/HomePage");
        }
        catch
        {
            await Shell.Current.DisplayAlert("Erro", "Email ou senha inválidos", "OK");
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }

    [RelayCommand]
    private async Task NavigateToRegisterAsync()
    {
        if (IsBusy)
            return;

        IsBusy = true;

        try
        {
            await Application.Current.MainPage.Navigation.PushAsync(App.Services.GetService<RegisterPage>());

        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }
}
