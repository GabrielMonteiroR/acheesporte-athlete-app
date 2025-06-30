using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Configuration;

public class ApiSettings
{
    [JsonPropertyName("BaseUrl")]
    public string BaseUrl { get; set; }

    [JsonPropertyName("SignInEndpoint")]
    public string SignInEndpoint { get; set; }

    [JsonPropertyName("SignUpEndpoint")]
    public string SignUpEndpoint { get; set; }

    [JsonPropertyName("ImageUploadEndpoint")]
    public string ImageUploadEndpoint { get; set; }

    [JsonPropertyName("CurrentUserEndpoint")]
    public string CurrentUserEndpoint { get; set; }

    [JsonPropertyName("PlacesApiKey")]
    public string PlacesApiKey { get; set; }

    public string VenuesEndpoint { get; set; }

    public string UploadVenueImagesEndpoint { get; set; }

    public string GetVenueTypesEndpoint { get; set; }

    public string CreateVenuesEndpoint { get; set; }

    public string UpdateVenueEndpoint { get; set; }

    public string GetVenueByIdEndpoint { get; set; }

    public string GetAvailableTimesByVenueIdEndpoint { get; set; }

    public string CreateAvailableTimesEndpoint { get; set; }

    public string GetAvailableTimesByIdEndpoint { get; set; }

    public string DeleteAvailableTimesEndpoint { get; set; }

    public string UpdateAvailableTimesEndpoint { get; set; }

    public string GetUserById { get; set; }

    public string UpdateUserInfoEndpoint { get; set; }

    public string UpdateUserProfilePictureBaseUrl { get; set; }

    public string UpdateUserProfilePicturePatchUrl { get; set; }

    public string GetReservationsByVenueIdEndpoint { get; set; }

    public string GetReservationsByUserIdEndpoint { get; set; }

    public string GetSportsEndpoint { get; set; }

    public string ReservationsEndpoint { get; set; }

    public string GetHistoryByUserIdEndpoint { get; set; }

    public string GetNextReservationByUserIdEndpoint { get; set; }

    public string GetUserCurrentStreakEndpoint { get; set; }
}