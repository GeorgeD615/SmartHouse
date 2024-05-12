using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        //Thread.Sleep(20000);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Обработчик входящих команд");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Команда должна посылаться в формате {название_внешнего_компонента} : {описание_команды}");
        Console.WriteLine("Описание команд для разных компонентов:");
        Console.WriteLine("Неритичные компоненты");
        Console.WriteLine(" - Пылесос");
        Console.WriteLine("     сухая уборка - {название_комнаты}");
        Console.WriteLine("     влажная уборка - {название_комнаты}");
        Console.WriteLine(" - Жалюзи");
        Console.WriteLine("     поднять");
        Console.WriteLine("     опустить");
        Console.WriteLine("Критичные компоненты(в конце необходимо указать пароль)");
        Console.WriteLine(" - Климат-контроль");
        Console.WriteLine("     температура +{градусы по цельсию}");
        Console.WriteLine("     температура -{градусы по цельсию}");
        Console.WriteLine(" - Свет");
        Console.WriteLine("     включить");
        Console.WriteLine("     выкючить");
        Console.WriteLine();
        Console.WriteLine("Для добавления сценария:");
        Console.WriteLine("script {название_сценария} begin {пароль}");
        Console.WriteLine("...");
        Console.WriteLine("...");
        Console.WriteLine("...");
        Console.WriteLine("end");
        Console.WriteLine("Запустить сценарий: start {название_сценария} {пароль}");


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

            channel.QueueDeclare(queue: "input_command_handler",
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
                var bodyOut = Encoding.UTF8.GetBytes("input_command_handler " + message);
                Console.WriteLine($"Получил команду {message}");
                switch (input[0])
                {
                    case "пылесос":
                        if (input.Length != 6 || input[1] != ":" ||
                            (input[2] != "сухая" && input[2] != "влажная") ||
                            input[3] != "уборка" || input[4] != "-")
                        {
                            Console.WriteLine("Некорректный формат входной команды");
                            break;
                        }
                        channel.BasicPublish(exchange: "",
                                             routingKey: "security_monitor",
                                             basicProperties: null,
                                             body: bodyOut);

                        Console.WriteLine($"Команда отправлена");
                        break;
                    case "жалюзи":
                        if (input.Length != 3 || input[1] != ":" || input[2] != "поднять" && input[2] != "опустить")
                        {
                            Console.WriteLine("Некорректный формат входной команды");
                            break;
                        }
                        channel.BasicPublish(exchange: "",
                                             routingKey: "security_monitor",
                                             basicProperties: null,
                                             body: bodyOut);

                        Console.WriteLine($"Команда отправлена");
                        break;
                    case "климат-контроль":
                        if (input.Length != 5 || input[1] != ":" || input[2] != "температура" ||
                            (!input[3].StartsWith('+') && !input[3].StartsWith("-")) ||
                            input[3].Skip(1).All(char.IsDigit))
                        {
                            Console.WriteLine("Некорректный формат входной команды");
                            break;
                        }
                        channel.BasicPublish(exchange: "",
                                             routingKey: "security_monitor",
                                             basicProperties: null,
                                             body: bodyOut);

                        Console.WriteLine($"Команда отправлена");
                        break;
                    case "свет":
                        if (input.Length != 4 || input[1] != ":" || (input[2] != "включить" && input[2] != "выключить"))
                        {
                            Console.WriteLine("Некорректный формат входной команды");
                            break;
                        }

                        channel.BasicPublish(exchange: "",
                                             routingKey: "security_monitor",
                                             basicProperties: null,
                                             body: bodyOut);

                        Console.WriteLine($"Команда отправлена");
                        break;
                    case "script":
                        if (input[2] != "begin" || input.Length != 4)
                            Console.WriteLine("Некорректный формат входной команды");


                        body = Encoding.UTF8.GetBytes("input_command_handler " + message);
                        channel.BasicPublish(exchange: "",
                                             routingKey: "security_monitor",
                                             basicProperties: null,
                                             body: bodyOut);
                        break;
                    case "start":
                        if (input.Length != 3)
                        {
                            Console.WriteLine("Некорректный формат входной команды");
                            break;
                        }
                        channel.BasicPublish(exchange: "",
                                             routingKey: "security_monitor",
                                             basicProperties: null,
                                             body: bodyOut);
                        break;

                    default:
                        Console.WriteLine("Компонент не опознан");
                        break;
                }
            };
            channel.BasicConsume(queue: "input_command_handler",
                                    autoAck: true,
                                    consumer: commands_consumer);

            string command;
            while((command = Console.ReadLine()) != "exit")
            {
                if (string.IsNullOrEmpty(command))
                    continue;

                command = command.Trim().ToLower();

                var input = command.Split();

                var body = Encoding.UTF8.GetBytes("input_command_handler " + command);

                switch (input[0])
                {
                    case "пылесос":
                        if (input.Length != 6 || input[1] != ":" ||
                            (input[2] != "сухая" && input[2] != "влажная") ||
                            input[3] != "уборка" || input[4] != "-")
                        {
                            Console.WriteLine("Некорректный формат входной команды");
                            break;
                        }
                        channel.BasicPublish(exchange: "",
                                             routingKey: "security_monitor",
                                             basicProperties: null,
                                             body: body);

                        Console.WriteLine($"Команда отправлена");
                        break;
                    case "жалюзи":
                        if (input.Length != 3 || input[1] != ":" || input[2] != "поднять" && input[2] != "опустить")
                        {
                            Console.WriteLine("Некорректный формат входной команды");
                            break;
                        }
                        channel.BasicPublish(exchange: "",
                                             routingKey: "security_monitor",
                                             basicProperties: null,
                                             body: body);

                        Console.WriteLine($"Команда отправлена");
                        break;
                    case "климат-контроль":
                        if (input.Length != 5 || input[1] != ":" || input[2] != "температура" ||
                            (!input[3].StartsWith('+') && !input[3].StartsWith("-")) ||
                            input[3].Skip(1).All(char.IsDigit))
                        {
                            Console.WriteLine("Некорректный формат входной команды");
                            break;
                        }
                        channel.BasicPublish(exchange: "",
                                             routingKey: "security_monitor",
                                             basicProperties: null,
                                             body: body);

                        Console.WriteLine($"Команда отправлена");
                        break;
                    case "свет":
                        if (input.Length != 4 || input[1] != ":" || (input[2] != "включить" && input[2] != "выключить"))
                        {
                            Console.WriteLine("Некорректный формат входной команды");
                            break;
                        }

                        channel.BasicPublish(exchange: "",
                                             routingKey: "security_monitor",
                                             basicProperties: null,
                                             body: body);

                        Console.WriteLine($"Команда отправлена");
                        break;
                    case "script":
                        if (input[2] != "begin" || input.Length != 4)
                            Console.WriteLine("Некорректный формат входной команды");

                        var script = command;
                        while ((command = Console.ReadLine()) != "end")
                        {
                            if (string.IsNullOrEmpty(command))
                                continue;

                            command = command.Trim().ToLower();

                            var inputScript = command.Split();

                            switch (inputScript[0])
                            {
                                case "пылесос":
                                    if (inputScript.Length != 6 || inputScript[1] != ":" ||
                                        (inputScript[2] != "сухая" && inputScript[2] != "влажная") ||
                                        inputScript[3] != "уборка" || inputScript[4] != "-")
                                    {
                                        Console.WriteLine("Некорректный формат входной команды");
                                        break;
                                    }

                                    script += " command " + command;
                                    break;
                                case "жалюзи":
                                    if (inputScript.Length != 3 || inputScript[1] != ":" || inputScript[2] != "поднять" && inputScript[2] != "опустить")
                                    {
                                        Console.WriteLine("Некорректный формат входной команды");
                                        break;
                                    }

                                    script += " command " + command;
                                    break;
                                case "климат-контроль":
                                    if (inputScript.Length != 4 || inputScript[1] != ":" || inputScript[2] != "температура" ||
                                        (!inputScript[3].StartsWith('+') && !inputScript[3].StartsWith("-")) ||
                                        !inputScript[3].Skip(1).All(char.IsDigit))
                                    {
                                        Console.WriteLine("Некорректный формат входной команды");
                                        break;
                                    }

                                    script += " command " + command;
                                    break;
                                case "свет":
                                    if (inputScript.Length != 3 || inputScript[1] != ":" || (inputScript[2] != "включить" && inputScript[2] != "выключить"))
                                    {
                                        Console.WriteLine("Некорректный формат входной команды");
                                        break;
                                    }

                                    script += " command " + command;
                                    break;
                                default:
                                    Console.WriteLine("Компонент не опознан");
                                    break;
                            }
                        }
                        body = Encoding.UTF8.GetBytes("input_command_handler " + script);
                        channel.BasicPublish(exchange: "",
                                             routingKey: "security_monitor",
                                             basicProperties: null,
                                             body: body);
                        break;
                    case "start":
                        if(input.Length != 3)
                        {
                            Console.WriteLine("Некорректный формат входной команды");
                            break;
                        }
                        channel.BasicPublish(exchange: "",
                                             routingKey: "security_monitor",
                                             basicProperties: null,
                                             body: body);
                        break;

                    default:
                        Console.WriteLine("Компонент не опознан");
                        break;
                }
            }
        }
    }
}

