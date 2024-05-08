using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Security.Authentication.ExtendedProtection;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Монитор безопасности");
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

            channel.QueueDeclare(queue: "non_critical_external_components",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "authorization_system",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "critical_input_command_processing_system",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "critical_external_component_management_system",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "critical_external_components",
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
                var body = Encoding.UTF8.GetBytes(string.Join(" ", input));
                switch (input[0])
                {
                    case "input_command_handler":
                        switch (input[1])
                        {
                            case "пылесос":
                            case "жалюзи":
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "non_critical_external_component_management_system",
                                                     basicProperties: null,
                                                     body: body);

                                Console.WriteLine($"Команда отправлена в non_critical_external_component_management_system");
                                break;

                            case "климат-контроль":
                            case "свет":
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "authorization_system",
                                                     basicProperties: null,
                                                     body: body);

                                Console.WriteLine($"Команда отправлена в authorization_system");
                                break;

                            default:
                                Console.WriteLine("Компонент не опознан");
                                break;
                        }
                        break;
                    case "non_critical_data_analysis_system":
                        channel.BasicPublish(exchange: "",
                                                     routingKey: "non_critical_external_component_management_system",
                                                     basicProperties: null,
                                                     body: body);

                        Console.WriteLine($"Информация отправлена в non_critical_external_component_management_system");
                        break;
                    case "non_critical_external_component_management_system":
                        channel.BasicPublish(exchange: "",
                                                     routingKey: "non_critical_external_components",
                                                     basicProperties: null,
                                                     body: body);

                        Console.WriteLine($"Команда отправлена в non_critical_external_components");
                        break;
                    case "authorization_system":
                        Console.WriteLine("Авторизация подтверждена");
                        channel.BasicPublish(exchange: "",
                                                     routingKey: "critical_input_command_processing_system",
                                                     basicProperties: null,
                                                     body: body);
                        Console.WriteLine($"Команда отправлена в critical_input_command_processing_system");
                        break;
                    case "critical_input_command_processing_system":
                        if (input[1] != "script")
                        {
                            channel.BasicPublish(exchange: "",
                                                         routingKey: "critical_external_component_management_system",
                                                         basicProperties: null,
                                                         body: body);
                            Console.WriteLine($"Команда отправлена в critical_external_component_management_system");
                        }
                        break;
                    case "critical_external_component_management_system":
                        channel.BasicPublish(exchange: "",
                                                         routingKey: "critical_external_components",
                                                         basicProperties: null,
                                                         body: body);
                        Console.WriteLine($"Команда отправлена в critical_external_components");
                        break;
                    default:
                        Console.WriteLine("Отправитель не опознан");
                        break;
                }
            };
            channel.BasicConsume(queue: "security_monitor",
                                    autoAck: true,
                                    consumer: commands_consumer);

            string command;
            while ((command = Console.ReadLine()) != "exit") {}
        }
    }
}


