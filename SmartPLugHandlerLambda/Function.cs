using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SmartPlugHandlerLambda
{
    public class Function
    {
        public object FunctionHandler(object input, ILambdaContext context)
        {
            // Get request object
            Request request = JsonConvert.DeserializeObject<Request>(input.ToString());

            // Create response object
            Response response = new Response
            {
                Event = new Event()
            };

            response.Event.Header = new Header();
            response.Event.Payload = new Payload
            {
                Endpoints = new List<Endpoint>()
            };

            // Set header properties
            response.Event.Header.Namespace = "Alexa.Discovery";
            response.Event.Header.Name = "Discover.Response";
            response.Event.Header.PayloadVersion = "3";
            response.Event.Header.MessageId = "0a58ace0-e6ab-47de-b6af-b600b5ab8a81"; //no idea if this is okay

            // Create endpoint
            Endpoint ep1 = new Endpoint
            {
                EndpointId = "endpoint-001",
                ManufacturerName = "Ryan Malencia",
                FriendlyName = "Plug",
                Description = "This is a plug!",
                DisplayCategories = new List<string>(){ "SWITCH" }
            };
            ep1.Capabilities = new List<Capability>();

            // Create cookie
            Cookie c1 = new Cookie
            {
                Detail1 = "This is a plug",
                Detail2 = "that can be controlled"
            };
            ep1.Cookie = c1;

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
            p1.ProactivelyReported = true;
            p1.Retrievable = true;
            cap2.Properties = p1;

            // Set capabilities
            ep1.Capabilities.Add(cap1);
            ep1.Capabilities.Add(cap2);

            // Add endpoint to response
            response.Event.Payload.Endpoints.Add(ep1);

            // Create json string of response
            string responsestring = JsonConvert.SerializeObject(response);

            // Log response
            LambdaLogger.Log("Response: " + responsestring + Environment.NewLine);

            // Return response. Not sure why we have to serialize again but ya know
            return JsonConvert.SerializeObject(responsestring);
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

        public class CameraStreamConfiguration
        {
            [JsonProperty("protocols")]
            public IList<string> Protocols { get; set; }

            [JsonProperty("resolutions")]
            public IList<Resolution> Resolutions { get; set; }

            [JsonProperty("authorizationTypes")]
            public IList<string> AuthorizationTypes { get; set; }

            [JsonProperty("videoCodecs")]
            public IList<string> VideoCodecs { get; set; }

            [JsonProperty("audioCodecs")]
            public IList<string> AudioCodecs { get; set; }
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

            [JsonProperty("cameraStreamConfigurations")]
            public IList<CameraStreamConfiguration> CameraStreamConfigurations { get; set; }
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

        [JsonObject("directive")]
        public class Request
        {
            [JsonProperty("header")]
            public RequestHeader Header { get; set; }

            [JsonProperty("payload")]
            public RequestPayload Payload { get; set; }
        }
#endregion
    }
}