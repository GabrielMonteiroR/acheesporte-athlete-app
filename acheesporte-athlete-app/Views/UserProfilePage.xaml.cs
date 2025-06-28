using acheesporte_athlete_app.ViewModels;
using Microsoft.Maui.Controls;

namespace acheesporte_athlete_app.Views;

public partial class UserProfilePage : ContentPage
{
    private readonly UserProfileViewModel _viewModel;

    public UserProfilePage(UserProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_viewModel.IsLoading)
        {
            await _viewModel.LoadUserAsync();
        }
    }
}