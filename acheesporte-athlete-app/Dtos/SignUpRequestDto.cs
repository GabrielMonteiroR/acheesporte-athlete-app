using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos;

public class SignUpRequestDto
{
    [JsonPropertyName("firstName")]
    [JsonRequired]
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    [JsonRequired]
    [Required]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    [JsonRequired]
    [Required]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    [JsonRequired]
    [Required]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("phone")]
    [JsonRequired]
    [Required]
    public string Phone { get; set; } = string.Empty;

    [JsonPropertyName("cpf")]
    [JsonRequired]
    [Required]
    public string Cpf { get; set; } = string.Empty;

    [JsonPropertyName("image")]
    public string? ProfileImageUrl { get; set; }
}
