using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.Dtos.ReservationDtos
{
    public class StreakDto
    {
        public int UserId { get; set; }
        public string Message { get; set; } = "";
        public int StreakCount { get; set; }
    }
}
