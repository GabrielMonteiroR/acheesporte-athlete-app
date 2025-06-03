using acheesporte_athlete_app.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            using var stream = assembly.GetManifestResourceStream("AcheesporteAppAthlete.appsettings.json");
            if (stream == null)
                throw new InvalidOperationException("Embedded resource 'AcheesporteAppAthlete.appsettings.json' not found.");
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();

            builder.Configuration.AddConfiguration(config);
            builder.Services.Configure<ApiSettings>(config.GetSection("ApiSettings"));
            builder.Services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<ApiSettings>>().Value);





            builder
                .UseMauiApp<App>()
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
