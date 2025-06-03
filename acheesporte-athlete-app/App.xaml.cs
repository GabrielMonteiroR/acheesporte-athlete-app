using acheesporte_athlete_app.Views;

namespace acheesporte_athlete_app
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new SelectVenueMapPage();
        }

    }
}