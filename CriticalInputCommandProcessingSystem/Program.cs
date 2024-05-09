using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Thread.Sleep(20000);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Система обработки критичных входящих команд");
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

            channel.QueueDeclare(queue: "critical_input_command_processing_system",
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
                var body = Encoding.UTF8.GetBytes("critical_input_command_processing_system " + string.Join(" ", input.Skip(1)));
                switch (input[0])
                {
                    case "authorization_system":
                        channel.BasicPublish(exchange: "",
                                                     routingKey: "security_monitor",
                                                     basicProperties: null,
                                                     body: body);
                        if (input[1] == "script")
                            Console.WriteLine("Сценарий отправлен");
                        else
                            Console.WriteLine("Команда отправлена");
                        break;
                    case "critical_data_analysis_system":
                        Console.WriteLine($"Получена информация: {string.Join(" ", input.Skip(1))}");
                        break;
                    default:
                        Console.WriteLine("Отправитель не опознан");
                        break;
                }

            };
            channel.BasicConsume(queue: "critical_input_command_processing_system",
                                    autoAck: true,
                                    consumer: commands_consumer);

            while (Console.ReadLine() != "exit") { }
        }
    }
}