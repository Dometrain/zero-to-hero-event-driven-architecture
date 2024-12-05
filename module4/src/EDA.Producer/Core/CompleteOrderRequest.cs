using System.Text.Json.Serialization;

namespace EDA.Producer.Core;

public class CompleteOrderRequest
{
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; }
}