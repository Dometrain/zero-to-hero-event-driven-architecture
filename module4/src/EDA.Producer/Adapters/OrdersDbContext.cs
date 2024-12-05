using EDA.Producer.Core;
using Microsoft.EntityFrameworkCore;

namespace EDA.Producer.Adapters;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> context) : base(context){}
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<OutboxItem> Outbox { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=mysupersecretlocalpassword;Database=mydb;");
}