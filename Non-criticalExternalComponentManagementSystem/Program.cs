using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Система управления некритичными внешними компонентами");
        Console.ForegroundColor = ConsoleColor.White;

        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            //Input command channel
            channel.QueueDeclare(queue: "for_non_critical_command",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            //Input non critical data channel
            channel.QueueDeclare(queue: "for_non_critical_data",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            //Output command for external components channel
            channel.QueueDeclare(queue: "for_non_critical_command_from_manager",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);


            //Input command receving
            var commands_consumer = new EventingBasicConsumer(channel);
            commands_consumer.Received += (model, ea) =>
            {
                var receiving_command = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(receiving_command);
                Console.WriteLine($"Получил команду: {message}");

                //TODO : process the command

                var sending_command = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                                     routingKey: "for_non_critical_command_from_manager",
                                     basicProperties: null,
                                     body: sending_command);

                Console.WriteLine($"Команда отправлена");

            };
            channel.BasicConsume(queue: "for_non_critical_command",
                                    autoAck: true,
                                    consumer: commands_consumer);

            //Outside data receving
            var outside_data_consumer = new EventingBasicConsumer(channel);
            outside_data_consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Получил информацию: {message}");

                //TODO : process the data and sent command if you need
            };
            channel.BasicConsume(queue: "for_non_critical_data",
                                    autoAck: true,
                                    consumer: outside_data_consumer);

            while (Console.ReadLine() != "exit") { }
        }
    }
}