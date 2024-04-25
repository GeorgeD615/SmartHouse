using RabbitMQ.Client;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Обработчик входящих команд");
        Console.ForegroundColor = ConsoleColor.White;

        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {

            channel.QueueDeclare(queue: "for_non_critical_command",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
            //TODO : display message about possible command templates

            string command;
            while((command = Console.ReadLine()) != "exit")
            {
                //TODO : process the command
                if (string.IsNullOrEmpty(command))
                    continue;


                //if the command is not critical

                var body = Encoding.UTF8.GetBytes(command);

                channel.BasicPublish(exchange: "",
                                     routingKey: "for_non_critical_command",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($"Команда отправлена");
            }
        }
    }
}

