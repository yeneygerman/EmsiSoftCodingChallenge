using Data;
using EmsiSoft.Data.Entity;
using EmsiSoft.Models;
using EmsiSoft.Services;
using EmsiSoft.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace RabbitMQHashes.Processor
{
    internal class Program : IHashesRabbitMQService
    {
        public static IConfigurationRoot Configuration { get; set; }
        private static EmsiSoftContextDB _emsiSoftContextDB;

        void IHashesRabbitMQService.AddHashes(IEnumerable<EmsiSoft.Data.Entity.Hashes> hashList) { }

        static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            var services = new ServiceCollection();

            services.AddDbContext<EmsiSoftContextDB>(options => options.UseSqlServer(Configuration.GetConnectionString("Local")));

            var serviceProvider = services.BuildServiceProvider();
            _emsiSoftContextDB = serviceProvider.GetService<EmsiSoftContextDB>();

            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare("hashes", exclusive: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                if (string.IsNullOrEmpty(message))
                {
                    Console.WriteLine("Hash message is empty");
                }
                else
                {
                    var hashList = JsonSerializer.Deserialize<HashModel>(message);
                    IHashesRabbitMQService obj = new HashesService(_emsiSoftContextDB);

                    IList<Hashes> hashes = new List<Hashes>();

                    foreach (var hash in hashList.Hashes)
                    {
                        hashes.Add(new Hashes { Date = hashList.Date.Date, SHA1 = hash });
                    }

                    obj.AddHashes(hashes.AsEnumerable());

                    Console.WriteLine($"Hashes message received with date: {hashList.Date.Date}");
                }
            };

            channel.BasicConsume(queue: "hashes", autoAck: true, consumer: consumer);
            Console.ReadKey();
        }
    }
}