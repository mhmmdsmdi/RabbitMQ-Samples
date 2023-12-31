using MassTransit;
using Shared.Events;

namespace OtherEventBusConsumer;

public class ItemUpdateConsumer(ILogger<ItemUpdateConsumer> logger) : IConsumer<ItemUpdated>
{
    public Task Consume(ConsumeContext<ItemUpdated> context)
    {
        logger.LogInformation($"Item {context.Message.Id} is updated name to {context.Message.Name}");

        return Task.CompletedTask;
    }
}