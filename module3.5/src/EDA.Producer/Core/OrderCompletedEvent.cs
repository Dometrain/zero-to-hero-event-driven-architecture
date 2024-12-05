namespace EDA.Producer.Core;

public record OrderCompletedEvent
{
    public string OrderId { get; set; }
}