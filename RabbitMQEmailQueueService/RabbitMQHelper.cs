using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEmailQueueService
{
    public class RabbitMQHelper
    {
        public static void SendMessage(string message)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "email_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "", routingKey: "email_queue", basicProperties: properties, body: body);
                    Console.WriteLine($"📨 Mesaj Kuyruğa Gönderildi: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Kuyruğa mesaj gönderilirken hata oluştu: {ex.Message}");
            }
        }
    }
}
