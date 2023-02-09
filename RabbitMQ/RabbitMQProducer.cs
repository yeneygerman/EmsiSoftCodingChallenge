using EmsiSoft.RabbitMQ.Interface;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace EmsiSoft.RabbitMQ
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        public void SendProductMessage<T>(T message)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost"
                };

                var connection = factory.CreateConnection();

                var channel = connection.CreateModel();

                channel.QueueDeclare("hashes", exclusive: false);

                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: "hashes", body: body);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}