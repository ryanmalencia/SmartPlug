using System;
using System.Linq;
using Amazon.SQS;
using Amazon.SQS.Model;
using System.Net;

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
                    string command = receiveMessageResponse.Messages.FirstOrDefault()?.Body;
                    Console.WriteLine("Received: " + command);

                    if (command == "ON")
                    {
                        string http = "http://10.0.0.136:3000/api/plug/turnallon/1";
                        using (var cli = new WebClient())
                        {
                            cli.Headers.Add("content-type", "application/json");
                            cli.UploadString(http, "POST", "");
                        }
                    }
                    else if (command == "OFF")
                    {
                        string http = "http://10.0.0.136:3000/api/plug/turnalloff/1";
                        using (var cli = new WebClient())
                        {
                            cli.Headers.Add("content-type", "application/json");
                            cli.UploadString(http, "POST", "");
                        }
                    }
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
