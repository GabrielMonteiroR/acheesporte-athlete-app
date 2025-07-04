﻿

using acheesporte_athlete_app.Configuration;
using acheesporte_athlete_app.Interfaces;
using acheesporte_athlete_app.Services;
using acheesporte_athlete_app.ViewModels;
using acheesporte_athlete_app.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace acheesporte_athlete_app
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("acheesporte_athlete_app.appsettings.json");
            if (stream == null)
                throw new InvalidOperationException("Embedded resource 'AcheesporteAppAthlete.appsettings.json' not found.");
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();

            builder.Configuration.AddConfiguration(config);
            builder.Services.Configure<ApiSettings>(config.GetSection("ApiSettings"));
            builder.Services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<ApiSettings>>().Value);

            builder.Services.AddTransient<SelectVenueMapPage>();
            builder.Services.AddTransient<SelectVenueMapViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<SplashPage>();
            builder.Services.AddTransient<ReservationPage>();
            builder.Services.AddTransient<MyReservationsViewModel>();
            builder.Services.AddTransient<ReservationCreatePage>();
            builder.Services.AddTransient<ReservationCreateViewModel>();
            builder.Services.AddTransient<PixPaymentPage>();
            builder.Services.AddTransient<PixPaymentViewModel>();
            builder.Services.AddTransient<UserProfilePage>();
            builder.Services.AddTransient<UserProfileViewModel>();
            builder.Services.AddTransient<ReservationHistoryPage>();
            builder.Services.AddTransient<ReservationHistoryViewModel>();


            builder.Services.AddHttpClient<IReservationService, ReservationService>(client =>
            {
                var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });

            builder.Services.AddHttpClient<ISportService, SportService>(client =>
            {
                var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });

            builder.Services.AddHttpClient<IVenueService, VenueService>(client =>
            {
                var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });

            builder.Services.AddHttpClient<IUserService, UserService>(client =>
            {
                var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });

            builder.Services.AddHttpClient<IGooglePlacesService, GooglePlacesService>(client =>
            {
                var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });

            builder.Services.AddHttpClient<IImageService, ImageService>(client =>
            {
                var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });

            builder.Services.AddHttpClient<VenueTypeService>(client =>
            {
                var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
            });

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
