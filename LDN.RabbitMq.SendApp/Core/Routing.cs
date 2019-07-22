using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDN.RabbitMq.SendApp.Core
{
    public class Routing
    {
        public void Start()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");
                Console.WriteLine(" Press input message ");

                while (true)
                {
                    string message = Console.ReadLine();
                    string[] messArr= message.Split('-');

                    var body = Encoding.UTF8.GetBytes(messArr[1]);
                    channel.BasicPublish(exchange: "direct_logs",
                        routingKey: messArr[0],
                        basicProperties: null,
                        body: body
                        );

                    Console.WriteLine("[x] Sent {0}", message);
                }
            }
        }
    }
}
