using acheesporte_athlete_app.Dtos.Venue;
using acheesporte_athlete_app.Interfaces;
using Microsoft.Maui.Controls.Maps;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace acheesporte_athlete_app.ViewModels.Venue
{
    public partial class SelectVenueMapViewModel : ObservableObject
    {
        private readonly IVenueService _venueService;

        public ObservableCollection<Pin> VenuePins { get; } = new();

        [ObservableProperty]
        private bool isLoading;

        public SelectVenueMapViewModel(IVenueService venueService)
        {
            _venueService = venueService;
        }

        [RelayCommand]
        public async Task LoadVenuesAsync()
        {
            try
            {
                IsLoading = true;

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
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
