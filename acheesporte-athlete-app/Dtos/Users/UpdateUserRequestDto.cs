using System.Text.Json.Serialization;

namespace acheesporte_athlete_app.Dtos.Users;

public class UpdateUserRequestDto
{
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }
}
