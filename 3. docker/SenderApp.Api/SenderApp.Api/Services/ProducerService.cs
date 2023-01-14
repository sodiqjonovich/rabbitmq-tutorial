using Newtonsoft.Json;
using RabbitMQ.Client;
using SenderApp.Api.Interfaces;
using SenderApp.Api.Models;
using System.Text;

namespace SenderApp.Api.Services;
public class ProducerService : IProducerService
{
    private readonly IConfigurationSection _config;
    private readonly IModel _channel;
    public ProducerService(IConfiguration configuration)
    {
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

    public void Send(Message message)
    {
        var jsonMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);
        
        _channel.BasicPublish(exchange: "",
                             routingKey: _config["Queue"],
                             basicProperties: null,
                             body: body);
    }
}
