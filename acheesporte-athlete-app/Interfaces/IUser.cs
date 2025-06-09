using acheesporte_athlete_app.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acheesporte_athlete_app.Interfaces
{
    public interface IUser
    {
        Task<LoginResponseDto> SignInUserAsync(LoginRequestDto dto);
        Task<RegisterResponseDto> SignInUpUserAsync(RegisterRequestDto dto);
        Task<CurrentUserDto> GetCurrentUserAsync();
    }
}
