
using acheesporte_athlete_app.Views;

namespace acheesporte_athlete_app;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("LoginPage", typeof(LoginPage));
        Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
        Routing.RegisterRoute("TestPage", typeof(TestPage));

        // Routing.RegisterRoute("loading", typeof(LoadingPage));
        Routing.RegisterRoute("HomePage", typeof(HomePage));
        // Routing.RegisterRoute("HistoryPage", typeof(Views.History.HistoryPage));
        // Routing.RegisterRoute("ReservationPage", typeof(ReservationPage));

    }
}