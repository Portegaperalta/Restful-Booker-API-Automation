using System.Text.Json.Serialization;

namespace RestfulBooker.Models;

public class AuthResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;
}
