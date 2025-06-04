using acheesporte_athlete_app.Dtos.GooglePlaces;
using acheesporte_athlete_app.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.ViewModels.Venue
{
    public partial class SelectVenueMapViewModel : ObservableObject
    {
        private readonly IVenueService _venueService;
        private readonly IGooglePlacesService _googlePlacesService;

        public ObservableCollection<Pin> VenuePins { get; } = new();
        public ObservableCollection<Prediction> Suggestions { get; } = new();

        [ObservableProperty]
        private bool isLoading;

        public SelectVenueMapViewModel(IVenueService venueService, IGooglePlacesService googlePlacesService)
        {
            _venueService = venueService;
            _googlePlacesService = googlePlacesService;
        }

        [RelayCommand]
        public async Task ApplyFilterAsync()
        {
            IsLoading = true;
            try
            {
                var venues = await _venueService.GetVenuesAsync(
                    venueTypeId: 1, 
                    minCapacity: 10,
                    maxCapacity: 100,
                    name: null,
                    address: null
                );

                VenuePins.Clear();
                foreach (var venue in venues)
                {
                    var pin = new Pin
                    {
                        Label = venue.Name,
                        Address = venue.Address,
                        Location = new Location(venue.Latitude, venue.Longitude),
                        Type = PinType.Place
                    };
                    VenuePins.Add(pin);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }


        [RelayCommand]
        public async Task LoadVenuesAsync()
        {
            IsLoading = true;
            try
            {
                var venues = await _venueService.GetVenuesAsync();
                VenuePins.Clear();

                foreach (var venue in venues)
                {
                    var pin = new Pin
                    {
                        Label = venue.Name,
                        Address = venue.Address,
                        Location = new Location(venue.Latitude, venue.Longitude),
                        Type = PinType.Place
                    };
                    VenuePins.Add(pin);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task SearchPlacesAsync(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Suggestions.Clear();
                return;
            }

            var results = await _googlePlacesService.GetAutocompleteSuggestionsAsync(input);
            Suggestions.Clear();
            foreach (var item in results.Take(5))
                Suggestions.Add(item);
        }

        [RelayCommand]
        public async Task SelectSuggestionAsync(Prediction selected)
        {
            Suggestions.Clear();
            var location = await _googlePlacesService.GetPlaceLocationAsync(selected.PlaceId);
            if (location != null)
            {
                var mapSpan = MapSpan.FromCenterAndRadius(
                    new Location(location.Lat, location.Lng),
                    Distance.FromKilometers(2));

                MessagingCenter.Send(this, "CenterMap", mapSpan);
            }
        }
    }
}
