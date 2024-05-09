using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    public class Script
    {
        public string Name { get; set; }
        public List<string> Commands { get; set; } = new();
    }

    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Библиотека пользовательских сценариев");
        Console.ForegroundColor = ConsoleColor.White;

        var scripts = new List<Script>();


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
                switch (input[0])
                {
                    case "system_for_reading_and_writing_user_scripts":
                        switch (input[1])
                        {
                            case "script":
                                var newScript = new Script();
                                newScript.Name = input[2];
                                var commands = input.Skip(6);
                                string newCommand = string.Empty;
                                foreach (var c in commands)
                                {
                                    if (c == "command")
                                    {
                                        newScript.Commands.Add(newCommand.Trim());
                                        newCommand = string.Empty;
                                    }
                                    else
                                        newCommand += $"{c} ";
                                }
                                newScript.Commands.Add(newCommand);
                                newCommand = string.Empty;
                                scripts.Add(newScript);
                                Console.WriteLine($"Сценарий {newScript.Name} сохранён");
                                break;
                            case "start":
                                var script = scripts.FirstOrDefault(s => s.Name == input[2]);

                                if(script == null)
                                {
                                    Console.WriteLine($"Сценарий {input[2]} не найден.");
                                    break;
                                }

                                foreach (var c in script.Commands)
                                {
                                    var body = Encoding.UTF8.GetBytes("сustom_script_library " + c);
                                    channel.BasicPublish(exchange: "",
                                                         routingKey: "security_monitor",
                                                         basicProperties: null,
                                                         body: body);
                                }
                                Console.WriteLine("Сценарий найден. Команды отправлены.");
                                break;
                                   
                        }
                        break;
                    default:
                        Console.WriteLine("Отправитель не опознан");
                        break;
                }

            };
            channel.BasicConsume(queue: "сustom_script_library",
                                    autoAck: true,
                                    consumer: commands_consumer);

            while (Console.ReadLine() != "exit") { }
        }
    }
}
