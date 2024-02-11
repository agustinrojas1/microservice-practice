using Microsoft.AspNetCore.Connections;
using PaymentAuthorization.Api.Models;
using RabbitMQ.Client;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;

namespace PaymentAuthorization.Api.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
            };

            try
            {
                _connection = factory.CreateConnection();

                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _channel.QueueDeclare("payment_authorization_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                // Enlazar la cola al exchange "trigger"
                _channel.QueueBind("payment_authorization_queue", "trigger", "");

                _connection.ConnectionShutdown += RabbitMQConnectionShutDown;

                Console.WriteLine("Connected to Message Bus");
                
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Could not connect to Message Bus: {ex.Message}");
            }
        }
        public void PublishNewAuthorization(PaymentAuthorizationRequest paymentRequest)
        {
            var message = JsonSerializer.Serialize(paymentRequest);
            if (_connection.IsOpen)
            {
                //Enviar el mensaje
                SendMessage(message);

                Console.WriteLine("RabbitMQ Connection Open");
            }
            else
            {
                Console.WriteLine("RabbitMQ Connection is closed");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            //fanout ignora las routing keys asi que la dejo vacia
            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);

            Console.WriteLine($"Message sent: {message}");


        }

        //cuando la clase muere, borro todo
        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            Console.WriteLine("Message Bus disposed");
        }
        private void RabbitMQConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ Connection Shutdown");
        }

    }
}
