using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos;

public class SignInRequestDto
{
    [Required]
    [JsonPropertyName("email")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; }

    [Required]
    [JsonPropertyName("password")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }
}
