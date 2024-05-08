using RabbitMQ.Client;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Система анализа некритичных данных");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Формат входных некритичных данных:");
        Console.WriteLine("{показатель} : {значение_показателя}");
        Console.WriteLine("Показатели:");
        Console.WriteLine(" - Время");
        Console.WriteLine(" - Пыль");

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

            string data;
            while ((data = Console.ReadLine()) != "exit")
            {
                if (string.IsNullOrEmpty(data))
                    continue;

                data = data.Trim().ToLower();

                var input = data.Split();

                var body = Encoding.UTF8.GetBytes("non_critical_data_analysis_system " + data);

                if (input.Length != 3 || input[1] != ":" ||
                            (input[0] != "время" && input[0] != "пыль"))
                {
                    Console.WriteLine("Некорректный формат входной информации");
                    continue;
                }

                channel.BasicPublish(exchange: "",
                                     routingKey: "security_monitor",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($"Информация отправлена");
            }
        }
    }
}

