using Microsoft.EntityFrameworkCore;

namespace EDA.Producer.Adapters;

[PrimaryKey("Id")]
public class OutboxItem
{
    public int Id { get; set; }
    
    public DateTime EventTime { get; set; }
    
    public string EventType { get; set; }
    
    public string EventData { get; set; }
    
    public bool Processed { get; set; }
}