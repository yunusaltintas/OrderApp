using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderApp.Application.Interfaces.IService;
using OrderApp.Application.SystemsModels;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Infrastructure.Services.BackgroundService
{
    public class RabbitMQProducer : IMessageProducer
    {
        private readonly RabbitMqSystemModel _rabbitMqSystemModel;
        private readonly RabbitMQConnectionManager _rabbitMQConnectionManager;
        public RabbitMQProducer(IOptions<RabbitMqSystemModel> options, RabbitMQConnectionManager rabbitMQConnectionManager)
        {
            _rabbitMqSystemModel = options.Value;
            _rabbitMQConnectionManager = rabbitMQConnectionManager;
        }
        public void SendMessage<T>(T message)
        {
            var channel = _rabbitMQConnectionManager.CreateConnection();

            channel.QueueDeclare(queue: _rabbitMqSystemModel.QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            string jsonMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            channel.BasicPublish(exchange: _rabbitMqSystemModel.Exchange,
                routingKey: _rabbitMqSystemModel.QueueName,
                body: body);

        }
    }
}
