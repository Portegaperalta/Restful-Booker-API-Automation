using System.Text.Json.Serialization;

namespace RestfulBooker.Models;

public class AuthResponse
{
    [JsonPropertyName("token")]
    public required string Token { get; set; }
}
