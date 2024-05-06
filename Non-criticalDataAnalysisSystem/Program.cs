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

        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "for_non_critical_data",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            //TODO : display message about possible data templates

            string data;
            while ((data = Console.ReadLine()) != "exit")
            {
                //TODO : process the data
                if (string.IsNullOrEmpty(data))
                    continue;


                var body = Encoding.UTF8.GetBytes(data);

                channel.BasicPublish(exchange: "",
                                     routingKey: "for_non_critical_data",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($"Информация отправлена");
            }
        }
    }
}

