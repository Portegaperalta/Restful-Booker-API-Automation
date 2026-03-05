using System;
using System.Text.Json.Serialization;
using RestfulBooker.Models;

namespace models;

public class BookingPatchRequest
{
  [JsonPropertyName("firstname")]
  public required string FirstName { get; set; }

  [JsonPropertyName("lastname")]
  public required string LastName { get; set; }
}
