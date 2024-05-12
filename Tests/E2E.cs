using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Tests
{
    [TestClass]
    public class E2E
    {
        [TestMethod]
        public void ScriptCreationAndImplementationTest()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string input = string.Empty;
                channel.QueueDeclare(queue: "e2e_test_out",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                channel.QueueDeclare(queue: "input_command_handler",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                var commands_consumer = new EventingBasicConsumer(channel);
                commands_consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    input += message + "\n";
                };
                channel.BasicConsume(queue: "e2e_test_out",
                                        autoAck: true,
                                        consumer: commands_consumer);

                var body = Encoding.UTF8.GetBytes($"script пробуждение begin qwerty command жалюзи : поднять command свет : включить");
                channel.BasicPublish(exchange: "",
                                             routingKey: "input_command_handler",
                                             basicProperties: null,
                                             body: body);

                Thread.Sleep(5000);

                input = string.Empty;

                body = Encoding.UTF8.GetBytes($"start пробуждение qwerty");
                channel.BasicPublish(exchange: "",
                                             routingKey: "input_command_handler",
                                             basicProperties: null,
                                             body: body);
                
                Thread.Sleep(15000);
                Assert.AreEqual("Выполнил команду: жалюзи : поднять\nВыполнил команду: свет : включить \n", input);
            }
        }
    }
}