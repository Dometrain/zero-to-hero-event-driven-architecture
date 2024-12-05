namespace EDA.Producer.Core;

public record OrderCreatedPrivateEvent
{
    public string OrderId { get; set; }
    
    public string CustomerId { get; set; }
    
    public decimal OrderValue { get; set; }
    
    public string DeliveryAddress { get; set; }
}