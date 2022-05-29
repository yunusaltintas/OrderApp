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
        public RabbitMQProducer(IOptions<RabbitMqSystemModel> options)
        {
            _rabbitMqSystemModel = options.Value;
        }
        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory() { 
                HostName = _rabbitMqSystemModel.HostName, 
                UserName = _rabbitMqSystemModel.UserName,
                Password = _rabbitMqSystemModel.Password };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
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
}
