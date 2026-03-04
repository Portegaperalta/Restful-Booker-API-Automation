using System;
using System.Text.Json.Serialization;

namespace models;

public class InvalidBookingPatchRequest
{
  [JsonPropertyName("age")]
  public int Age { get; set; }
}