using acheesporte_athlete_app.Dtos;
using acheesporte_athlete_app.Helpers;
using acheesporte_athlete_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace acheesporte_athlete_app.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly IImageService _imageInterface;
    private readonly IUserService _userInterface;

    public RegisterViewModel(IImageService imageService, IUserService userService)
    {
        _imageInterface = imageService;
        _userInterface = userService;
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
    private string profileImageUrl = "placeholder_profile.png";

    [ObservableProperty]
    private bool isBusy;

    public bool IsNotBusy => !IsBusy;

    partial void OnProfileImageUrlChanged(string oldValue, string newValue)
    {
        if (string.IsNullOrWhiteSpace(newValue))
            ProfileImageUrl = "placeholder_profile.png";
    }

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

        var response = await _imageInterface.UploadProfileImageAsync(file);
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

            var dto = new SignUpRequestDto
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password,
                Phone = Phone,
                Cpf = Cpf,
                ProfileImageUrl = ProfileImageUrl
            };

            await _userInterface.SignUpUserAsync(dto);

            var loginDto = new SignInRequestDto
            {
                Email = Email,
                Password = Password
            };

            await _userInterface.SignInUserAsync(loginDto);

            var currentUser = await _userInterface.GetCurrentUserAsync();
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