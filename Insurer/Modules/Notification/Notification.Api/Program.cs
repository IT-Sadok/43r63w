using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Notification.Api.Data;
using Notification.Api.EventsHandler;
using Notification.Api.Interfaces;
using Notification.Api.Messaging;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.Configure<RabbitMqQueue>(builder.Configuration.GetSection("RabbitMqQueues"));

builder.Services.AddScoped<IEventHandler, PolicyCreatedEventHandler>();
builder.Services.AddScoped<IEventHandler, PolicyUpdatedEventHandler>();

builder.Services.AddDbContext<NotificationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<IConnection>(sp =>
{
    var rabbitMqOptions = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
    var factory = new ConnectionFactory
    {
        HostName = rabbitMqOptions.Host,
        Port = rabbitMqOptions.Port,
        VirtualHost = rabbitMqOptions.VirtualHost,
        UserName = rabbitMqOptions.UserName,
        Password = rabbitMqOptions.Password,
    };

    return factory.CreateConnectionAsync()
        .GetAwaiter()
        .GetResult();
});

builder.Services.AddHostedService<RabbitMqConsumer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.ApplyMigrationAsync();
}

app.UseHttpsRedirection();

app.Run();