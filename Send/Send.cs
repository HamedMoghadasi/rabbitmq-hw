using System;
using System.Diagnostics;
using System.Text;
using RabbitMQ.Client;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            string message = string.Empty;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var sw = new Stopwatch();
                sw.Start();
                for (int i = 0; i < 100000; i++)
                {
                    message = $"Hello World! -- ({i})";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "hello",
                                         basicProperties: null,
                                         body: body);
                }
                sw.Stop();
                Console.WriteLine(" [x] Sent {0}", message);
                System.Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
