using System;
using System.Linq;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace SmartPlug
{
    class Program
    {
        static void Main(string[] args)
        {
            const string queueUrl = "https://sqs.us-east-1.amazonaws.com/320502343338/SmartPlugQueue";
            var config = new AmazonSQSConfig
            {
                ServiceURL = "http://sqs.us-east-1.amazonaws.com"
            };
            var client = new AmazonSQSClient(config);
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl,
                WaitTimeSeconds = 20
            };
            while(true)
            {
                Console.WriteLine("Waiting: ");
                var receiveMessageResponse = client.ReceiveMessage(receiveMessageRequest);
                foreach(var message in receiveMessageResponse.Messages)
                {
                    Console.WriteLine("Received: " + receiveMessageResponse.Messages.FirstOrDefault()?.Body);
                    client.DeleteMessage(new DeleteMessageRequest
                    {
                        QueueUrl = queueUrl,
                        ReceiptHandle = message.ReceiptHandle
                    });
                }
            }
        }
    }
}
