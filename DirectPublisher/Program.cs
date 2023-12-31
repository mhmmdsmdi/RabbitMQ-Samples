using DirectPublisher;
using DirectShared;
using Shared.Events;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddRabbitMq(c =>
    {
        c.Hostname = "localhost";
        c.Username = "guest";
        c.Password = "guest";
        c.Port = -1;
    })
    .AddRabbitMqPublisher<ItemUpdated>("EventBus_Test", "Event.ItemUpdated", "Publisher1");

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();