using System;
using System.Text.Json.Serialization;

namespace models;

public class BookingId
{
  [JsonPropertyName("bookingid")]
  public int BookingID { get; set; }
}