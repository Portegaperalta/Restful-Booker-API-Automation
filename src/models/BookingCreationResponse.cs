using System.Text.Json.Serialization;

namespace RestfulBooker.Models;

public class BookingCreationResponse
{
    [JsonPropertyName("bookingid")]
    public int BookingId { get; set; }

    [JsonPropertyName("booking")]
    public Booking Booking { get; set; } = new();
}