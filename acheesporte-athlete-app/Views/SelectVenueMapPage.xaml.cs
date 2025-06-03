using acheesporte_athlete_app.ViewModels.Venue;
using Microsoft.Maui.Controls.Maps;
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

            await _viewModel.LoadVenuesAsync();

            VenueMap.Pins.Clear();

            foreach (var pin in _viewModel.VenuePins)
                VenueMap.Pins.Add(pin);

            if (_viewModel.VenuePins.Count > 0)
            {
                var firstLocation = _viewModel.VenuePins[0].Location;
                VenueMap.MoveToRegion(MapSpan.FromCenterAndRadius(firstLocation, Distance.FromKilometers(5)));
            }
        }
    }
}
