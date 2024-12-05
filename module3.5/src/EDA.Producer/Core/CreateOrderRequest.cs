using System.Text.Json.Serialization;

namespace EDA.Producer.Core;

public class CreateOrderRequest
{
    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; }
}