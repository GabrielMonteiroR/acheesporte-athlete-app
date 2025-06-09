using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos.User;

public class RegisterResponseDto
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("roleId")]
    public int RoleId { get; set; }

    [JsonPropertyName("roleName")]
    public string? RoleName { get; set; }

    [JsonPropertyName("isBanned")]
    public bool IsBanned { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}