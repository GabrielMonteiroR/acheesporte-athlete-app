using acheesporte_athlete_app.Dtos.User;

namespace acheesporte_athlete_app.Helpers;

public static class UserSession
{
    public static CurrentUserDto? CurrentUser { get; set; }
}