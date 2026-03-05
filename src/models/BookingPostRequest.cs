using System.Text.Json.Serialization;

namespace RestfulBooker.Models;

public class BookingPostRequest
{
    [JsonPropertyName("firstname")]
    public required string FirstName { get; set; }

    [JsonPropertyName("lastname")]
    public required string LastName { get; set; }

    [JsonPropertyName("totalprice")]
    public required int TotalPrice { get; set; }

    [JsonPropertyName("depositpaid")]
    public required bool DepositPaid { get; set; }

    [JsonPropertyName("bookingdates")]
    public required BookingDates BookingDates { get; set; }

    [JsonPropertyName("additionalneeds")]
    public required string AdditionalNeeds { get; set; }
}
