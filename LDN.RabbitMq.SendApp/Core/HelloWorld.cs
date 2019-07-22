﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace LDN.RabbitMq.SendApp.Core
{
    public class HelloWorld
    {
        public void Start()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
                Console.WriteLine(" Press input message ");

                while (true)
                {
                    string message = Console.ReadLine();
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                        routingKey: "hello",
                        basicProperties: null,
                        body: body
                        );

                    Console.WriteLine("[x] Sent {0}", message);
                }
            }
        }
    }
}
