using System;
using System.Text.Json.Serialization;
using RestfulBooker.Models;

namespace models;

public class BookingPatchRequest
{
  [JsonPropertyName("firstname")]
  public string? FirstName { get; set; }

  [JsonPropertyName("lastname")]
  public string? LastName { get; set; }

  [JsonPropertyName("totalprice")]
  public int? TotalPrice { get; set; }

  [JsonPropertyName("depositpaid")]
  public bool? DepositPaid { get; set; }

  [JsonPropertyName("bookingdates")]
  public BookingDates? BookingDates { get; set; }

  [JsonPropertyName("additionalneeds")]
  public string? AdditionalNeeds { get; set; }
}
