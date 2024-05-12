using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Security.Authentication.ExtendedProtection;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        //Thread.Sleep(20000);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Монитор безопасности");
        Console.ForegroundColor = ConsoleColor.White;


        var factory = new ConnectionFactory() { HostName = "localhost" };
        //var factory = new ConnectionFactory() { HostName = "rabbitmq" };
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

            channel.QueueDeclare(queue: "test_out",
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
                byte[] bodyOut;
                Console.WriteLine($"Получил сообщение от {input[0]}");
                bodyOut = Encoding.UTF8.GetBytes($"Получил сообщение от {input[0]}");
                channel.BasicPublish(exchange: "",
                                     routingKey: "test_out",
                                     basicProperties: null,
                                     body: bodyOut);
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
                                bodyOut = Encoding.UTF8.GetBytes($"Команда отправлена в non_critical_external_component_management_system");
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "test_out",
                                                     basicProperties: null,
                                                     body: bodyOut);
                                break;

                            case "климат-контроль":
                            case "свет":
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "authorization_system",
                                                     basicProperties: null,
                                                     body: body);

                                Console.WriteLine($"Команда отправлена в authorization_system");
                                bodyOut = Encoding.UTF8.GetBytes($"Команда отправлена в authorization_system");
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "test_out",
                                                     basicProperties: null,
                                                     body: bodyOut);
                                break;
                            case "script":
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "authorization_system",
                                                     basicProperties: null,
                                                     body: body);

                                Console.WriteLine($"Сценарий отправлен в authorization_system");
                                bodyOut = Encoding.UTF8.GetBytes($"Сценарий отправлен в authorization_system");
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "test_out",
                                                     basicProperties: null,
                                                     body: bodyOut);
                                break;
                            case "start":
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "authorization_system",
                                                     basicProperties: null,
                                                     body: body);

                                Console.WriteLine($"Команда отправлена в authorization_system");
                                bodyOut = Encoding.UTF8.GetBytes($"Команда отправлена в authorization_system");
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "test_out",
                                                     basicProperties: null,
                                                     body: bodyOut);
                                break;
                            default:
                                Console.WriteLine("Компонент не опознан");
                                bodyOut = Encoding.UTF8.GetBytes("Компонент не опознан");
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "test_out",
                                                     basicProperties: null,
                                                     body: bodyOut);
                                break;
                        }
                        break;
                    case "non_critical_data_analysis_system":
                        channel.BasicPublish(exchange: "",
                                                     routingKey: "non_critical_external_component_management_system",
                                                     basicProperties: null,
                                                     body: body);

                        Console.WriteLine($"Информация отправлена в non_critical_external_component_management_system");
                        bodyOut = Encoding.UTF8.GetBytes($"Информация отправлена в non_critical_external_component_management_system");
                        channel.BasicPublish(exchange: "",
                                             routingKey: "test_out",
                                             basicProperties: null,
                                             body: bodyOut);
                        break;
                    case "non_critical_external_component_management_system":
                        channel.BasicPublish(exchange: "",
                                                     routingKey: "non_critical_external_components",
                                                     basicProperties: null,
                                                     body: body);

                        Console.WriteLine($"Команда отправлена в non_critical_external_components");
                        bodyOut = Encoding.UTF8.GetBytes($"Команда отправлена в non_critical_external_components");
                        channel.BasicPublish(exchange: "",
                                             routingKey: "test_out",
                                             basicProperties: null,
                                             body: bodyOut);
                        break;
                    case "authorization_system":
                        Console.WriteLine("Авторизация подтверждена");
                        bodyOut = Encoding.UTF8.GetBytes("Авторизация подтверждена");
                        channel.BasicPublish(exchange: "",
                                             routingKey: "test_out",
                                             basicProperties: null,
                                             body: bodyOut);

                        channel.BasicPublish(exchange: "",
                                                     routingKey: "critical_input_command_processing_system",
                                                     basicProperties: null,
                                                     body: body);
                        if (input[1] != "script")
                        {
                            Console.WriteLine($"Команда отправлена в critical_input_command_processing_system");
                            bodyOut = Encoding.UTF8.GetBytes($"Команда отправлена в critical_input_command_processing_system");
                            channel.BasicPublish(exchange: "",
                                                 routingKey: "test_out",
                                                 basicProperties: null,
                                                 body: bodyOut);
                        }
                        else
                        {
                            Console.WriteLine($"Сценарий отправлен в critical_input_command_processing_system");
                            bodyOut = Encoding.UTF8.GetBytes($"Сценарий отправлен в critical_input_command_processing_system");
                            channel.BasicPublish(exchange: "",
                                                 routingKey: "test_out",
                                                 basicProperties: null,
                                                 body: bodyOut);
                        }
                        break;
                    case "critical_input_command_processing_system":
                        if (input[1] != "script" && input[1] != "start")
                        {
                            channel.BasicPublish(exchange: "",
                                                         routingKey: "critical_external_component_management_system",
                                                         basicProperties: null,
                                                         body: body);
                            Console.WriteLine($"Команда отправлена в critical_external_component_management_system");
                            bodyOut = Encoding.UTF8.GetBytes($"Команда отправлена в critical_external_component_management_system");
                            channel.BasicPublish(exchange: "",
                                                 routingKey: "test_out",
                                                 basicProperties: null,
                                                 body: bodyOut);
                        }
                        else
                        {
                            channel.BasicPublish(exchange: "",
                                                         routingKey: "system_for_reading_and_writing_user_scripts",
                                                         basicProperties: null,
                                                         body: body);
                            if (input[1] == "script")
                            {
                                Console.WriteLine($"Сценарий отправлен в system_for_reading_and_writing_user_scripts");
                                bodyOut = Encoding.UTF8.GetBytes($"Сценарий отправлен в system_for_reading_and_writing_user_scripts");
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "test_out",
                                                     basicProperties: null,
                                                     body: bodyOut);
                            }
                            else
                            {
                                Console.WriteLine($"Команда отправлена в system_for_reading_and_writing_user_scripts");
                                bodyOut = Encoding.UTF8.GetBytes($"Команда отправлена в system_for_reading_and_writing_user_scripts");
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "test_out",
                                                     basicProperties: null,
                                                     body: bodyOut);
                            }
                        }
                        break;
                    case "critical_external_component_management_system":
                        channel.BasicPublish(exchange: "",
                                                         routingKey: "critical_external_components",
                                                         basicProperties: null,
                                                         body: body);
                        Console.WriteLine($"Команда отправлена в critical_external_components");
                        bodyOut = Encoding.UTF8.GetBytes($"Команда отправлена в critical_external_components");
                        channel.BasicPublish(exchange: "",
                                             routingKey: "test_out",
                                             basicProperties: null,
                                             body: bodyOut);
                        break;
                    case "critical_data_analysis_system":
                        channel.BasicPublish(exchange: "",
                                                     routingKey: "critical_input_command_processing_system",
                                                     basicProperties: null,
                                                     body: body);
                        Console.WriteLine($"Информация отправлена в critical_input_command_processing_system");
                        bodyOut = Encoding.UTF8.GetBytes($"Информация отправлена в critical_input_command_processing_system");
                        channel.BasicPublish(exchange: "",
                                             routingKey: "test_out",
                                             basicProperties: null,
                                             body: bodyOut);
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
                                {
                                    Console.WriteLine("Сценарий отправлен в сustom_script_library");
                                    bodyOut = Encoding.UTF8.GetBytes("Сценарий отправлен в сustom_script_library");
                                    channel.BasicPublish(exchange: "",
                                                         routingKey: "test_out",
                                                         basicProperties: null,
                                                         body: bodyOut);
                                }
                                else
                                {
                                    Console.WriteLine("Запрос на выполнение сценария отправен в сustom_script_library");
                                    bodyOut = Encoding.UTF8.GetBytes("Запрос на выполнение сценария отправен в сustom_script_library");
                                    channel.BasicPublish(exchange: "",
                                                         routingKey: "test_out",
                                                         basicProperties: null,
                                                         body: bodyOut);

                                }
                                break;
                            default:
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "critical_external_component_management_system",
                                                     basicProperties: null,
                                                     body: body);
                                Console.WriteLine("Команда отправлена в critical_external_component_management_system");
                                bodyOut = Encoding.UTF8.GetBytes("Команда отправлена в critical_external_component_management_system");
                                channel.BasicPublish(exchange: "",
                                                     routingKey: "test_out",
                                                     basicProperties: null,
                                                     body: bodyOut);
                                break;
                        }
                        break;
                    case "сustom_script_library":
                        channel.BasicPublish(exchange: "",
                                                     routingKey: "system_for_reading_and_writing_user_scripts",
                                                     basicProperties: null,
                                                     body: body);
                        Console.WriteLine($"Команда отправлена в system_for_reading_and_writing_user_scripts");
                        bodyOut = Encoding.UTF8.GetBytes($"Команда отправлена в system_for_reading_and_writing_user_scripts");
                        channel.BasicPublish(exchange: "",
                                             routingKey: "test_out",
                                             basicProperties: null,
                                             body: bodyOut);
                        break;
                    default:
                        Console.WriteLine("Отправитель не опознан");
                        bodyOut = Encoding.UTF8.GetBytes("Отправитель не опознан");
                        channel.BasicPublish(exchange: "",
                                             routingKey: "test_out",
                                             basicProperties: null,
                                             body: bodyOut);
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


