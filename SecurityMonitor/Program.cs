using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Security.Authentication.ExtendedProtection;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Thread.Sleep(20000);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Монитор безопасности");
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

            channel.QueueDeclare(queue: "system_for_reading_and_writing_user_scripts",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "сustom_script_library",
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
                            case "script":
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "authorization_system",
                                                     basicProperties: null,
                                                     body: body);

                                Console.WriteLine($"Сценарий отправлен в authorization_system");
                                break;
                            case "start":
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
                        if (input[1] != "script")
                            Console.WriteLine($"Команда отправлена в critical_input_command_processing_system");
                        else
                            Console.WriteLine($"Сценарий отправлен в critical_input_command_processing_system");
                        break;
                    case "critical_input_command_processing_system":
                        if (input[1] != "script" && input[1] != "start")
                        {
                            channel.BasicPublish(exchange: "",
                                                         routingKey: "critical_external_component_management_system",
                                                         basicProperties: null,
                                                         body: body);
                            Console.WriteLine($"Команда отправлена в critical_external_component_management_system");
                        }
                        else
                        {
                            channel.BasicPublish(exchange: "",
                                                         routingKey: "system_for_reading_and_writing_user_scripts",
                                                         basicProperties: null,
                                                         body: body);
                            if (input[1] == "script")
                                Console.WriteLine($"Сценарий отправлен в system_for_reading_and_writing_user_scripts");
                            else
                                Console.WriteLine($"Команда отправлена в system_for_reading_and_writing_user_scripts");
                        }
                        break;
                    case "critical_external_component_management_system":
                        channel.BasicPublish(exchange: "",
                                                         routingKey: "critical_external_components",
                                                         basicProperties: null,
                                                         body: body);
                        Console.WriteLine($"Команда отправлена в critical_external_components");
                        break;
                    case "critical_data_analysis_system":
                        channel.BasicPublish(exchange: "",
                                                     routingKey: "critical_input_command_processing_system",
                                                     basicProperties: null,
                                                     body: body);
                        Console.WriteLine($"Информация отправлена в critical_input_command_processing_system");
                        break;
                    case "system_for_reading_and_writing_user_scripts":
                        switch (input[1]) 
                        {
                            case "script":
                            case "start":
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "сustom_script_library",
                                                     basicProperties: null,
                                                     body: body);
                                if (input[1] == "script")
                                    Console.WriteLine("Сценарий отправлен в сustom_script_library");
                                else
                                    Console.WriteLine("Запрос на выполнение сценария отправен в сustom_script_library");
                                break;
                            default:
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "critical_external_component_management_system",
                                                     basicProperties: null,
                                                     body: body);
                                Console.WriteLine("Команда отправлена в critical_external_component_management_system");
                                break;
                        }
                        break;
                    case "сustom_script_library":
                        channel.BasicPublish(exchange: "",
                                                     routingKey: "system_for_reading_and_writing_user_scripts",
                                                     basicProperties: null,
                                                     body: body);
                        Console.WriteLine($"Команда отправлена в system_for_reading_and_writing_user_scripts");
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


