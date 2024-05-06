using RabbitMQ.Client;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Обработчик входящих команд");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Команда должна посылаться в формате {название_внешнего_компонента} : {описание_команды}");
        Console.WriteLine("Описание команд для разных компонентов:");
        Console.WriteLine(" - Пылесос");
        Console.WriteLine("     сухая уборка - {название_комнаты}");
        Console.WriteLine("     влажная уборка - {название_комнаты}");
        Console.WriteLine(" - Жалюзи");
        Console.WriteLine("     поднять");
        Console.WriteLine("     опустить");
        Console.WriteLine(" - Климат-контроль");
        Console.WriteLine("     температура +{градусы по цельсию}");
        Console.WriteLine("     температура -{градусы по цельсию}");
        Console.WriteLine(" - Свет");
        Console.WriteLine("     включить");
        Console.WriteLine("     выкючить");


        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {

            channel.QueueDeclare(queue: "for_non_critical_command",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "for_critical_command",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            string command;
            while((command = Console.ReadLine()) != "exit")
            {
                if (string.IsNullOrEmpty(command))
                    continue;

                command = command.Trim().ToLower();

                var input = command.Split();

                var body = Encoding.UTF8.GetBytes(command);

                switch (input[0])
                {
                    case "пылесос":
                        if (input.Length != 6 || 
                            (input[2] != "сухая" && input[2] != "влажная") ||
                            input[3] != "уборка" || input[4] != "-" )
                        {
                            Console.WriteLine("Некорректный формат входной команды");
                            break;
                        }
                        channel.BasicPublish(exchange: "",
                                             routingKey: "for_non_critical_command",
                                             basicProperties: null,
                                             body: body);

                        Console.WriteLine($"Команда отправлена");
                        break;


                    case "жалюзи":
                        if (input.Length != 3 || input[2] != "поднять" && input[2] != "опустить")
                        {
                            Console.WriteLine("Некорректный формат входной команды");
                            break;
                        }
                        channel.BasicPublish(exchange: "",
                                             routingKey: "for_non_critical_command",
                                             basicProperties: null,
                                             body: body);

                        Console.WriteLine($"Команда отправлена");
                        break;

                    case "климат-контроль":
                        if (input.Length != 4 || input[2] != "температура" || 
                            (!input[3].StartsWith('+') && !input[3].StartsWith("-")) ||
                            input[3].Skip(1).All(char.IsDigit))
                        {
                            Console.WriteLine("Некорректный формат входной команды");
                            break;
                        }
                        channel.BasicPublish(exchange: "",
                                             routingKey: "for_critical_command",
                                             basicProperties: null,
                                             body: body);

                        Console.WriteLine($"Команда отправлена");
                        break;
                    case "свет":
                        if (input.Length != 3 || (input[2] != "включить" && input[2] != "выключить"))
                        {
                            Console.WriteLine("Некорректный формат входной команды");
                            break;
                        }

                        channel.BasicPublish(exchange: "",
                                             routingKey: "for_critical_command",
                                             basicProperties: null,
                                             body: body);

                        Console.WriteLine($"Команда отправлена");
                        break;

                    default:
                        Console.WriteLine("Компонент не опознан");
                        break;

                }

            }
        }
    }
}

