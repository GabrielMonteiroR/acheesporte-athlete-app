using AcheesporteAppAthlete.Helpers;
using acheesporte_athlete_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IUserService _userService;

    [ObservableProperty]
    private string userName;

    [ObservableProperty]
    private string profileImageUrl;

    public HomeViewModel(IUserService userService)
    {
        _userService = userService;
    }

    [RelayCommand]
    public async Task LoadUserAsync()
    {
        try
        {
            var user = await _userService.GetCurrentUserAsync();
            UserSession.CurrentUser = user;
            UserName = user.FirstName;
            ProfileImageUrl = string.IsNullOrWhiteSpace(user.ProfileImage)
                ? "profile.png"
                : user.ProfileImage;
        }
        catch
        {
            UserName = "Atleta";
            ProfileImageUrl = "profile.png";
        }
    }
}
