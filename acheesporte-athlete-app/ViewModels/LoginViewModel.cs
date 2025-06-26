using acheesporte_athlete_app;
using acheesporte_athlete_app.Dtos;
using acheesporte_athlete_app.Helpers;
using acheesporte_athlete_app.Interfaces;
using acheesporte_athlete_app.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace acheesporte_athlete_app.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IUserService _userInterface;

    public LoginViewModel(IUserService userService)
    {
        _userInterface = userService;
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
                await Application.Current.MainPage.DisplayAlert("Atenção", "Preencha o e-mail e a senha.", "OK");
                return;
            }

            var dto = new SignInRequestDto
            {
                Email = Email,
                Password = Password
            };

            await _userInterface.SignInUserAsync(dto);

            var currentUser = await _userInterface.GetCurrentUserAsync();

            if (currentUser == null)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possível carregar os dados do usuário.", "OK");
                return;
            }

            UserSession.CurrentUser = currentUser;


            Application.Current.MainPage = App.Services.GetService<AppShell>();
            await Shell.Current.GoToAsync("//MainApp/HomePage");
        }
        catch
        {
            await Application.Current.MainPage.DisplayAlert("Erro", "Email ou senha inválidos", "OK");
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
            await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");

        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }
}