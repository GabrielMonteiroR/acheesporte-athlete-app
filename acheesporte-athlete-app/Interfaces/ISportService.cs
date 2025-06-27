using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.Interfaces;

public interface ISportService
{
    Task<List<SportDto>> GetSportsAsync();
}
