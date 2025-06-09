using acheesporte_athlete_app.Interfaces;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly IImageService _imageService;
        private readonly IUser _userService;

        public string FirstName { get => firstName; set => SetProperty(ref firstName, value); }
        public string LastName { get => lastName; set => SetProperty(ref lastName, value); }
        public string Email { get => email; set => SetProperty(ref email, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public string Phone { get => phone; set => SetProperty(ref phone, value); }
        public string Cpf { get => cpf; set => SetProperty(ref cpf, value); }
        public string ProfileImageUrl { get => profileImageUrl; set => SetProperty(ref profileImageUrl, value); }

        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string email = string.Empty;
        private string password = string.Empty;
        private string phone = string.Empty;
        private string cpf = string.Empty;
        private string profileImageUrl = string.Empty;

        public Command RegisterCommand { get; }
        public Command NavigateToLoginCommand { get; }
        public Command PickImageCommand { get; }

        public new bool IsNotBusy => !IsBusy;

        public RegisterViewModel(IImageService imageService, IUser userService)
        {
            _imageService = imageService;
            _userService = userService;

            RegisterCommand = new Command(async () => await ExecuteRegisterAsync());
            NavigateToLoginCommand = new Command(async () => await ExecuteNavigateToLoginAsync());
            PickImageCommand = new Command(async () => await PickAndUploadImageAsync());

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(IsBusy))
                    OnPropertyChanged(nameof(IsNotBusy));
            };
        }

        private async Task ExecuteNavigateToLoginAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                await Shell.Current.GoToAsync("loading?message=Voltando...&redirect=//LoginPage");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task PickAndUploadImageAsync()
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
                await Application.Current.MainPage.DisplayAlert("Erro", "Falha ao enviar imagem", "OK");
            }
        }

        public async Task ExecuteRegisterAsync()
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
                    await Application.Current.MainPage.DisplayAlert("Atenção", "Preencha todos os campos.", "OK");
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

                await _authService.RegisterAsync(dto);

                var loginDto = new LoginRequestDto
                {
                    Email = Email,
                    Password = Password
                };

                await _authService.LoginAsync(loginDto);

                var currentUser = await _userService.GetCurrentUserAsync();
                UserSession.CurrentUser = currentUser;

                Application.Current.MainPage = new AppShell();
                await Shell.Current.GoToAsync("//MainApp/HomePage");
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                await Application.Current.MainPage.DisplayAlert("Erro", $"Erro do servidor: {errorMessage}", "OK");
                await Shell.Current.GoToAsync("//RegisterPage");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
