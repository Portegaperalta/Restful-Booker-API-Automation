using System.Text.Json.Serialization;

namespace RestfulBooker.Models;

public class AuthPostResponse
{
    [JsonPropertyName("token")]
    public required string Token { get; set; }
}
