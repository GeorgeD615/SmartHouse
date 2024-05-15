using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SecurityMonitor;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        //Thread.Sleep(20000);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Монитор безопасности");
        Console.ForegroundColor = ConsoleColor.White;


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

            channel.QueueDeclare(queue: "non_critical_external_component_management_system",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "non_critical_external_components",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "authorization_system",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "critical_input_command_processing_system",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "critical_external_component_management_system",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "critical_external_components",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueDeclare(queue: "system_for_reading_and_writing_user_scripts",
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
                byte[] bodyOut;
                Console.WriteLine($"Получил сообщение от {input[0]}");
                bodyOut = Encoding.UTF8.GetBytes($"Получил сообщение от {input[0]}");
                channel.BasicPublish(exchange: "",
                                     routingKey: "test_out",
                                     basicProperties: null,
                                     body: bodyOut);
                var body = Encoding.UTF8.GetBytes(string.Join(" ", input));
                var checkPolicies = Policies.CheckOperation(input);
                if (checkPolicies.Item1)
                {
                    channel.BasicPublish(exchange: "",
                                                     routingKey: checkPolicies.Item2,
                                                     basicProperties: null,
                                                     body: body);
                    Console.WriteLine($"Сообщение отправлено в {checkPolicies.Item2}");
                }
                else
                {
                    Console.WriteLine($"Ошибка авторизации");
                }
            };
            channel.BasicConsume(queue: "security_monitor",
                                    autoAck: true,
                                    consumer: commands_consumer);

            string command;
            while ((command = Console.ReadLine()) != "exit") {}
        }
    }
}


