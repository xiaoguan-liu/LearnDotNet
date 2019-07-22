using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDN.RabbitMq.SendApp.Core
{
    public class WorkQueues
    {
        public void Start()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "task_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                //持久化队列
                //channel.QueueDeclare(queue: "task_queue_durable", durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);

                Console.WriteLine(" Press input message ");
                
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                while (true)
                {
                    string message = Console.ReadLine();
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                        routingKey: "task_queue",
                        basicProperties: null,// properties,
                        body: body
                        );

                    Console.WriteLine("[x] Sent {0}", message);
                }
            }
        }
    }
}
