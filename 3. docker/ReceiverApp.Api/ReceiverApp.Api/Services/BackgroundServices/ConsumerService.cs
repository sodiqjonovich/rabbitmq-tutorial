using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReceiverApp.Api.Interfaces;
using System.Text;

namespace ReceiverApp.Api.Services.BackgroundServices;

public class ConsumerService : BackgroundService
{
    private readonly IModel _channel;
    private readonly IFileService _fileService;
    private readonly IConfigurationSection _config;
    public ConsumerService(IConfiguration configuration, 
        IFileService fileService)
    {
        this._fileService = fileService;
        this._config = configuration.GetSection("MessageBroker");
        var factory = new ConnectionFactory()
        {
            HostName = _config["Host"],
            Port = int.Parse(_config["Port"]),
            UserName = _config["Username"],
            Password = _config["Password"]
        };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.QueueDeclare(queue: _config["Queue"],
                              durable: true, exclusive: false,
                              autoDelete: false, arguments: null);

        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += Receive_Handle;
        _channel.BasicConsume(_config["Queue"], false, consumer);
        return Task.CompletedTask;
    }
    private void Receive_Handle(object sender, BasicDeliverEventArgs model)
    {
        var json = Encoding.UTF8.GetString(model.Body.ToArray());
        _fileService.Write(json);
        _channel.BasicAck(deliveryTag: model.DeliveryTag, multiple: false);
    }
}
