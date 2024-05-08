using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Система управления некритичными внешними компонентами");
        Console.ForegroundColor = ConsoleColor.White;

        //var factory = new ConnectionFactory() { HostName = "rabbitmq" };
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "security_monitor",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "non_critical_external_component_management_system",
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
                var body = Encoding.UTF8.GetBytes("non_critical_external_component_management_system " + string.Join(" ", input.Skip(1)));
                switch (input[0])
                {
                    case "input_command_handler":
                        channel.BasicPublish(exchange: "",
                                                     routingKey: "security_monitor",
                                                     basicProperties: null,
                                                     body: body);
                        Console.WriteLine("Команда отправлена");
                        break;
                    case "non_critical_data_analysis_system":
                        Console.WriteLine("Получены данные : " + string.Join(" ", input.Skip(1)));
                        break;
                    default:
                        Console.WriteLine("Отправитель не опознан");
                        break;
                }

            };
            channel.BasicConsume(queue: "non_critical_external_component_management_system",
                                    autoAck: true,
                                    consumer: commands_consumer);

            while (Console.ReadLine() != "exit") { }
        }
    }
}