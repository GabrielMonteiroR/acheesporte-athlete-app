using acheesporte_athlete_app.Dtos.User;
using acheesporte_athlete_app.Interfaces;
using acheesporte_athlete_app.Views;
using AcheesporteAppAthlete.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace acheesporte_athlete_app.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly IImageService _imageService;
    private readonly IUserService _userService;

    public RegisterViewModel(IImageService imageService, IUserService userService)
    {
        _imageService = imageService;
        _userService = userService;
    }

    [ObservableProperty]
    private string firstName = string.Empty;

    [ObservableProperty]
    private string lastName = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string phone = string.Empty;

    [ObservableProperty]
    private string cpf = string.Empty;

    [ObservableProperty]
    private string profileImageUrl = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    public bool IsNotBusy => !IsBusy;

    [RelayCommand]
    private async Task NavigateToLoginAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            await Shell.Current.GoToAsync("loading?message=Voltando...&redirect=//LoginPage");
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

    [RelayCommand]
    private async Task PickImageAsync()
    {
        var file = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Escolha uma imagem de perfil"
        });

        if (file == null) return;

        var response = await _imageService.UploadProfileImageAsync(file);
        if (!string.IsNullOrEmpty(response?.Image))
        {
            ProfileImageUrl = response.Image;
        }
        else
        {
            await Shell.Current.DisplayAlert("Erro", "Falha ao enviar imagem", "OK");
        }
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Phone) ||
                string.IsNullOrWhiteSpace(Cpf) ||
                string.IsNullOrWhiteSpace(ProfileImageUrl))
            {
                await Shell.Current.DisplayAlert("Atenção", "Preencha todos os campos.", "OK");
                return;
            }

            var dto = new RegisterRequestDto
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password,
                Phone = Phone,
                Cpf = Cpf,
                ProfileImageUrl = ProfileImageUrl
            };

            await _userService.SignInUpUserAsync(dto);

            var loginDto = new LoginRequestDto
            {
                Email = Email,
                Password = Password
            };

            await _userService.SignInUserAsync(loginDto);

            var currentUser = await _userService.GetCurrentUserAsync();
            UserSession.CurrentUser = currentUser;

            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//MainApp/HomePage");
        }
        catch (Exception ex)
        {
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            await Shell.Current.DisplayAlert("Erro", $"Erro do servidor: {errorMessage}", "OK");
            await Shell.Current.GoToAsync("//RegisterPage");
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }
}
