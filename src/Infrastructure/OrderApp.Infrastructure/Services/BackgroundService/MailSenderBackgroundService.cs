using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderApp.Application.Dtos.Requests;
using OrderApp.Application.SystemsModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Infrastructure.Services.BackgroundService
{
    internal class MailSenderBackgroundService : HostedService
    {
        private readonly RabbitMqSystemModel _rabbitMqSystemModel;
        private readonly SmtpSystemModel _smtpSystemModel;
        public MailSenderBackgroundService(
            IOptions<RabbitMqSystemModel> optionsRabbit,
            IOptions<SmtpSystemModel> optionsSmtp)
        {
            _rabbitMqSystemModel = optionsRabbit.Value;
            _smtpSystemModel = optionsSmtp.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken cToken)
        {
            while (!cToken.IsCancellationRequested)
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _rabbitMqSystemModel.HostName,
                    UserName = _rabbitMqSystemModel.UserName,
                    Password = _rabbitMqSystemModel.Password
                };
                using (IConnection connection = factory.CreateConnection())
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _rabbitMqSystemModel.QueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var orderModel = JsonConvert.DeserializeObject<CreateOrderRequest>(message);

                        SendMail(orderModel);
                    };

                    channel.BasicConsume(queue: _rabbitMqSystemModel.QueueName,
                        autoAck: true,
                        consumer: consumer);
                }

                await Task.Delay(TimeSpan.FromSeconds(10), cToken);
            }
        }

        private void SendMail(CreateOrderRequest model)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = _smtpSystemModel.Host;
            smtp.Port = _smtpSystemModel.Port;
            smtp.EnableSsl = true;

            NetworkCredential kullanicibilgi = new NetworkCredential(_smtpSystemModel.MailAdress, _smtpSystemModel.Password);
            smtp.Credentials = kullanicibilgi;

            MailAddress sender = new MailAddress(_smtpSystemModel.MailAdress);

            MailAddress receiver = new MailAddress(model.CustomerEmail);

            MailMessage mail = new MailMessage(sender, receiver);

            mail.Subject = "Ordered";
            mail.Body = "Ordered";
            mail.IsBodyHtml = true;

            //smtp.Send(mail);
        }
    }
}
