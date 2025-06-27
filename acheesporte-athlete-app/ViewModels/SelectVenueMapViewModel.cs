using acheesporte_athlete_app.Dtos.GooglePlaces;
using acheesporte_athlete_app.Dtos.Sports;
using acheesporte_athlete_app.Dtos.Venues;
using acheesporte_athlete_app.Interfaces;
using acheesporte_athlete_app.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;

namespace acheesporte_athlete_app.ViewModels;

public partial class SelectVenueMapViewModel : ObservableObject
{
    // ────────────── Serviços
    private readonly IVenueService _venueService;
    private readonly IGooglePlacesService _googlePlacesService;
    private readonly VenueTypeService _venueTypeService;

    // ────────────── Coleções bindadas
    public ObservableCollection<Pin> VenuePins { get; } = new();
    public ObservableCollection<Prediction> Suggestions { get; } = new();
    public ObservableCollection<VenueTypeDto> VenueTypes { get; } = new();
    public ObservableCollection<SportDto> AvailableSports { get; } = new();

    // ────────────── Propriedades para UI
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private bool isModalVisible;
    [ObservableProperty] private VenueDto? selectedVenue;
    [ObservableProperty] private int selectedImageIndex;
    [ObservableProperty] private string? currentImageUrl;
    [ObservableProperty] private VenueTypeDto? selectedVenueType;
    [ObservableProperty] private bool isFilterModalVisible;

    // Filtros escolhidos
    [ObservableProperty] private SportDto? selectedSport;
    [ObservableProperty] private DateTime? selectedDate;
    [ObservableProperty] private TimeSpan? fromTime;
    [ObservableProperty] private TimeSpan? toTime;

    public string? SportsString =>
        SelectedVenue?.Sports is { Count: > 0 }
            ? string.Join(", ", SelectedVenue.Sports!)
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

    // ────────────── Reacções automáticas
    partial void OnSelectedVenueChanged(VenueDto? value)
    {
        SelectedImageIndex = 0;
        CurrentImageUrl = value?.ImageUrls?.FirstOrDefault();
        OnPropertyChanged(nameof(SportsString));
    }

    partial void OnSelectedImageIndexChanged(int value)
    {
        if (SelectedVenue?.ImageUrls?.Any() == true)
            CurrentImageUrl = SelectedVenue.ImageUrls[value];
    }

    // ────────────── Navegação de imagens
    [RelayCommand] private void NextImage() => JumpImage(+1);
    [RelayCommand] private void PreviousImage() => JumpImage(-1);

    private void JumpImage(int delta)
    {
        if (SelectedVenue?.ImageUrls?.Count > 0)
            SelectedImageIndex =
                (SelectedImageIndex + delta + SelectedVenue.ImageUrls.Count)
                % SelectedVenue.ImageUrls.Count;
    }

    // ────────────── Tipos de local
    [RelayCommand]
    public async Task LoadVenueTypesAsync()
    {
        try
        {
            var resp = await _venueTypeService.GetVenueTypesAsync();
            VenueTypes.Clear();
            foreach (var t in resp.VenueTypesList)
                VenueTypes.Add(t);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    // ────────────── Locais (sem filtro)
    [RelayCommand] public Task LoadVenuesAsync() => FetchAndPlotVenuesAsync();

    // ────────────── Aplicar Filtro
    [RelayCommand]
    public async Task ApplyFilterAsync()
    {
        var from = Combine(selectedDate, fromTime);
        var to = Combine(selectedDate, toTime);

        await FetchAndPlotVenuesAsync(
        venueTypeId: SelectedVenueType?.Id,
        sportIds: SelectedSport != null ? new List<int> { SelectedSport.Id } : null,
        from: Combine(SelectedDate, FromTime),
        to: Combine(SelectedDate, ToTime)
    );

        IsFilterModalVisible = false;
    }

    // ────────────── Busca por texto (Google Places)
    [RelayCommand]
    public async Task SearchPlacesAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            Suggestions.Clear();
            return;
        }

        var res = await _googlePlacesService.GetAutocompleteSuggestionsAsync(text);
        Suggestions.Clear();
        foreach (var p in res.Take(5))
            Suggestions.Add(p);
    }

    [RelayCommand]
    public async Task SelectSuggestionAsync(Prediction p)
    {
        Suggestions.Clear();
        var loc = await _googlePlacesService.GetPlaceLocationAsync(p.PlaceId);
        if (loc is not null)
        {
            var span = MapSpan.FromCenterAndRadius(
                new Location(loc.Lat, loc.Lng), Distance.FromKilometers(2));

            MessagingCenter.Send(this, "CenterMap", span);
        }
    }

    // ────────────── Abertura/fecho de modais
    [RelayCommand] public void OpenFilterModal() => IsFilterModalVisible = true;
    [RelayCommand] public void CloseFilterModal() => IsFilterModalVisible = false;
    [RelayCommand] public void CloseModal() => (IsModalVisible, SelectedVenue) = (false, null);
    [RelayCommand] public void ConfirmVenue() => CloseModal();

    // ────────────── Core: busca & criação de pins
    private async Task FetchAndPlotVenuesAsync(
        int? venueTypeId = null,
        List<int>? sportIds = null,
        DateTime? from = null,
        DateTime? to = null)
    {
        IsLoading = true;

        try
        {
            var venues = await _venueService.GetVenuesAsync(
                venueTypeId: venueTypeId,
                sportId: sportIds,
                from: from,
                to: to,
                isReserved: false);

            // Pins ----------------------------------------------------------------
            VenuePins.Clear();
            foreach (var v in venues)
            {
                var pin = new Pin
                {
                    Label = v.Name,
                    Address = v.Address,
                    Location = new Location(v.Latitude, v.Longitude),
                    Type = PinType.Place
                };
                pin.MarkerClicked += (_, _) =>
                {
                    SelectedVenue = v;
                    IsModalVisible = true;
                };
                VenuePins.Add(pin);
            }
            MessagingCenter.Send(this, "UpdatePins", VenuePins.ToList());

            // Esportes (só os usados) ---------------------------------------------
            var inUse = venues
                .SelectMany(v => v.SportsObj ?? new List<SportDto>())
                .GroupBy(s => s.Id)
                .Select(g => g.First())
                .ToList();

            AvailableSports.Clear();
            foreach (var s in inUse)
                AvailableSports.Add(s);

            if (!venues.Any())
                await Shell.Current.DisplayAlert("Nenhum local",
                    "Não há locais que atendam aos filtros selecionados.", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", ex.Message, "OK");
        }
        finally { IsLoading = false; }
    }

    private static DateTime? Combine(DateTime? date, TimeSpan? time) =>
        (date, time) switch
        {
            (DateTime d, TimeSpan t) => d.Date.Add(t),
            _ => null
        };

    [RelayCommand]
    public async Task ClearFiltersAsync()
    {
        SelectedVenueType = null;
        SelectedSport = null;
        SelectedDate = null;
        FromTime = null;
        ToTime = null;

        await FetchAndPlotVenuesAsync(); // recarrega tudo sem filtros
        IsFilterModalVisible = false;
    }

}
