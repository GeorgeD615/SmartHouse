using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Система авторизации");
        Console.ForegroundColor = ConsoleColor.White;
        var password = "qwerty";
        Console.WriteLine($"Password : {password}");

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

            channel.QueueDeclare(queue: "authorization_system",
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
                var body = Encoding.UTF8.GetBytes("authorization_system " + string.Join(" ", input.Skip(1)));
                switch (input[0])
                {
                    case "input_command_handler":
                        if(input.Last() == password)
                        {
                            channel.BasicPublish(exchange: "",
                                                         routingKey: "security_monitor",
                                                         basicProperties: null,
                                                         body: body);
                            Console.WriteLine("Команда отправлена");
                        }
                        else
                        {
                            Console.WriteLine("Неверный пароль");
                        }
                        break;
                    default:
                        Console.WriteLine("Отправитель не опознан");
                        break;
                }

            };
            channel.BasicConsume(queue: "authorization_system",
                                    autoAck: true,
                                    consumer: commands_consumer);

            while (Console.ReadLine() != "exit") { }
        }
    }
}
