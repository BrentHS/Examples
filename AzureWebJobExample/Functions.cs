using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;

namespace Nop.Integration.WebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public const string QueueName = "MyQueue";

        public static void ProcessQueueMessage([ServiceBusTrigger(QueueName)] BrokeredMessage message, TextWriter log)
        {
            log.WriteLine("Scheduled job fired!");
            log.WriteLine(message);
            log.WriteLine("Body: " + message.GetBody<string>());
            log.WriteLine("MessageID: " + message.MessageId);

            try
            {
                //Do some work

                message.Complete();
            }
            catch (Exception)
            {
                log.WriteLine("There was an issue processing this message. Abandoning message.");

                // Indicates a problem, unlock message in queue. Makes available to be recieved again.
                message.Abandon();
            }
        }
    }
}