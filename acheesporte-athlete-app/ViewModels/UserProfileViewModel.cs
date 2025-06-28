using acheesporte_athlete_app.Services;
using acheesporte_athlete_app.Dtos.Users;
using acheesporte_athlete_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace acheesporte_athlete_app.ViewModels;

public partial class UserProfileViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly IImageService _imageService;

    public UserProfileViewModel(IUserService userService, IImageService imageService)
    {
        _userService = userService;
        _imageService = imageService;
    }

    [ObservableProperty]
    private int userId;

    [ObservableProperty]
    private string firstName;

    [ObservableProperty]
    private string lastName;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string phone;

    [ObservableProperty]
    private string profileImageUrl;

    [ObservableProperty]
    private bool isLoading;

    [RelayCommand]
    public async Task LoadUserAsync()
    {
        try
        {
            IsLoading = true;

            var currentUser = await _userService.GetCurrentUserAsync();
            UserId = currentUser.Id;

            var user = await _userService.GetUserByIdAsync(UserId);

            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Phone = user.Phone;
            ProfileImageUrl = user.ProfileImageUrl;

            await Shell.Current.DisplayAlert("Usuário", $"Bem-vindo, {user.FirstName}", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar dados do usuário: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ChangeProfileImageAsync()
    {
        try
        {
            var pick = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Selecione sua nova foto"
            });
            if (pick == null) return;

            IsLoading = true;

            var upload = await _imageService.UploadProfileImageAsync(pick);
            var newUrl = upload.Image;

            var updated = await _userService.UpdateUserProfileImageAsync(UserId, newUrl);

            ProfileImageUrl = updated.ProfileImageUrl ?? newUrl;

            await Shell.Current.DisplayAlert("Sucesso",
                "Foto de perfil atualizada!", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        try
        {
            IsLoading = true;

            var updateDto = new UpdateUserRequestDto
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Phone = Phone
            };

            var updatedUser = await _userService.UpdateUserAsync(UserId, updateDto);

            await Shell.Current.DisplayAlert("Sucesso", "Dados atualizados com sucesso!", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", $"Erro ao atualizar dados: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }
}