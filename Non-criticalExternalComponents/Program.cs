using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Некритичные внешние компоненты");
        Console.ForegroundColor = ConsoleColor.White;
        //Thread.Sleep(40000);

        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            //Input command channel
            channel.QueueDeclare(queue: "for_non_critical_command_from_manager",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var commands_consumer = new EventingBasicConsumer(channel);
            commands_consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Получил команду: {message}");

                //TODO : process the command and execute a command

                Console.WriteLine($"Выполнил команду: {message}");
            };
            channel.BasicConsume(queue: "for_non_critical_command_from_manager",
                                    autoAck: true,
                                    consumer: commands_consumer);

            while (Console.ReadLine() != "exit") { }
        }
    }
}