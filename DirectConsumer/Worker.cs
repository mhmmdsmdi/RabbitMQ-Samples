using DirectShared;
using Shared.Events;

namespace DirectConsumer;

public class Worker(ILogger<Worker> logger, MessageQueueConsumer<ItemUpdated> consumer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumer.ReceivedData += ConsumerOnReceivedData;
        consumer.StartConsuming();
    }

    private Task ConsumerOnReceivedData(object sender, MessageQueueEventArg<ItemUpdated> @event)
    {
        logger.LogInformation($"Item {@event.Data.Id} is updated name to {@event.Data.Name}");
        return Task.CompletedTask;
    }
}