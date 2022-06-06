using Microsoft.Extensions.Options;
using OrderApp.Application.SystemsModels;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Infrastructure.Services.BackgroundService
{
    public class RabbitMQConnectionManager
    {

        private readonly RabbitMqSystemModel _rabbitMqSystemModel;
        public RabbitMQConnectionManager(IOptions<RabbitMqSystemModel> options)
        {
            _rabbitMqSystemModel = options.Value;
        }

        public IModel CreateConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqSystemModel.HostName,
                UserName = _rabbitMqSystemModel.UserName,
                Password = _rabbitMqSystemModel.Password
            };
            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();

            return channel;
        }
    }
}
