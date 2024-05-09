using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Thread.Sleep(20000);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Система управления критичными внешними компонентами");
        Console.ForegroundColor = ConsoleColor.White;

        //var factory = new ConnectionFactory() { HostName = "localhost" };
        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "security_monitor",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "critical_external_component_management_system",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var commands_consumer = new EventingBasicConsumer(channel);
            commands_consumer.Received += (model, ea) =>
            {
                var receiving_command = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(receiving_command);
                var input = message.Split().ToArray();
                Console.WriteLine($"Получил сообщение от {input[0]}");
                var body = Encoding.UTF8.GetBytes("critical_external_component_management_system " + string.Join(" ", input.Skip(1)));
                switch (input[0])
                {
                    case "critical_input_command_processing_system":
                    case "system_for_reading_and_writing_user_scripts":
                        channel.BasicPublish(exchange: "",
                                                     routingKey: "security_monitor",
                                                     basicProperties: null,
                                                     body: body);
                        Console.WriteLine("Команда отправлена");
                        break;
                    default:
                        Console.WriteLine("Отправитель не опознан");
                        break;
                }

            };
            channel.BasicConsume(queue: "critical_external_component_management_system",
                                    autoAck: true,
                                    consumer: commands_consumer);

            while (Console.ReadLine() != "exit") { }
        }
    }
}