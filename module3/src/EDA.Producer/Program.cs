using EDA.Producer;
using EDA.Producer.Adapters;
using EDA.Producer.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = serviceScopeFactory.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.MapGet("/health", HandleHealthCheck)
.WithName("HealthCheck")
.WithOpenApi();

app.MapPost("/orders", HandleCreateOrder)
    .WithName("CreateOrder")
    .WithOpenApi();

app.Run();

static async Task<Order> HandleCreateOrder(CreateOrderRequest request, IOrders orders, IEventPublisher events)
{
    var order = await orders.New(request.CustomerId);

    await events.Publish(new OrderCreatedEvent()
    {
        OrderId = order.OrderId
    });

    return order;
}

static async Task<string> HandleHealthCheck(OrdersDbContext context)
{
    await context.Database.MigrateAsync();
    return "OK";
}
