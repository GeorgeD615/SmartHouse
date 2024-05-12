using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class MonitorUnitTests
    {
        [TestMethod]
        public void NonCrticalCommandTest()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string input = string.Empty;
                channel.QueueDeclare(queue: "test_out",
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
                channel.BasicConsume(queue: "test_out",
                                        autoAck: true,
                                        consumer: commands_consumer);

                Thread.Sleep(5000);
                input = string.Empty;

                var body = Encoding.UTF8.GetBytes($"пылесос : сухая уборка - спальня");
                channel.BasicPublish(exchange: "",
                                             routingKey: "input_command_handler",
                                             basicProperties: null,
                                             body: body);

                Thread.Sleep(20000);
                Assert.AreEqual("Получил сообщение от input_command_handler\n" +
                    "Команда отправлена в non_critical_external_component_management_system\n" +
                    "Получил сообщение от non_critical_external_component_management_system\n" +
                    "Команда отправлена в non_critical_external_components\n", input);
            }
        }

        [TestMethod]
        public void CriticalCommandTest()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string input = string.Empty;
                channel.QueueDeclare(queue: "test_out",
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
                channel.BasicConsume(queue: "test_out",
                                        autoAck: true,
                                        consumer: commands_consumer);

                Thread.Sleep(5000);
                input = string.Empty;
                var body = Encoding.UTF8.GetBytes($"свет : выключить qwerty");
                channel.BasicPublish(exchange: "",
                                             routingKey: "input_command_handler",
                                             basicProperties: null,
                                             body: body);

                Thread.Sleep(20000);
                Assert.AreEqual("Получил сообщение от input_command_handler\n"+
                                "Команда отправлена в authorization_system\n" +
                                "Получил сообщение от authorization_system\n" +
                                "Авторизация подтверждена\n" +
                                "Команда отправлена в critical_input_command_processing_system\n" +
                                "Получил сообщение от critical_input_command_processing_system\n" +
                                "Команда отправлена в critical_external_component_management_system\n" +
                                "Получил сообщение от critical_external_component_management_system\n" +
                                "Команда отправлена в critical_external_components\n", input);
            }
        }

        [TestMethod]
        public void CriticalInformationTest()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string input = string.Empty;
                channel.QueueDeclare(queue: "test_out",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                channel.QueueDeclare(queue: "critical_data_analysis_system",
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
                channel.BasicConsume(queue: "test_out",
                                        autoAck: true,
                                        consumer: commands_consumer);

                Thread.Sleep(5000);
                input = string.Empty;

                var body = Encoding.UTF8.GetBytes($"температура : +30");
                channel.BasicPublish(exchange: "",
                                             routingKey: "critical_data_analysis_system",
                                             basicProperties: null,
                                             body: body);

                Thread.Sleep(20000);
                Assert.AreEqual("Получил сообщение от critical_data_analysis_system\n" +
                                "Информация отправлена в critical_input_command_processing_system\n", input);
            }
        }
    }
}
