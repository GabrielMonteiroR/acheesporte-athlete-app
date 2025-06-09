using acheesporte_athlete_app.Dtos;
using acheesporte_athlete_app.Dtos.GooglePlaces;
using acheesporte_athlete_app.Dtos.Venues;
using acheesporte_athlete_app.Interfaces;
using acheesporte_athlete_app.Services;
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
        private readonly VenueTypeService _venueTypeService;

        public ObservableCollection<Pin> VenuePins { get; } = new();
        public ObservableCollection<Prediction> Suggestions { get; } = new();
        public ObservableCollection<VenueTypeDto> VenueTypes { get; } = new();

        [ObservableProperty] private bool isLoading;
        [ObservableProperty] private bool isModalVisible;
        [ObservableProperty] private VenueDto? selectedVenue;
        [ObservableProperty] private int selectedImageIndex;
        [ObservableProperty] private string? currentImageUrl;
        [ObservableProperty] private VenueTypeDto? selectedVenueType;
        [ObservableProperty] private bool isFilterModalVisible;

        public string? SportsString => SelectedVenue?.Sports != null
            ? string.Join(", ", SelectedVenue.Sports)
            : null;

        public SelectVenueMapViewModel(
            IVenueService venueService,
            IGooglePlacesService googlePlacesService,
            VenueTypeService venueTypeService)
        {
            _venueService = venueService;
            _googlePlacesService = googlePlacesService;
            _venueTypeService = venueTypeService;
        }

        partial void OnSelectedVenueChanged(VenueDto? value)
        {
            SelectedImageIndex = 0;
            CurrentImageUrl = value?.ImageUrls?.FirstOrDefault();
            OnPropertyChanged(nameof(SportsString));
        }

        partial void OnSelectedImageIndexChanged(int value)
        {
            if (SelectedVenue?.ImageUrls?.Any() != true) return;
            CurrentImageUrl = SelectedVenue.ImageUrls[value];
        }

        [RelayCommand]
        private void NextImage()
        {
            var images = SelectedVenue?.ImageUrls;
            if (images == null || images.Count == 0)
                return;

            SelectedImageIndex = (SelectedImageIndex + 1) % images.Count;
        }

        [RelayCommand]
        private void PreviousImage()
        {
            var images = SelectedVenue?.ImageUrls;
            if (images == null || images.Count == 0)
                return;

            SelectedImageIndex = (SelectedImageIndex - 1 + images.Count) % images.Count;
        }

        [RelayCommand]
        public async Task LoadVenueTypesAsync()
        {
            try
            {
                var allVenues = await _venueService.GetVenuesAsync();
                var venueTypesWithVenues = allVenues
                    .GroupBy(v => v.VenueTypeId)
                    .Select(g => g.First().VenueTypeName)
                    .ToHashSet();

                var response = await _venueTypeService.GetVenueTypesAsync();
                VenueTypes.Clear();

                foreach (var type in response.VenueTypesList)
                {
                    if (venueTypesWithVenues.Contains(type.Name))
                        VenueTypes.Add(type);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", $"Erro ao carregar tipos de locais: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        public async Task ApplyFilterAsync()
        {
            if (SelectedVenueType == null)
            {
                await Shell.Current.DisplayAlert("Atenção", "Selecione um tipo de local antes de aplicar o filtro.", "OK");
                return;
            }

            IsLoading = true;

            try
            {
                var filteredVenues = await _venueService.GetVenuesAsync(
                    venueTypeId: SelectedVenueType.Id,
                    isReserved: false
                );

                VenuePins.Clear();
                SelectedVenue = null;

                foreach (var venue in filteredVenues)
                {
                    var pin = new Pin
                    {
                        Label = venue.Name,
                        Address = venue.Address,
                        Location = new Location(venue.Latitude, venue.Longitude),
                        Type = PinType.Place
                    };

                    pin.MarkerClicked += (s, e) =>
                    {
                        SelectedVenue = venue;
                        IsModalVisible = true;
                    };

                    VenuePins.Add(pin);
                }

                MessagingCenter.Send(this, "UpdatePins", VenuePins.ToList());
                IsFilterModalVisible = false;

                if (!filteredVenues.Any())
                {
                    await Shell.Current.DisplayAlert("Nenhum local encontrado", "Não há locais disponíveis com os filtros aplicados.", "OK");
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

                    pin.MarkerClicked += (s, e) =>
                    {
                        SelectedVenue = venue;
                        IsModalVisible = true;
                    };

                    VenuePins.Add(pin);
                }

                MessagingCenter.Send(this, "UpdatePins", VenuePins.ToList());
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

        [RelayCommand]
        public void CloseModal()
        {
            IsModalVisible = false;
            SelectedVenue = null;
        }

        [RelayCommand]
        public void ConfirmVenue()
        {
            Shell.Current.DisplayAlert("Selecionado", SelectedVenue?.Name ?? "", "OK");
            IsModalVisible = false;
        }

        [RelayCommand]
        public void OpenFilterModal()
        {
            IsFilterModalVisible = true;
        }

        [RelayCommand]
        public void CloseFilterModal()
        {
            IsFilterModalVisible = false;
        }
    }
}
