using acheesporte_athlete_app.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.Services
{
    public class ReservationService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public ReservationService(HttpClient httpClient, ApiSettings apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings;
        }

        public async Task<ReservationRes>
    }
}
