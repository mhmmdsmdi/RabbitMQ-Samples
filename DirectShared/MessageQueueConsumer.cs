using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DirectShared;

public class MessageQueueConsumer<TData>(ILogger<MessageQueueConsumer<TData>> logger, IConnection connection,
    string exchangeName,
    string routeKey,
    string queueName)
    where TData : class
{
    private IModel _channel;

    //public event EventHandler<TData> ReceivedData;

    public event AsyncEventHandler<MessageQueueEventArg<TData>> ReceivedData;

    public void StartConsuming()
    {
        _channel = connection.CreateModel();
        if (!string.IsNullOrEmpty(exchangeName))
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true, false, null);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += Consumer_EventReceived;

        var queue = _channel.QueueDeclare(queueName, true, false, false);

        if (!string.IsNullOrEmpty(exchangeName))
            _channel.QueueBind(queue.QueueName, exchangeName, routeKey);

        _channel.BasicConsume(queue.QueueName, true, consumer);

        logger.LogInformation("Queue With Name {queueName} Created.", queue.QueueName);
    }

    private Task Consumer_EventReceived(object sender, BasicDeliverEventArgs @event)
    {
        var content = Encoding.UTF8.GetString(@event.Body.Span);
        var data = content.ToObject<TData>();
        ReceivedData?.Invoke(this, new MessageQueueEventArg<TData>(data));

        return Task.CompletedTask;
    }
}