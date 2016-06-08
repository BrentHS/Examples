using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Nop.Integration.WebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        private static string _servicesBusConnectionString;
        private static NamespaceManager _namespaceManager;

        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        public static void Main()
        {
            _servicesBusConnectionString = AmbientConnectionStringProvider.Instance.GetConnectionString(ConnectionStringNames.ServiceBus);
            _namespaceManager = NamespaceManager.CreateFromConnectionString(_servicesBusConnectionString);
            CreateStartMessage();

            JobHostConfiguration config = new JobHostConfiguration();
            ServiceBusConfiguration serviceBusConfig = new ServiceBusConfiguration
            {
                ConnectionString = _servicesBusConnectionString
            };
            config.UseServiceBus(serviceBusConfig);

            var host = new JobHost();
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }

        private static void CreateStartMessage()
        {
            if (!_namespaceManager.QueueExists(Functions.QueueName))
            {
                _namespaceManager.CreateQueue(Functions.QueueName);
            }

            QueueClient queueClient = QueueClient.CreateFromConnectionString(_servicesBusConnectionString, Functions.QueueName);

            using (Stream stream = new MemoryStream())
            using (TextWriter writer = new StreamWriter(stream))
            {
                writer.Write("Start");
                writer.Flush();
                stream.Position = 0;

                queueClient.Send(new BrokeredMessage(stream) { ContentType = "text/plain" });
            }

            queueClient.Close();
        }
    }
}
