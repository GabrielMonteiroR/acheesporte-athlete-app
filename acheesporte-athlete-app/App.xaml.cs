using acheesporte_athlete_app.Views;
using acheesporte_athlete_app.ViewModels.Venue;
using acheesporte_athlete_app.Services;

namespace acheesporte_athlete_app
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            var viewModel = serviceProvider.GetRequiredService<SelectVenueMapViewModel>();

            MainPage = new SelectVenueMapPage(viewModel);
        }
    }

}
