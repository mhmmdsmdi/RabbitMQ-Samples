using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace DirectShared;

public class MessageQueuePublisher<TData>
    where TData : class
{
    private readonly ILogger<MessageQueuePublisher<TData>> _logger;
    private readonly IConnection _connection;
    private readonly string _exchangeName;
    private readonly string _routeKey;
    private readonly string _queueName;
    private readonly IModel _channel;

    public MessageQueuePublisher(ILogger<MessageQueuePublisher<TData>> logger, IConnection connection, string exchangeName, string routeKey, string queueName)
    {
        _logger = logger;
        _connection = connection;
        _exchangeName = exchangeName;
        _routeKey = routeKey;
        _queueName = queueName;
        _channel = connection.CreateModel();

        if (!string.IsNullOrEmpty(_exchangeName))
            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, true, false, null);

        //var queue = _channel.QueueDeclare(_queueName, true, false, false);

        //if (!string.IsNullOrEmpty(_exchangeName))
        //    _channel.QueueBind(queue.QueueName, _exchangeName, _routeKey);
    }

    public void Publish(TData data)
    {
        if (_connection.IsOpen == false)
            _logger.LogError("Message Queue connection is closed");

        var json = data.ToJson();
        _channel.BasicPublish(exchange: _exchangeName,
            routingKey: _routeKey,
            basicProperties: null,
            body: json.ToByteArray());

        _logger.LogDebug("Message sent to {ExchangeName} -> {QueueName}", _exchangeName, _routeKey);
    }
}