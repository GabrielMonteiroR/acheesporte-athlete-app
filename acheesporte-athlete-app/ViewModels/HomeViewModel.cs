using acheesporte_athlete_app.Helpers;
using acheesporte_athlete_app.Interfaces;
using acheesporte_athlete_app.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class HomeViewModel : ObservableObject
{
    private readonly IUserService _userService;

    [ObservableProperty] private string userName = "";
    [ObservableProperty] private string profileImageUrl = "";

    public HomeViewModel(IUserService userService)
        => _userService = userService;

    [RelayCommand]
    private async Task NavigateToMapAsync()
    {
        await Shell.Current.GoToAsync(nameof(SelectVenueMapPage));
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
