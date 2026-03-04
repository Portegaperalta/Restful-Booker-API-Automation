using System;
using System.Text.Json.Serialization;

namespace models;

public class InvalidBookingPostRequest
{
  [JsonPropertyName("lastname")]
  public string LastName { get; set; } = string.Empty;

  [JsonPropertyName("totalprice")]
  public int TotalPrice { get; set; }

  [JsonPropertyName("additionalneeds")]
  public string AdditionalNeeds { get; set; } = string.Empty;
}
