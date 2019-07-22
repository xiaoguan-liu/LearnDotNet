using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDN.RabbitMq.SendApp.Core
{
    public class PublishSubscribe
    {
        public void Start()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: "fanout");
                Console.WriteLine(" Press input message ");

                while (true)
                {
                    string message = Console.ReadLine();
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "logs",
                        routingKey: "",
                        basicProperties: null,
                        body: body
                        );

                    Console.WriteLine("[x] Sent {0}", message);
                }
            }
        }
    }
}
