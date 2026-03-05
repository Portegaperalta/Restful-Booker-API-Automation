using System.Text.Json.Serialization;

namespace RestfulBooker.Models;

public class AuthPostRequest
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}
