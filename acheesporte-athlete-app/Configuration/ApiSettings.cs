using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.Configuration
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string LoginEndpoint { get; set; } = string.Empty;
        public string RegisterEndpoint { get; set; } = string.Empty;
        public string CurrentUserEndpoint { get; set; } = string.Empty;
        public string ImageUploadEndpoint { get; set; } = string.Empty;
        public string ReservationEndpoint { get; set; } = string.Empty;
        public string VenueEndpoint { get; set; } = string.Empty;
        public string PlacesApiKey { get; set; } = string.Empty;
        public string VenueTypeEndpoint { get; set; } = string.Empty;
    }
}
