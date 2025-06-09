using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos.User;

public class LoginRequestDto
{
    [Required]
    [JsonPropertyName("email")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }

    [Required]
    [JsonPropertyName("password")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }
}
