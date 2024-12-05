namespace EDA.Producer.Core;

public interface IOrders
{
    Task<Order> New(string customerId);
}