using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurityMonitor;

namespace Tests
{
    [TestClass]
    public class MonitorUnitTests
    {
        [TestMethod]
        public void NonCrticalCommandTest()
        {
            var command = "input_command_handler жалюзи : поднять";
            var checker = Policies.CheckOperation(command.Split().ToArray());
            if (checker.Item1 && checker.Item2 == "non_critical_external_component_management_system")
                return;

            Assert.Fail();
        }

        [TestMethod]
        public void CriticalCommandTest()
        {
            var command = "input_command_handler свет : включить qwerty";
            var checker = Policies.CheckOperation(command.Split().ToArray());
            if (checker.Item1 && checker.Item2 == "authorization_system")
                return;

            Assert.Fail();
        }

        [TestMethod]
        public void UnknownSenderTest()
        {
            var command = "uncknown_sender bad_request";
            var checker = Policies.CheckOperation(command.Split().ToArray());
            if (!checker.Item1 && checker.Item2 == string.Empty)
                return;

            Assert.Fail();
        }

        [TestMethod]
        public void CriticalInformationTest()
        {
            var command = "critical_data_analysis_system температура : +20";
            var checker = Policies.CheckOperation(command.Split().ToArray());
            if (checker.Item1 && checker.Item2 == "critical_input_command_processing_system")
                return;

            Assert.Fail();
        }
    }
}
