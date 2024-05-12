using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        //Thread.Sleep(20000);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Критичные внешние компоненты");
        Console.ForegroundColor = ConsoleColor.White;

        var factory = new ConnectionFactory() { HostName = "localhost" };
        //var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "critical_external_components",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "e2e_test_out",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var commands_consumer = new EventingBasicConsumer(channel);
            commands_consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var input = message.Split().ToArray();
                Console.WriteLine($"Получил команду от {input[0]}");
                Console.WriteLine($"Выполнил команду: {string.Join(" ", input.Skip(1))}");

                var bodyOut = Encoding.UTF8.GetBytes($"Выполнил команду: {string.Join(" ", input.Skip(1))}");

                channel.BasicPublish(exchange: "",
                                             routingKey: "e2e_test_out",
                                             basicProperties: null,
                                             body: bodyOut);

            };
            channel.BasicConsume(queue: "critical_external_components",
                                    autoAck: true,
                                    consumer: commands_consumer);

            while (Console.ReadLine() != "exit") { }
        }
    }
}