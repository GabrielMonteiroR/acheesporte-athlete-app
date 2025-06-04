using acheesporte_athlete_app.ViewModels.Venue;
using Microsoft.Maui.Maps;

namespace acheesporte_athlete_app.Views
{
    public partial class SelectVenueMapPage : ContentPage
    {
        private readonly SelectVenueMapViewModel _viewModel;

        public SelectVenueMapPage(SelectVenueMapViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (location == null)
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(10)
                    });

                if (location != null)
                {
                    var userPosition = new Location(location.Latitude, location.Longitude);
                    VenueMap.MoveToRegion(MapSpan.FromCenterAndRadius(userPosition, Distance.FromKilometers(3)));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao obter localização: {ex.Message}", "OK");
            }

            await _viewModel.LoadVenuesAsync();

            VenueMap.Pins.Clear();

            foreach (var pin in _viewModel.VenuePins)
                VenueMap.Pins.Add(pin);
        }
    }
}
