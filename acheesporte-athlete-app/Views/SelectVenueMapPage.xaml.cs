using acheesporte_athlete_app.Dtos.GooglePlaces;
using acheesporte_athlete_app.ViewModels;
using Microsoft.Maui.Controls;
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

            MessagingCenter.Subscribe<SelectVenueMapViewModel, MapSpan>(this, "CenterMap", (sender, span) =>
            {
                VenueMap.MoveToRegion(span);
            });

            MessagingCenter.Subscribe<SelectVenueMapViewModel, List<Pin>>(this, "UpdatePins", (sender, pins) =>
            {
                VenueMap.Pins.Clear();
                foreach (var pin in pins)
                    VenueMap.Pins.Add(pin);
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                await _viewModel.LoadVenueTypesAsync(); 

                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                var location = await Geolocation.GetLastKnownLocationAsync()
                              ?? await Geolocation.GetLocationAsync(new GeolocationRequest
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
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.SearchPlacesCommand.Execute(e.NewTextValue);
        }

        private void OnFilterClicked(object sender, EventArgs e)
        {
            _viewModel.OpenFilterModalCommand.Execute(null);

        }

        private void OnSuggestionSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Prediction selected)
                _viewModel.SelectSuggestionCommand.Execute(selected);

            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
