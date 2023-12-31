using DirectShared;
using Shared.Events;

namespace DirectPublisher;

public class Worker(ILogger<Worker> logger, MessageQueuePublisher<ItemUpdated> publisher)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var counter = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            publisher.Publish(new ItemUpdated(counter, "Some random name."));

            counter++;
            await Task.Delay(1000, stoppingToken);
        }
    }
}