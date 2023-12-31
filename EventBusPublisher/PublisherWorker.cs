using MassTransit;
using Shared.Events;

namespace EventBusPublisher;

public class PublisherWorker(ILogger<PublisherWorker> logger, IBus publisher) : BackgroundService
{
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var counter = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("PublisherWorker running at: {time}", DateTimeOffset.Now);

            await publisher.Publish(new ItemUpdated(counter, "Some random name"), stoppingToken);

            counter++;
            await Task.Delay(1000, stoppingToken);
        }
    }
}