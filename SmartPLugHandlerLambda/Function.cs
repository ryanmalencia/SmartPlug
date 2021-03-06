using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SmartPLugHandlerLambda.Request;
using SmartPLugHandlerLambda.Response;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SmartPLugHandlerLambda
{
    public class Function
    {
        public object FunctionHandler(object input, ILambdaContext context)
        {
            // Get request object
            Request request = JsonConvert.DeserializeObject<Request>(input.ToString());
            string requestType = request.Directive.Header.Name;
            LambdaLogger.Log("Request type: " + requestType + Environment.NewLine);

            if (requestType == "Discover")
            {
                // Create response object
                Response response = new Response
                {
                    Event = new Event()
                };

                response.Event.Header = new Header();

                LambdaLogger.Log("DISCOVER: " + input.ToString());
                response.Event.Payload = new Payload
                {
                    Endpoints = new List<Endpoint>()
                };
                LambdaLogger.Log("Discovering devices" + Environment.NewLine);
                // Set header properties
                response.Event.Header.Namespace = "Alexa.Discovery";
                response.Event.Header.Name = "Discover.Response";
                response.Event.Header.PayloadVersion = "3";
                response.Event.Header.MessageId = request.Directive.Header.MessageId;

                // Create endpoint
                Endpoint ep1 = new Endpoint
                {
                    EndpointId = "endpoint-001",
                    ManufacturerName = "Ryan Malencia",
                    FriendlyName = "Apple Jacks",
                    Description = "This is a switch!",
                    DisplayCategories = new List<string>() { "SMARTPLUG" }
                };
                ep1.Capabilities = new List<Capability>();

                // Create capabilities
                Capability cap1 = new Capability
                {
                    Type = "AlexaInterface",
                    Interface = "Alexa",
                    Version = "3"
                };
                Capability cap2 = new Capability
                {
                    Type = "AlexaInterface",
                    Interface = "Alexa.PowerController",
                    Version = "3"
                };

                // Create properties
                Properties p1 = new Properties();
                Supported s1 = new Supported
                {
                    Name = "powerState"
                };
                p1.Supported = new List<Supported>();
                p1.Supported.Add(s1);
                //p1.ProactivelyReported = true;
                //p1.Retrievable = true;
                cap2.Properties = p1;

                // Set capabilities
                ep1.Capabilities.Add(cap1);
                ep1.Capabilities.Add(cap2);

                // Add endpoint to response
                response.Event.Payload.Endpoints.Add(ep1);

                return response;
            }
            else if (requestType == "TurnOff")
            {
                PowerRequest powerRequest = JsonConvert.DeserializeObject<PowerRequest>(input.ToString());
                PowerResponse powerResponse = new PowerResponse
                {
                    Context = new Context
                    {
                        Properties = new List<Property>()
                    }
                };
                Property p1 = new Property
                {
                    Namespace = "Alexa.PowerController",
                    Name = "powerState",
                    Value = "OFF",
                    TimeOfSample = DateTime.Now,
                    UncertaintyInMilliseconds = 200
                };
                Property p2 = new Property
                {
                    Namespace = "Alexa.EndpointHealth",
                    Name = "connectivity",
                    Value = "OK",
                    TimeOfSample = DateTime.Now,
                    UncertaintyInMilliseconds = 200
                };
                powerResponse.Context.Properties.Add(p1);
                powerResponse.Context.Properties.Add(p2);

                powerResponse.Event = new SmartPLugHandlerLambda.Response.Event
                {
                    Header = new SmartPLugHandlerLambda.Response.Header
                    {
                        Namespace = "Alexa",
                        Name = "Response",
                        PayloadVersion = "3",
                        MessageId = powerRequest.Directive.Header.MessageId,
                        CorrelationToken = powerRequest.Directive.Header.CorrelationToken
                    },

                    Endpoint = new SmartPLugHandlerLambda.Response.Endpoint
                    {
                        Scope = new SmartPLugHandlerLambda.Response.Scope
                        {
                            Type = powerRequest.Directive.Endpoint.Scope.Type,
                            Token = powerRequest.Directive.Endpoint.Scope.Token
                        },
                        EndpointId = powerRequest.Directive.Endpoint.EndpointId,
                    },

                    Payload = new SmartPLugHandlerLambda.Response.Payload()
                };


                LambdaLogger.Log("OFF: " + input.ToString());
                const string queueUrl = "https://sqs.us-east-1.amazonaws.com/320502343338/SmartPlugQueue";
                var config = new AmazonSQSConfig
                {
                    ServiceURL = "http://sqs.us-east-1.amazonaws.com"
                };
                var client = new AmazonSQSClient(config);
                var sendMessage = client.SendMessageAsync(queueUrl, "OFF", new System.Threading.CancellationToken());
                while (!sendMessage.IsCompleted)
                { }
                return powerResponse;
            }
            else if (requestType == "TurnOn")
            {
                PowerRequest powerRequest = JsonConvert.DeserializeObject<PowerRequest>(input.ToString());
                PowerResponse powerResponse = new PowerResponse
                {
                    Context = new Context
                    {
                        Properties = new List<Property>()
                    }
                };
                Property p1 = new Property
                {
                    Namespace = "Alexa.PowerController",
                    Name = "powerState",
                    Value = "ON",
                    TimeOfSample = DateTime.Now,
                    UncertaintyInMilliseconds = 200
                };
                Property p2 = new Property
                {
                    Namespace = "Alexa.EndpointHealth",
                    Name = "connectivity",
                    Value = "OK",
                    TimeOfSample = DateTime.Now,
                    UncertaintyInMilliseconds = 200
                };
                powerResponse.Context.Properties.Add(p1);
                powerResponse.Context.Properties.Add(p2);

                powerResponse.Event = new SmartPLugHandlerLambda.Response.Event
                {
                    Header = new SmartPLugHandlerLambda.Response.Header
                    {
                        Namespace = "Alexa",
                        Name = "Response",
                        PayloadVersion = "3",
                        MessageId = powerRequest.Directive.Header.MessageId,
                        CorrelationToken = powerRequest.Directive.Header.CorrelationToken
                    },

                    Endpoint = new SmartPLugHandlerLambda.Response.Endpoint
                    {
                        Scope = new SmartPLugHandlerLambda.Response.Scope
                        {
                            Type = powerRequest.Directive.Endpoint.Scope.Type,
                            Token = powerRequest.Directive.Endpoint.Scope.Token
                        },
                        EndpointId = powerRequest.Directive.Endpoint.EndpointId,
                    },

                    Payload = new SmartPLugHandlerLambda.Response.Payload()
                };

                LambdaLogger.Log("ON: " + input.ToString());
                const string queueUrl = "https://sqs.us-east-1.amazonaws.com/320502343338/SmartPlugQueue";
                var config = new AmazonSQSConfig
                {
                    ServiceURL = "http://sqs.us-east-1.amazonaws.com"
                };
                var client = new AmazonSQSClient(config);
                var sendMessage = client.SendMessageAsync(queueUrl, "ON", new System.Threading.CancellationToken());
                while (!sendMessage.IsCompleted)
                { }
                return powerResponse;
            }
            return new Response();
        }

#region RESPONSE
        public class Header
        {
            [JsonProperty("namespace")]
            public string Namespace{ get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("payloadVersion")]
            public string PayloadVersion { get; set; }

            [JsonProperty("messageId")]
            public string MessageId { get; set; }
        }

        public class Cookie
        {
            [JsonProperty("detail1")]
            public string Detail1 { get; set; }

            [JsonProperty("detail2")]
            public string Detail2 { get; set; }
        }

        public class Supported
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class Properties
        {
            [JsonProperty("supported")]
            public IList<Supported> Supported { get; set; }

            [JsonProperty("proactivelyReported")]
            public bool ProactivelyReported { get; set; }

            [JsonProperty("retrievable")]
            public bool Retrievable { get; set; }
        }

        public class Resolution
        {
            [JsonProperty("width")]
            public int Width { get; set; }

            [JsonProperty("height")]
            public int Height { get; set; }
        }

        public class Capability
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("interface")]
            public string Interface{get; set; }

            [JsonProperty("version")]
            public string Version { get; set; }

            [JsonProperty("properties")]
            public Properties Properties { get; set; }

            [JsonProperty("supportsDeactivation")]
            public bool? SupportsDeactivation { get; set; }

            [JsonProperty("proactivelyReported")]
            public bool? ProactivelyReported { get; set; }
        }

        public class Endpoint
        {
            [JsonProperty("endpointId")]
            public string EndpointId { get; set; }

            [JsonProperty("manufacturerName")]
            public string ManufacturerName { get; set; }

            [JsonProperty("friendlyName")]
            public string FriendlyName { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("displayCategories")]
            public IList<string> DisplayCategories { get; set; }

            [JsonProperty("cookie")]
            public Cookie Cookie { get; set; }

            [JsonProperty("capabilities")]
            public IList<Capability> Capabilities { get; set; }
        }

        public class Payload
        {
            [JsonProperty("endpoints")]
            public IList<Endpoint> Endpoints { get; set; }
        }

        public class Event
        {
            [JsonProperty("header")]
            public Header Header { get; set; }

            [JsonProperty("payload")]
            public Payload Payload { get; set; }
        }

        public class Response
        {
            [JsonProperty("event")]
            public Event Event { get; set; }
        }
#endregion

#region REQUEST
        public class RequestHeader
        {
            [JsonProperty("namespace")]
            public string Namespace { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("payloadVersion")]
            public string PayloadVersion { get; set; }

            [JsonProperty("messageId")]
            public string MessageId { get; set; }
        }

        public class RequestScope
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("token")]
            public string Token { get; set; }
        }

        public class RequestPayload
        {
            [JsonProperty("scope")]
            public RequestScope Scope { get; set; }
        }

        public class Request
        {
            [JsonProperty("directive")]
            public Directive Directive { get; set; }
        }

        public class Directive
        {
            [JsonProperty("header")]
            public RequestHeader Header { get; set; }

            [JsonProperty("payload")]
            public RequestPayload Payload { get; set; }
        }
#endregion
    }
}