using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        //Thread.Sleep(20000);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Система анализа критичных данных");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Формат входных критичных данных:");
        Console.WriteLine("{показатель} : {значение_показателя}");
        Console.WriteLine("Показатели:");
        Console.WriteLine(" - Температура");
        Console.WriteLine(" - Свет");

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

            channel.QueueDeclare(queue: "critical_data_analysis_system",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            string data;
            var commands_consumer = new EventingBasicConsumer(channel);
            commands_consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                data = message.Trim().ToLower();

                var input = data.Split();

                var bodyOut = Encoding.UTF8.GetBytes("critical_data_analysis_system " + data);

                if (input.Length != 3 || input[1] != ":" ||
                            (input[0] != "температура" && input[0] != "свет"))
                {
                    Console.WriteLine("Некорректный формат входной информации");
                }
                else
                {
                    channel.BasicPublish(exchange: "",
                                         routingKey: "security_monitor",
                                         basicProperties: null,
                                         body: bodyOut);

                    Console.WriteLine($"Информация отправлена");
                }

            };
            channel.BasicConsume(queue: "critical_data_analysis_system",
                                    autoAck: true,
                                    consumer: commands_consumer);

            while ((data = Console.ReadLine()) != "exit")
            {
                if (string.IsNullOrEmpty(data))
                    continue;

                data = data.Trim().ToLower();

                var input = data.Split();

                var body = Encoding.UTF8.GetBytes("critical_data_analysis_system " + data);

                if (input.Length != 3 || input[1] != ":" ||
                            (input[0] != "температура" && input[0] != "свет"))
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

