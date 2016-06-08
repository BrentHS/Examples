using System;
using System.Collections.Generic;
using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace AzureServiceBusExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start.");
            string serviceBusConnectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionStringRaven");
            //string serviceBusConnectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.WindowsConnectionString");

            var queues = GetQueues(serviceBusConnectionString);
            foreach (var queue in queues)
            {
                Console.WriteLine("Name: " + queue.Path);
            }

            //CreateQueue(serviceBusConnectionString, "foo");
            CreateQueue(serviceBusConnectionString, "customers");
            CreateMessage(serviceBusConnectionString, "customers");
            CreateMessage(serviceBusConnectionString, "customers");
            CreateMessage(serviceBusConnectionString, "customers");
            //RecieveMessage(serviceBusConnectionString, "customers");

            var queues2 = GetQueues(serviceBusConnectionString);
            foreach (var queue in queues2)
            {
                Console.WriteLine("Name: " + queue.Path);
            }

            Console.WriteLine("End.");
            Console.ReadLine();
        }


        public static IEnumerable<QueueDescription> GetQueues(string serviceBusConnectionString)
        {
            var nameSpaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            var queues = GetQueues(nameSpaceManager);
            return queues;
        }

        public static IEnumerable<QueueDescription> GetQueues(NamespaceManager nameSpaceManager)
        {
            var queues = nameSpaceManager.GetQueues();
            return queues;
        }

        /// <summary>
        /// Creates the queue. If the queue already exists then nothing is done.
        /// </summary>
        /// <param name="serviceBusConnectionString">The service bus connection string.</param>
        /// <param name="queueName">Name of the queue.</param>
        public static void CreateQueue(string serviceBusConnectionString, string queueName)
        {
            var nameSpaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            CreateQueue(nameSpaceManager, queueName);
        }

        /// <summary>
        /// Creates the queue. If the queue already exists then nothing is done.
        /// </summary>
        /// <param name="nameSpaceManager">The name space manager.</param>
        /// <param name="queueName">Name of the queue.</param>
        public static void CreateQueue(NamespaceManager nameSpaceManager, string queueName)
        {
            if (!nameSpaceManager.QueueExists(queueName))
            {
                var qd = new QueueDescription(queueName)
                {
                    MaxSizeInMegabytes = 5120,
                    DefaultMessageTimeToLive = new TimeSpan(0, 1, 0)
                    //IsAnonymousAccessible = true
                };

                nameSpaceManager.CreateQueue(qd);
                nameSpaceManager.GetQueues();
            }
        }
        public static void DeleteQueue(string serviceBusConnectionString, string queueName)
        {
            var nameSpaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);

            DeleteQueue(nameSpaceManager, queueName);
        }
        public static void DeleteQueue(NamespaceManager nameSpaceManager, string queueName)
        {
            if (nameSpaceManager.QueueExists(queueName))
            {
                nameSpaceManager.DeleteQueue(queueName);
            }
        }

        public static void CreateMessage(string serviceBusConnectionString, string queueName)
        {
            var queueClient = QueueClient.CreateFromConnectionString(serviceBusConnectionString, queueName);

            // Create message, passing a string message for the body.
            var message = new BrokeredMessage("Test message");

            // Set some addtional custom app-specific properties.
            message.Properties["TestProperty"] = "TestValue";
            message.Properties["Message number"] = 1;

            // Send message to the queue.
            queueClient.Send(message);
        }

        public static void RecieveMessage(string serviceBusConnectionString, string queueName)
        {
            var client = QueueClient.CreateFromConnectionString(serviceBusConnectionString, queueName);

            // Configure the callback options.
            var options = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes(1)
            };

            // Callback to handle received messages.
            client.OnMessage((message) =>
            {
                try
                {
                    // Process message from queue.
                    Console.WriteLine("Body: " + message.GetBody<string>());
                    Console.WriteLine("MessageID: " + message.MessageId);
                    Console.WriteLine("Test Property: " +
                    message.Properties["TestProperty"]);

                    // Remove message from queue.
                    message.Complete();
                }
                catch (Exception)
                {
                    // Indicates a problem, unlock message in queue. Makes available to be recieved again.
                    message.Abandon();
                }
            }, options);

        }
    }
}
