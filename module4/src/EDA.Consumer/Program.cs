using EDA.Consumer;
using EDA.Consumer.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddHostedService<OrderCreatedEventWorker>();
builder.Services.AddSingleton<OrderCreatedEventHandler>();
var app = builder.Build();

app.Run();
